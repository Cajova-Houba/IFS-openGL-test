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
        Matrix m1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0.5f, 0);
        Matrix m2 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, -0.5f);
        Matrix m3 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0.5f, -0.5f, 0.5f);

        Matrix m4 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, -0.5f);

        Matrix m5 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, -0.5f, -0.5f, 0.5f);

        private Random r;
        private bool dbg = false;
        private int pocetTransformaci = 5;

        public IFS()
        {
            r = new Random();
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
            }

            return body;
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
            if (k == -1)
            {
                k = r.Next(3);
            }

            Bod novy = bod;

            switch(k)
            {
                case 0:
                    novy = bod.vynasob(m1);
                    break;
                case 1:
                    novy = bod.vynasob(m2);
                    break;
                case 2:
                    novy = bod.vynasob(m3);
                    break;
                case 3:
                    novy = bod.vynasob(m4);
                    break;
                case 4:
                    novy = bod.vynasob(m5);
                    break;
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
            for (int i = 0; i < iterace; i++)
            {
                for (int k = 0; k < body.Length; k++)
                {
                    int p = r.Next(pocetTransformaci);
                    Bod bn = funkce3D(body[k], p);
                    
                    dbgOut(String.Format("[{0},{1},{2}] -> {3} ->[{4},{5},{6}]", body[k].x, body[k].y, body[k].z, p, bn.x, bn.y, bn.z));

                    body[k] = bn;
                    res.Add(bn);
                }
            }

            return res.ToArray();

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

            return obarvi(body, new float[,] { { xMin, xMax }, { yMin, yMax }, { zMin, zMax }});
        }

        /// <summary>
        /// Metoda vygeneruje sierpinského trojúhelník v prostoru náhodným algoritmem. Získané body pak přetransformuje na správný rozměr
        /// a vrátí je.
        /// </summary>
        /// <param name="iterace">Maximální počet iterací.</param>
        /// <returns>Pole bodů představující sierpinského trojúhelník.</returns>
        public Bod[] sierpTrojuhelnikNahodne3D(int iterace)
        {
            Bod[] res = sierpNahodne3D(iterace);
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
