using GasBraai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Braai
{
    public class GasBraai
    {
        public GasBraai()
        {

        }

        private bool _lidOpen;
        /// <summary>
        /// Returns true if the gas braai lid is open.
        /// </summary>
        public bool LidOpen
        {
            get
            {
                return _lidOpen;
            }
            set
            {
                _lidOpen = value;
            }
        }

        /// <summary>
        /// Maintains the status of gas release.;
        /// </summary>
        /// <remarks>Gas flow status can be retrieved from the GasStatus.GasIsFlowing property.</remarks>
        public RecievingGasFlowEventArgs GasStatus { get; private set; }

        public event EventHandler<RecievingGasFlowEventArgs> GasIsFlowing;
        /// <summary>
        /// Raises the GasIsFlowing event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnGasIsFlowingChanged(RecievingGasFlowEventArgs e)
        {
            EventHandler<RecievingGasFlowEventArgs> handler = GasIsFlowing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private double _flowSpeed;
        /// <summary>
        /// Maintains a value representing the gas release speed;
        /// </summary>
        public double FlowSpeed
        {
            get
            {
                return _flowSpeed;
            }
            private set
            {
                _flowSpeed = value;
            }
        }
        /// <summary>
        /// Sets the speed of the gas released;
        /// </summary>
        /// <param name="speed">Gas release speed expressed as percentage from 0%(off) to 100%(full throttle)</param>
        public void SetBurner(double speed)
        {
            System.Diagnostics.Trace.TraceInformation("Setting the gas flow release speed.");

            if (speed > 100 || speed < 0)
            {
                throw new InvalidOperationException("Burner speed must be between 0(off) and 100(full throttle).");
            }

            FlowSpeed = speed;
            GasStatus = new RecievingGasFlowEventArgs(FlowSpeed > 0 && GasTankVolume > 0);
            OnGasIsFlowingChanged(GasStatus);

            System.Diagnostics.Trace.TraceInformation(string.Format("Gas speed setting {0}%.", FlowSpeed));
        }

        /// <summary>
        /// Maintains a value representing the volume of gas in the tank.
        /// </summary>
        public double GasTankVolume { get; private set; }
        /// <summary>
        /// Sets the gas tanks remaining volume.
        /// </summary>
        /// <param name="volume">volume of gas in the tank in Ml.</param>
        public void SetGasTankVolume(double volume)
        {
            System.Diagnostics.Trace.TraceInformation("Setting the gas tank volume.");

            GasTankVolume = volume;
            GasStatus = new RecievingGasFlowEventArgs(FlowSpeed > 0 && GasTankVolume > 0);
            OnGasIsFlowingChanged(GasStatus);

            System.Diagnostics.Trace.TraceInformation(string.Format("Remaining gas is now {0}Ml.", GasTankVolume));

        }

        /// <summary>
        /// Returns true if you can start cooking;
        /// </summary>
        /// <remarks>Returns true if flame is on. This is determined by the gas flow release speed, the gas tank having gas, and the gas having been ignited.</remarks>
        public bool Fire { get; private set; }

        /// <summary>
        /// Triggers a spark to ignite gas.
        /// </summary>
        public void IgniteBurner()
        {
            System.Diagnostics.Trace.TraceInformation("Attempting to spark the Gas.");

            if (FlowSpeed > 0 && GasTankVolume > 0
                && !LidOpen)
            {
                throw new InvalidOperationException("Kabooooom.");
            }

            //Spark
            Fire = GasStatus.GasIsFlowing;

            System.Diagnostics.Trace.TraceInformation("Sparked the Gas.");
        }
    }
}
