using System;
using IFS_openGL_test.ifs;

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
            Bod[] fraktal;
            IFS ifs = new IFS();
            //fraktal = ifs.sierpTrojuhelnikNahodne(10);
            fraktal = ifs.sierpTrojuhelnikNahodne3D(150000);
            Zobrazovac zobrazovac = new Zobrazovac(1024, 648, fraktal);
        }
    }
}
