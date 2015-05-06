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
        private const int MAX_POCET_SLOUPCU_MATIC = 3;

        //maximální počet matic ve sloupci
        private const int MAX_POCET_MATIC_SLOUPEC = 3;

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

        //maximální počet transformací je 9 => 9 barev, pro každou transformaci 1
        static Color[] barvy = new Color[] {Color.Red, Color.Green, Color.Blue, 
                                     Color.Orange, Color.Yellow, Color.Violet, 
                                     Color.Aqua, Color.GreenYellow, Color.White};

        #region příklady IDS
        #region sierp troj
        static Matrix stm1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0.5f, 0, 0.2, barvy[0]);
        static Matrix stm2 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, -0.5f, 0.2, barvy[1]);
        static Matrix stm3 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, 0.5f, 0.2, barvy[2]);

        static Matrix stm4 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, -0.5f, 0.2, barvy[3]);

        static Matrix stm5 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, 0.5f, 0.2, barvy[4]);
        Matrix[] stMatice = new Matrix[] { stm1, stm2, stm3, stm4, stm5 };

        #endregion

        #region barnsleyho kapradina
        static Matrix bkm1 = new Matrix(new float[,]{
                                            {0,0,0},
                                            {0,0.16f,0},
                                            {0,0,0}
        }, 0, 0, 0, 0.01, barvy[0]);

        static Matrix bkm2 = new Matrix(new float[,]{
                                            {0.85f,0.04f,0},
                                            {-0.04f,0.85f,0},
                                            {0,0,0}
        }, 0, 1.60f, 0, 0.85, barvy[1]);

        static Matrix bkm3 = new Matrix(new float[,]{
                                            {0.20f,-0.26f,0},
                                            {0.23f,0.22f,0},
                                            {0,0,0f}
        }, 0, 01.60f, 0, 0.07, barvy[2]);

        static Matrix bkm4 = new Matrix(new float[,]{
                                            {-0.15f,0.28f,0},
                                            {0.26f,0.24f,0},
                                            {0,0,0f}
        }, 0, 0.44f, 0, 0.07, barvy[3]);
        Matrix[] bkMatice = new Matrix[] { bkm1, bkm2, bkm3, bkm4 };
        #endregion

        #region draci krivka
        static Matrix dkm1 = new Matrix(new float[,]{
                                            {0.5f,-0.5f,0},
                                            {0.5f,0.5f,0},
                                            {0,0,0}
        }, 0, 0, 0, 0.5, barvy[0]);

        static Matrix dkm2 = new Matrix(new float[,]{
                                            {-0.5f,0.5f,0},
                                            {-0.5f,-0.5f,0},
                                            {0,0,0}
        }, 1, 0f, 0, 0.5, barvy[1]);
        Matrix[] dkMatice = new Matrix[] { dkm1, dkm2 };

        #endregion

        #endregion

        public Okno()
        {
            InitializeComponent();
            this.Text = "Zadání IFS";
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
        /// Metoda smaže všechny formuláře na zadávání matic.
        /// </summary>
        private void deleteAllMatrixes()
        {
            //smazání současných matrix formů
            List<MatrixForm> naSmazani = new List<MatrixForm>();
            foreach (MatrixForm mf in matrixForms)
            {
                naSmazani.Add(mf);
            }
            foreach (MatrixForm mf in naSmazani)
            {
                deleteMatrix(mf);
            }
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
            this.Controls.Remove(mf);
            aktualizujMatice();
        }

        /// <summary>
        /// Ze zadaného pole matic vytvoří zadávací formuláře.
        /// </summary>
        /// <param name="mat">Pole matic.</param>
        private void addNewMatrixes(Matrix[] mat)
        {
            foreach (Matrix m in mat)
            {
                addNewMatrixForm(m);
            }
        }

        /// <summary>
        /// Metoda vytvoří a přidá novou komponentu nazadávání matice do formuláře.
        /// </summary>
        private void addNewMatrixForm(Matrix m = null)
        {
            if(sloupec == MAX_POCET_SLOUPCU_MATIC)
            {
                MessageBox.Show("Dosažen maximální počet transformačních matic.");
                return;
            }

            MatrixForm mf = null;

            if(m != null)
            {
                mf = new MatrixForm(this, defText + " " + (matrixForms.Count + 1), m);

            }
            else
            {
                mf = new MatrixForm(this, defText + " " + (matrixForms.Count + 1),barvy[matrixForms.Count]);
            }
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
                MessageBox.Show(ch+chyby, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return res;
        }

        private void bGeneruj_Click(object sender, EventArgs e)
        {
            //nacteni matic
            List<Matrix> matice = nactiMatice();
            if (matice.Count == 0) { return; }

            //kontrola součtu pravděpodobností
            double prd = 0;
            double chyba = 0.01;
            foreach(Matrix m in matice)
            {
                prd += m.probability;
                Console.WriteLine(m.probability.ToString());
            }
            if(Math.Abs(prd - 1) > chyba)
            {
                MessageBox.Show("Součet pravděpodobností transformací není  1.","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            Bod[] fraktal;
            IFS ifs = new IFS(matice);
            fraktal = ifs.fraktalNahodne3D(150000);
            zobrazovac = new Zobrazovac(1024, 648, fraktal);
        }

        #endregion

        /// <summary>
        /// Metoda vymaže stávající zadávací formuláře a nahradí je formuláři s hodnotami podle zadaného pole matic.
        /// </summary>
        /// <param name="matice">Pole matic</param>
        private void setZadavaciFormulare(Matrix[] matice)
        {
            deleteAllMatrixes();

            //přidání nových matrixformů se zadáním sierpinského trojúhelníku
            addNewMatrixes(matice);

            aktualizujMatice();
        }

        private void bSierpTroj_Click(object sender, EventArgs e)
        {
            setZadavaciFormulare(stMatice);
        }

        private void bBarnsKapr_Click(object sender, EventArgs e)
        {
            setZadavaciFormulare(bkMatice);
        }

        private void bDraciKrivka_Click(object sender, EventArgs e)
        {
            setZadavaciFormulare(dkMatice);
        }

    }
}
