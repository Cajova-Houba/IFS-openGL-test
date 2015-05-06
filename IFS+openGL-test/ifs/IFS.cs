using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_openGL_test.ifs
{
    public class IFS
    {
        //transformace
        Matrix stm1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0.5f, 0, 0.2, barvy[0]);
        Matrix stm2 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, -0.5f, 0.2, barvy[1]);
        Matrix stm3 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, 0.5f, 0.2, barvy[2]);

        Matrix stm4 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, -0.5f, 0.2, barvy[3]);

        Matrix stm5 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, 0.5f, 0.2, barvy[4]);

        //maximální počet transformací je 9 => 9 barev, pro každou transformaci 1
        static Color[] barvy = new Color[] {Color.Red, Color.Green, Color.Blue, 
                                     Color.Orange, Color.Yellow, Color.Violet, 
                                     Color.Aqua, Color.GreenYellow, Color.White};

        //seznam používaných transformací
        private List<Matrix> transformace;
        private Random r;
        private bool dbg = false;

        //index vybrane transforamce
        private int indexTransforamce;

        private int[] statistika;

        public IFS(List<Matrix> matice=null)
        {
            r = new Random();
            transformace = new List<Matrix>();
            if(matice == null)
            {
                transformace.Add(stm1);
                transformace.Add(stm2);
                transformace.Add(stm3);
                transformace.Add(stm4);
                transformace.Add(stm5);
            }
            else
            {
                transformace = matice;
            }
            inicStatistika();
        }

        private void inicStatistika()
        {
            statistika = new int[transformace.Count];
            for (int i = 0; i < statistika.Length; i++)
            {
                statistika[i] = 0;
            }
        }

        /// <summary>
        /// Metoda obarví zadané pole bodů podle jejich souřadnic a zadaných rozsahů (minimum na ose = 0, maximum 255).
        /// </summary>
        /// <param name="body">Pole bodů k obarvení.</param>
        /// <param name="rozsahy">Rozsahy ve tvaru [[x_min,x_max] , [y_min,y_max] , [z_min,z_max]].</param>
        /// <returns>Obarvené body.</returns>
        private Bod[] obarvi(Bod[] body, float[,] rozsahy)
        {
            //souřadnice každého bodu se transformuje na rozsahy [0..1,0..1,0..1]
            //a takto transformovaná souřadnice se nastaví jako barva x=r,y=g,z=b
            for (int i = 0; i < body.Length; i++)
            {
                float rOld = body[i].x, gOld = body[i].y, bOld = body[i].z;
                body[i].r = (rOld - rozsahy[0, 0]) / (rozsahy[0, 1] - rozsahy[0, 0]);
                body[i].g = (gOld - rozsahy[1, 0]) / (rozsahy[1, 1] - rozsahy[1, 0]);
                body[i].b = (bOld - rozsahy[2, 0]) / (rozsahy[2, 1] - rozsahy[2, 0]);
                //body[i].r = 1f;
                //body[i].g = 0;
                //body[i].b = 0;
            }

            return body;
        }

        /// <summary>
        /// Metoda vybere jednu ze seznamu transformací podle jejich pravděpodobností.
        /// Musí platit, že součet pravděpodobností transformací je 1.
        /// </summary>
        /// <returns>Transformace</returns>
        private Matrix vyberTransformaci()
        {
            //kontrola vstupu
            if (transformace == null) { return null; }
            if (transformace.Count == 0) { return null; }

            double tmp = 0;
            indexTransforamce = -1;
            double rnd = r.NextDouble();
            foreach(Matrix m in transformace)
            {
                if (m.probability > 0)
                {
                    tmp += m.probability;
                    indexTransforamce++;
                    if (tmp > rnd) 
                    { 
                        statistika[indexTransforamce]++; 
                        return m; 
                    }
                }
            }

            return transformace.Last();
        }

        /// <summary>
        /// Metoda náhodně vybere jednu z transformací a aplikuje ji na zadaný bod který vrátí. Číslo transformace je možné zadat
        /// jako parametr.
        /// </summary>
        /// <param name="bod">Bod k transformaci.</param>
        /// <param name="k">Nepovinný parametr - číslo transformace.</param>
        /// <returns>Transformovaný bod.</returns>
        private Bod funkce3D(Bod bod, int k=-1)
        {
            Bod novy = bod;
            if (k == -1)
            {
                novy = bod.vynasob(vyberTransformaci());
            }
            else
            {
                if( k < transformace.Count )
                {
                    novy = bod.vynasob(transformace.ElementAt(k));
                }
            }

            return novy;
        }

        /// <summary>
        /// Metoda vygeneruje sierpinského trojúhelník v prostoru náhodným algoritmem.
        /// </summary>
        /// <param name="iterace">Maximální počet iterací.</param>
        /// <returns>Pole bodů, které představují sierpinského trojúhelník v prostoru.</returns>
        private Bod[] sierpNahodne3D(int iterace)
        {
            List<Bod> res = new List<Bod>();

            //body na pocatku
            Bod[] body = new Bod[] { new Bod(0f, 0f, 0f, Color.Red) };
            //float[][] body = new float[][] { new float[]{0f,0f},
            //                                 new float[]{0.3f,0},
            //                                 new float[]{0f,0.3f}
            //};

            //počítání jednotlivých iterací
            //obarveni do zelena podle stáří
            //na začátku každý bod bílý
            //při každé iteraci snížit barevnou složku kromě zelené o i/iterace
            for (int i = 0; i < iterace; i++)
            {
                for (int k = 0; k < body.Length; k++)
                {
                    Bod bn = funkce3D(body[k]);
                    
                    body[k] = bn;
                    //bn.setColor(getColorStari(i,iterace));
                    res.Add(bn);
                }
            }

            return res.ToArray();
        }

        private Color getColorStari(int i, int maxIterace)
        {
            //do zelena
            double r = 255 - (i / (double)maxIterace) * 255;
            double g = 255;
            double b = 255 - (i / (double)maxIterace) * 255;

            return Color.FromArgb((int)r, (int)g, (int)b);
        }

        /// <summary>
        /// Metoda nalezne maxima a minima jednotlivých os u zadaného pole bodů. Tyto body transformuje
        /// na na jiné rozsahy - kvůli zobrazení v openGL okně, zavola metodu pro obarvení a vrátí je.
        /// </summary>
        /// <param name="body">Body k transformaci.</param>
        /// <returns>Přetransformované pole bodů.</returns>
        private Bod[] transformujBody3D(Bod[] body)
        {
            if (body.Length == 0) { return body; }
            float xMin = body[0].x, xMax = body[0].x,
                  yMin = body[0].y, yMax = body[0].y,
                  zMin = body[0].z, zMax = body[0].z;

            //nalezeni minima, maxima
            for (int i = 0; i < body.Length; i++)
            {
                xMin = Math.Min(body[i].x, xMin);
                xMax = Math.Max(body[i].x, xMax);

                yMin = Math.Min(body[i].y, yMin);
                yMax = Math.Max(body[i].y, yMax);

                zMin = Math.Min(body[i].z, zMin);
                zMax = Math.Max(body[i].z, zMax);
            }

            float[] rX = new float[] { -1f, 1f };
            float[] rY = new float[] { -1f, 1f };
            float[] rZ = new float[] { -1f, 1f };
            //transformace na [rX[0]..rX[1], rY[0]..rY[1], rZ[0]..rZ[1]]
            for (int i = 0; i < body.Length; i++)
            {
                float xn, yn, zn;
                if (xMax == xMin) { xn = body[i].x; }
                else
                {
                    xn = (rX[1] - rX[0]) * (body[i].x - xMin) / (xMax - xMin) + rX[0];
                }
                
                if (yMax == yMin) { yn = body[i].y; }
                else
                {
                    yn = (rY[1] - rY[0]) * (body[i].y - yMin) / (yMax - yMin) + rY[0];
                }
                
                if (zMax == zMin) { zn = body[i].z; }
                else
                {
                    zn = (rZ[1] - rZ[0]) * (body[i].z - zMin) / (zMax - zMin) + rZ[0];
                }

                dbgOut(String.Format("[{0},{1},{2}]", xn, yn, zn));
                body[i] = new Bod(xn, yn, zn, body[i].getColor());
            }

            return body;//obarvi(body, new float[,] { { xMin, xMax }, { yMin, yMax }, { zMin, zMax }});
        }

        /// <summary>
        /// Metoda vygeneruje sierpinského trojúhelník v prostoru náhodným algoritmem. Získané body pak přetransformuje na správný rozměr
        /// a vrátí je.
        /// </summary>
        /// <param name="iterace">Maximální počet iterací.</param>
        /// <returns>Pole bodů představující sierpinského trojúhelník.</returns>
        public Bod[] fraktalNahodne3D(int iterace)
        {
            inicStatistika();
            Bod[] res = sierpNahodne3D(iterace);
            for (int i = 0; i < transformace.Count; i++)
            {
                Console.WriteLine(String.Format("{0}: {1}",i,statistika[i]));
            }
            return transformujBody3D(res);
        }

        private void dbgOut(String msg)
        {
            if (dbg)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
