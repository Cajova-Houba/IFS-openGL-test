using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IFS_openGL_test.ifs;
using System.Drawing;

namespace IFS_openGL_test.GUI
{
    public partial class MatrixForm : GroupBox
    {
        //zadávací komponenty
        TextBox[,] tbMatice; //matice 3x3
        TextBox tbDx;
        TextBox tbDy;
        TextBox tbDz;
        TextBox tbProbability;

        //color dialog pro výběr barvy transformace
        ColorDialog cdBarva;
        
        //button pro zobrazení vybrané barvy
        //bude mít stejné rozměry jako delete button
        Color barva = Color.Black;  
        Button bVyberBarvy;

        //button pro smazání komponenty
        Button bDelete;

        //rozměry komponenty
        public static int w = 190;
        public static int h = 170;

        //defaultní rozměr textboxu
        int tbW = 35;
        int tbH = 20;
        System.Drawing.Size defTbSize;

        //odsazeni od krajů komponenty
        int mezeraVelka = 10;
        int mezeraMala = 5;

        //rozměr delete buttonu
        int delBtnW = 40;
        int delBtnH = 40;

        //odkaz na rodiče kvůli mazání
        Okno rodic;

        public MatrixForm(Okno rodic, String caption, Color barva)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(w,h);
            this.Text = caption;
            this.rodic = rodic;
            this.barva = barva;
            inic();
        }

        public MatrixForm(Okno rodic, String caption, Matrix matrix)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(w, h);
            this.Text = caption;
            this.rodic = rodic;
            this.barva = matrix.barva;
            inic();

