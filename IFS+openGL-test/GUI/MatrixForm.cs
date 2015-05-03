using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IFS_openGL_test.ifs;

namespace IFS_openGL_test.GUI
{
    public partial class MatrixForm : Component
    {
        //zadávací komponenty
        TextBox[,] tbMatice; //matice 3x3
        TextBox tbDx;
        TextBox tbDy;
        TextBox tbDz;
        TextBox tbProbability;

        public MatrixForm()
        {
            InitializeComponent();
        }

        public MatrixForm(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Metoda inicializuje zadávací komponenty.
        /// </summary>
        private void inic()
        {
            //zadávání matice
            tbMatice = new TextBox[3,3];
            for (int i = 0; i < tbMatice.GetLength(0); i++)
            {
                for (int j = 0; j < tbMatice.GetLength(1); j++)
                {
                    tbMatice[i, j] = new TextBox();
                    tbMatice[i, j].Text = i + "," + j;
                    this.Container.Add(tbMatice[i, j]);
                }
            }

            //zadávání dx,dy,dz
            tbDx = new TextBox();
            tbDx.Text = "dx";
            tbDy = new TextBox();
            tbDy.Text = "dy";
            tbDz = new TextBox();
            tbDz.Text = "dz";
            this.Container.Add(tbDx);
            this.Container.Add(tbDy);
            this.Container.Add(tbDz);
        }

        /// <summary>
        /// Metoda vrátí matici zadanou touto komponentou. Pokud dojde k chybě, metoda vrátí null.
        /// </summary>
        /// <returns>Matice zadaná komponentou.</returns>
        public Matrix getMatrix()
        {
            return null;
        }
    }
}
