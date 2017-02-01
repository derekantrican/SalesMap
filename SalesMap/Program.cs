using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new SalesMapSearch());
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                {
                    Common.Log("Uhandled exception " + ex.Message);
                    Common.Log("Stack trace " + ex.StackTrace, false);
                }

                System.Windows.Forms.MessageBox.Show("There was an unhandled exception. Please contact the developer and relay this information: \n\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
