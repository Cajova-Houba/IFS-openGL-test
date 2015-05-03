using System;
using System.Windows.Forms;
using IFS_openGL_test.ifs;
using IFS_openGL_test.GUI;

namespace IFS_openGL_test
{
    static class Program
    {
        static Zobrazovac zobrazovac;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Okno());
        }
    }
}
