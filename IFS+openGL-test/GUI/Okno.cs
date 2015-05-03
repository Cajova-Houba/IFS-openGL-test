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

        //maximální počet sloupců s maticemi
        private const int MAX_POCET_SLOUPCU_MATIC = 2;

        //maximální počet matic ve sloupci
        private const int MAX_POCET_MATIC_SLOUPEC = 5;

        //sloupec do kterého se nové matice umisťují
        private int sloupec = 0;

        //počet matic ve sloupci do kterého se právě umisťuje
        private int pocetMatic = 0;

        //defaultní text MatrixFormu
        private String defText = "Matice";

        //odsazení mezi maticemi
        private int mezeraVelka = 10;
        private int mezeraMala = 5;

        //komponenty na zadávání matic
        private List<MatrixForm> matrixForms;

        public Okno()
        {
            InitializeComponent();
            matrixForms = new List<MatrixForm>();
            addNewMatrixForm();
            addNewMatrixForm();
        }

        #region přidávání/odebírání MatrixFormů

        /// <summary>
        /// Metoda znovurozmístí a přejmenuje matice.
        /// </summary>
        private void aktualizujMatice()
        {
            //matice ve sloupci, sloupec, poradove cislo
            int m = 0, s = 0, i = 1 ;
            foreach(MatrixForm mf in matrixForms)
            {
                mf.Location = new Point(mezeraVelka + s * (MatrixForm.w + mezeraMala), mezeraVelka + m * (MatrixForm.h + mezeraMala));
                mf.Text = defText+" "+i;
                m++;
                i++;
                if(m == MAX_POCET_MATIC_SLOUPEC)
                {
                    m = 0;
                    s++;
                }
            }

            //aktualizování řídíc proměnných
            pocetMatic = m;
            sloupec = s;
        }

        /// <summary>
        /// Metoda vymaže zadanou komponentu ze seznamu. Pak zavolá metody na znovurozmístění.
        /// Tuto metodu bude volat MatrixForm po kliknutí na smazání.
        /// a přejmenování stávajících metod.
        /// </summary>
        /// <param name="mf">Komponenta ke smazání</param>
        public void deleteMatrix(MatrixForm mf)
        {
            if(!matrixForms.Contains(mf))
            {
                //neobsahuje zadanou matici, nic se nestane
                return;
            }

            matrixForms.Remove(mf);
            aktualizujMatice();
        }

        /// <summary>
        /// Metoda vytvoří a přidá novou komponentu nazadávání matice do formuláře.
        /// </summary>
        private void addNewMatrixForm()
        {
            if(sloupec == MAX_POCET_SLOUPCU_MATIC)
            {
                MessageBox.Show("Dosažen maximální počet transformačních matic.");
                return;
            }

            MatrixForm mf = new MatrixForm(this, defText + " " + (matrixForms.Count + 1));
            mf.Location = new Point(mezeraVelka + sloupec*(MatrixForm.w+mezeraMala), mezeraVelka+pocetMatic*(MatrixForm.h+mezeraMala));
            pocetMatic++;
            if(pocetMatic == MAX_POCET_MATIC_SLOUPEC)
            {
                pocetMatic = 0;
                sloupec++;
            }

            matrixForms.Add(mf);
            this.Controls.Add(mf);
        }

        private void bPridejMatici_Click(object sender, EventArgs e)
        {
            addNewMatrixForm();
        }

        #endregion

        #region generování IFS

        /// <summary>
        /// Metoda projde seznam MatrixFormů a z každého načte matici.
        /// </summary>
        /// <returns>Seznam načtených matic.</returns>
        private List<Matrix> nactiMatice()
        {
            List<Matrix> res = new List<Matrix>();
            String ch = "Ignorovány chybně zadané matice: ";
            StringBuilder sbChyby = new StringBuilder();

            foreach(MatrixForm mf in matrixForms)
            {
                Matrix m = mf.getMatrix();
                if(m == null)
                {
                    sbChyby.Append(mf.Text+";");
                }
                else
                {
                    res.Add(m);
                }

            }

            //výpis případných chybných matic
            String chyby = sbChyby.ToString();
            if(chyby.Length != 0)
            {
                MessageBox.Show(ch + chyby);
            }

            return res;
        }

        private void bGeneruj_Click(object sender, EventArgs e)
        {
            //nacteni matic
            List<Matrix> matice = nactiMatice();
            if (matice.Count == 0) { return; }

            Bod[] fraktal;
            IFS ifs = new IFS();
            fraktal = ifs.sierpTrojuhelnikNahodne3D(150000);
            zobrazovac = new Zobrazovac(1024, 648, fraktal);
        }

        #endregion

    }
}
