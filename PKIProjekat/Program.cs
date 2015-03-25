using log4net;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKIProjekat
{
    static class Program
    {
        /*
        [DllImport("kernel32.dll", SetLastError=true, CallingConvention=CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError=true, CallingConvention=CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();
        */
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
               
        }
    }
}
