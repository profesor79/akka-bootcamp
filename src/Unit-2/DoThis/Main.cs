// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="">
//   
// </copyright>
// <summary>
//   The main.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Akka.Actor;
using Akka.Util.Internal;

using ChartApp.Actors;

namespace ChartApp
{
    /// <summary>
    /// The main.
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        /// The _chart actor.
        /// </summary>
        private IActorRef chartActor;

        /// <summary>
        /// The _series counter.
        /// </summary>
        private readonly AtomicCounter seriesCounter = new AtomicCounter(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            this.InitializeComponent();
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
            this.chartActor = Program.ChartActors.ActorOf(Props.Create(() => new ChartingActor(this.sysChart)), "charting");
            var series = ChartDataHelper.RandomSeries("FakeSeries" + this.seriesCounter.GetAndIncrement());
            this.chartActor.Tell(new InitializeChart(new Dictionary<string, Series>()
            {
                {series.Name, series}
            }));
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
    }
}
