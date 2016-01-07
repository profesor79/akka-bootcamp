// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="">
//   
// </copyright>
// <summary>
//   The main.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Main.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Akka.Actor;
    using Akka.Util.Internal;

    using ChartApp.Actors.ButtonToggleActor;
    using ChartApp.Actors.ChartingActor;
    using ChartApp.Actors.PerformanceCounterCoordinatorActor;
    using ChartApp.Enums;
    using ChartApp.Messages;

    #endregion

    /// <summary>
    ///     The main.
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        ///     The _series counter.
        /// </summary>
        private readonly AtomicCounter seriesCounter = new AtomicCounter(1);

        /// <summary>
        ///     The toggle actors.
        /// </summary>
        private readonly Dictionary<CounterType, IActorRef> toggleActors = new Dictionary<CounterType, IActorRef>();

        /// <summary>
        ///     The _chart actor.
        /// </summary>
        private IActorRef chartActor;

        /// <summary>
        ///     The coordinator actor.
        /// </summary>
        private IActorRef coordinatorActor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Main" /> class.
        /// </summary>
        public Main()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The cpu toggle click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CpuToggleClick(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Cpu].Tell(new Toggle());
        }

        /// <summary>
        /// The memory toggle click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MemoryToggleClick(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Memory].Tell(new Toggle());
        }

        /// <summary>
        /// The disk toggle click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DiskToggleClick(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Disk].Tell(new Toggle());
        }

        #region Initialization

        /// <summary>
        /// The main_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainLoad(object sender, EventArgs e)
        {
            this.chartActor =
                Program.ChartActors.ActorOf(
                    Props.Create(() => new ChartingActor(this.sysChart, this.PauseToggle)), 
                    "charting");
            this.chartActor.Tell(new InitializeChart(null)); // no initial series

            this.coordinatorActor =
                Program.ChartActors.ActorOf(
                    Props.Create(() => new PerformanceCounterCoordinatorActor(this.chartActor)), 
                    "counters");

            // CPU button toggle actor
            this.toggleActors[CounterType.Cpu] =
                Program.ChartActors.ActorOf(
                    Props.Create(
                        () => new ButtonToggleActor(this.coordinatorActor, this.CpuToggle, CounterType.Cpu, false))
                        .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // MEMORY button toggle actor
            this.toggleActors[CounterType.Memory] =
                Program.ChartActors.ActorOf(
                    Props.Create(
                        () => new ButtonToggleActor(this.coordinatorActor, this.MemoryToggle, CounterType.Memory, false))
                        .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // DISK button toggle actor
            this.toggleActors[CounterType.Disk] =
                Program.ChartActors.ActorOf(
                    Props.Create(
                        () => new ButtonToggleActor(this.coordinatorActor, this.DiskToggle, CounterType.Disk, false))
                        .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // Set the CPU toggle to ON so we start getting some data
            this.toggleActors[CounterType.Cpu].Tell(new Toggle());
        }

        /// <summary>
        /// The main_ form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            // shut down the charting actor
            this.chartActor.Tell(PoisonPill.Instance);

            // shut down the ActorSystem
            Program.ChartActors.Shutdown();
        }

        #endregion

        /// <summary>
        /// The pause toggle click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PauseToggleClick(object sender, EventArgs e)
        {
            this.chartActor.Tell(new TogglePause());
        }
    }
}