            //nastavení hodnot podle zadané matice
            for (int i = 0; i < matrix.matice.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.matice.GetLength(1); j++)
                {
                    tbMatice[i, j].Text = matrix.matice[i, j].ToString();
                }
            }

            tbDx.Text = matrix.dx.ToString();
            tbDy.Text = matrix.dy.ToString();
            tbDz.Text = matrix.dz.ToString();

            tbProbability.Text = matrix.probability.ToString();

        }

        public MatrixForm(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            inic();
        }

        /// <summary>
        /// Vytvoří defaultní text box a nastaví mu zadaný text.
        /// </summary>
        /// <param name="text">Zobrazený text.</param>
        /// <returns>Vytvořený textBox</returns>
        private TextBox createTextBox(String text)
        {
            TextBox tb = new TextBox();
            tb.Text = text;
            tb.Size = defTbSize;
            //tb.Dock = DockStyle.Left;

            return tb;
        }

        /// <summary>
        /// Metoda inicializuje zadávací komponenty.
        /// </summary>
        private void inic()
        {
            defTbSize = new System.Drawing.Size(tbW, tbH);

            //zadávání matice
            tbMatice = new TextBox[3,3];
            for (int i = 0; i < tbMatice.GetLength(0); i++)
            {
                for (int j = 0; j < tbMatice.GetLength(1); j++)
                {
                    if(i == j)
                    {
                        tbMatice[i, j] = createTextBox("0,5");
                    }
                    else
                    {
                        tbMatice[i, j] = createTextBox("0");
                    }
                    tbMatice[i, j].SetBounds(i * (tbW+mezeraMala) + mezeraVelka, j * (tbH+mezeraMala) + 2*mezeraVelka, tbW, tbH);
                    this.Controls.Add(tbMatice[i, j]);
                }
            }

            //zadávání dx,dy,dz
            tbDx = createTextBox("0,5");
            tbDx.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka, tbW, tbH);
            tbDy = createTextBox("0,5");
            tbDy.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka + tbH + mezeraMala, tbW, tbH);
            tbDz = createTextBox("0,5");
            tbDz.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka + 2*(tbH + mezeraMala), tbW, tbH);
            this.Controls.Add(tbDx);
            this.Controls.Add(tbDy);
            this.Controls.Add(tbDz);

            //zadávání pravděpodobnosti
            Label pl = new Label();
            pl.Text = "Pravděpodobnost:";
            pl.Location = new Point(mezeraVelka,h-mezeraVelka-delBtnH-tbH);
            tbProbability = new TextBox();
            tbProbability.Text = "0,2";
            tbProbability.SetBounds(pl.Location.X + pl.Width, pl.Location.Y-3, 2*tbW, tbH);
            this.Controls.Add(pl);
            this.Controls.Add(tbProbability);

            //mazací tlačítko
            bDelete = new Button();
            bDelete.Text = "X";
            bDelete.SetBounds(w - mezeraVelka - delBtnW, h - mezeraVelka - delBtnH, delBtnW, delBtnH);
            bDelete.Click += bDelete_Click;
            this.Controls.Add(bDelete);

            //button pro výběr barvy
            bVyberBarvy = new Button();
            bVyberBarvy.Text = "";
            bVyberBarvy.SetBounds(mezeraVelka, bDelete.Location.Y, delBtnW, delBtnH);
            bVyberBarvy.BackColor = this.barva;
            bVyberBarvy.Click += bVyberBarvy_Click;
            this.Controls.Add(bVyberBarvy);

        }

        #region načítání dat z komponenty

        /// <summary>
        /// Metoda načte matici 3x3 z pole tbMatice. Pokud dojde k chybě, vrátí null
        /// </summary>
        /// <returns>Matice 3x3 nebo null.</returns>
        private float[,] nactiMatici()
        {
            float[,] res = new float[tbMatice.GetLength(0), tbMatice.GetLength(1)];

            for (int i = 0; i < tbMatice.GetLength(0); i++)
            {
                for (int j = 0; j < tbMatice.GetLength(1); j++)
                {
                    float cislo = 0;
                    try
                    {
                        cislo = (float)Convert.ToDouble(tbMatice[i, j].Text);
                        res[i, j] = cislo;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Metoda načte čísla dx, dy a dz. Pokud dojde k chybě vrátí null.
        /// </summary>
        /// <returns>Pole [dx,dy,dz] nebo null.</returns>
        private float[] nactiDxyz()
        {
            float[] res = new float[3];

            try
            {
                res[0] = (float)Convert.ToDouble(tbDx.Text);
                res[1] = (float)Convert.ToDouble(tbDy.Text);
                res[2] = (float)Convert.ToDouble(tbDz.Text);
            }
            catch (Exception)
            {
                return null;
            }

            return res;
        }

        /// <summary>
        /// Metoda načte pravděpodobnost. Pokud dojde k chybě, vrátí -1.
        /// </summary>
        /// <returns>Pravděpodobnost, nebo -1.</returns>
        private float nactiPravdepodobnost()
        {
            float res = 0;

            try
            {
                res = (float)Convert.ToDouble(tbProbability.Text);
            }
            catch (Exception)
            {
                return -1;
            }

            return res;
        }

        /// <summary>
        /// Metoda vrátí matici zadanou touto komponentou. Pokud dojde k chybě, metoda vrátí null.
        /// </summary>
        /// <returns>Matice zadaná komponentou, nebo null.</returns>
        public Matrix getMatrix()
        {
            float[,] matice = nactiMatici();
            float[] dxyz = nactiDxyz();
            float prvd = nactiPravdepodobnost();

            if(matice == null || dxyz == null || prvd == -1)
            {
                return null;
            }

            return new Matrix(matice, dxyz[0], dxyz[1], dxyz[2], prvd, this.barva);
        }

        #endregion

        /// <summary>
        /// Reakce na událost kliknutí na mazací tlačítko. Přes odkaz na rodiče se zavolá metoda delete a objekt se zničí.
        /// </summary>
        /// <param name="sender">Objekt který na událost reaguje</param>
        /// <param name="e">Argumenty události.</param>
        private void bDelete_Click(object sender, EventArgs e)
        {
            rodic.deleteMatrix(this);
            this.Dispose();
        }

        /// <summary>
        /// Metoda spustí colordialog na výběr barvy.
        /// </summary>
        /// <param name="sender">Objekt, který na událost reaguje.</param>
        /// <param name="e">Argumenty události.</param>
        private void bVyberBarvy_Click(object sender, EventArgs e)
        {
            cdBarva = new ColorDialog();
            cdBarva.Color = this.barva;

            if(cdBarva.ShowDialog() == DialogResult.OK)
            {
                this.barva = cdBarva.Color;
                this.bVyberBarvy.BackColor = this.barva;
            }
        }

        /// <summary>
        /// Metoda vrátí obsah pole Text.
        /// </summary>
        /// <returns></returns>
        public String getName()
        {
            return this.Text;
        }
    }
}
