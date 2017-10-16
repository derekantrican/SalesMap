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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                SalesMapSearch mainWindow = new SalesMapSearch();

                if (args.Count() > 0)
                    mainWindow.CommandLineSelect = args.First();

                Application.Run(mainWindow);
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
