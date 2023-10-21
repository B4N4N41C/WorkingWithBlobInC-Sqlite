using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CS_SQLite_blob
{
    static class Program
    {
        /// <summary>
       
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
