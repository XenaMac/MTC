using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OctaHarness
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Form1 myForm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new Form1();
            Application.Run(myForm);
        }
    }
}
