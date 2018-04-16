using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasBraai
{
    public class RecievingGasFlowEventArgs : EventArgs
    {
        public RecievingGasFlowEventArgs(bool gasIsFlowing)
        {
            GasIsFlowing = gasIsFlowing;
        }
        
        /// <summary>
        /// Returns true if gas is currently being released.
        /// </summary>
        public bool GasIsFlowing { get; set; }
    }
}
