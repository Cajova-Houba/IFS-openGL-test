using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IFS_openGL_test.ifs;

namespace IFS_openGL_test.GUI
{
    public partial class Okno : Form
    {
        //OpenGL
        private Zobrazovac zobrazovac;

        public Okno()
        {
            InitializeComponent();
        }

        private void bGeneruj_Click(object sender, EventArgs e)
        {
            //nacteni matic

            Bod[] fraktal;
            IFS ifs = new IFS();
            fraktal = ifs.sierpTrojuhelnikNahodne3D(150000);
            zobrazovac = new Zobrazovac(1024, 648, fraktal);
        }
    }
}
