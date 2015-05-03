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

        public MatrixForm(Okno rodic, String caption)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(w,h);
            this.Text = caption;
            this.rodic = rodic;
            inic();
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
                    tbMatice[i, j] = createTextBox("0");
                    tbMatice[i, j].SetBounds(i * (tbW+mezeraMala) + mezeraVelka, j * (tbH+mezeraMala) + 2*mezeraVelka, tbW, tbH);
                    this.Controls.Add(tbMatice[i, j]);
                }
            }

            //zadávání dx,dy,dz
            tbDx = createTextBox("dx");
            tbDx.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka, tbW, tbH);
            tbDy = createTextBox("dy");
            tbDy.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka + tbH + mezeraMala, tbW, tbH);
            tbDz = createTextBox("dz");
            tbDz.SetBounds(mezeraVelka + 3 * (tbW + mezeraMala) + mezeraVelka, 2*mezeraVelka + 2*(tbH + mezeraMala), tbW, tbH);
            this.Controls.Add(tbDx);
            this.Controls.Add(tbDy);
            this.Controls.Add(tbDz);

            //zadávání pravděpodobnosti
            Label pl = new Label();
            pl.Text = "Pravděpodobnost:";
            pl.Location = new Point(mezeraVelka,h-mezeraVelka-delBtnH-tbH);
            tbProbability = new TextBox();
            tbProbability.Text = "1";
            tbProbability.SetBounds(pl.Location.X + pl.Width, pl.Location.Y-3, 2*tbW, tbH);
            this.Controls.Add(pl);
            this.Controls.Add(tbProbability);

            //mazací tlačítko
            bDelete = new Button();
            bDelete.Text = "X";
            bDelete.SetBounds(w - mezeraVelka - delBtnW, h - mezeraVelka - delBtnH, delBtnW, delBtnH);
            bDelete.Click += bDelete_Click;
            this.Controls.Add(bDelete);
        }

        /// <summary>
        /// Metoda vrátí matici zadanou touto komponentou. Pokud dojde k chybě, metoda vrátí null.
        /// </summary>
        /// <returns>Matice zadaná komponentou.</returns>
        public Matrix getMatrix()
        {

            return null;
        }

        /// <summary>
        /// Reakce na událost kliknutí na mazací tlačítko. Přes odkaz na rodiče se zavolá metoda delete a objekt se zničí.
        /// </summary>
        /// <param name="sender">Objekt který na událost reaguje</param>
        /// <param name="e">Argumenty událost.</param>
        private void bDelete_Click(object sender, EventArgs e)
        {
            rodic.deleteMatrix(this);
            this.Dispose();
        }
    }
}
