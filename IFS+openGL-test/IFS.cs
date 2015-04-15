using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_openGL_test
{
    public class IFS
    {
        private bool[,] T, S;

        //transformace
        float[] a = new float[] { 0.5f, 0.5f, 0.5f };
        float[] b = new float[] { 0, 0, 0, 0 };
        float[] c = new float[] { 0, 0, 0, 0 };
        float[] d = new float[] { 0.5f, 0.5f, 0.5f };
        float[] e = new float[] { 0f, 0.5f, 0.5f };
        float[] f = new float[] { 0f, 0f, 0.5f };

        private Random r;
        private bool dbg = false;

        public IFS()
        {
            r = new Random();
        }

        private void inicT(int w, int h)
        {
            T = new bool[w, h];
            Random r = new Random();

            //inicializace pole T
            for (int i = 0; i < T.GetLength(0); i++)
            {
                for (int j = 0; j < T.GetLength(1); j++)
                {
                    if (r.Next(60) == 3)
                    {
                        T[i, j] = true;
                    }
                    ////1. a posledni radek
                    //if (i == 0 || i == T.GetLength(0) - 1)
                    //{
                    //    T[i, j] = true;
                    //}

                    ////1. a poslendi sloupec
                    //if (j == 0 || j == T.GetLength(1) - 1)
                    //{
                    //    T[i, j] = true;
                    //}
                }
            }
        }

        /// <summary>
        /// Metoda převede dvojrozměrné pole na jednorozměrné pole bodů.
        /// </summary>
        /// <param name="pole">Dvojrozměrné pole bool.</param>
        /// <returns>Výsledné pole bodů.</returns>
        private Bod[] booltoBod(bool[,] pole)
        {
            if (pole == null)
            {
                return null;
            }

            int w = pole.GetLength(0);
            int h = pole.GetLength(1);
            float minX = -2f;
            float maxX = 2f;
            float minY = -2f;
            float maxY = 2f;

            List<Bod> res = new List<Bod>();

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if(pole[i,j])
                    {
                        //transformace z [0..99,0..99] na [-2..2,-2..2]
                        float x = (maxX - minX) * i / (float)w + minX;
                        float y = (maxY - minY) * j / (float)h + minY;

                        res.Add(new Bod(x,y, 0f, Color.Red));
                    }
                }
            }

            return res.ToArray();
        }

        /// <summary>
        /// Deterministickým algoritmem vypočítá zadaný počet iterací.
        /// </summary>
        /// <param name="iterace">Maximální počet iterací.</param>
        /// <param name="w">Šířka pole.</param>
        /// <param name="h">Výška pole</param>
        /// <returns>Výsledný rastr boolean. Bod platí, pokud je true.</returns>
        private bool[,] pocitejSierpTroj(int iterace, int w, int h)
        {
            inicT(w, h);
            S = new bool[w, h];

            for (int l = 0; l < iterace; l++)
            {
                //vypocet
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        if (T[i, j])
                        {
                            //vypocet indexu do pole S a nastaveni 1 v S
                            int[] bod;
                            for (int k = 0; k < 3; k++)
                            {
                                bod = new int[] { (int)(a[k] * i + b[k] * j + e[k]), (int)(c[k] * i + d[k] * j + f[k]) };
                                if (bod[0] >= 0 && bod[0] < w && bod[1] >= 0 && bod[1] < h)
                                {
                                    S[bod[0], bod[1]] = true;
                                }
                            }
                        }
                    }
                }
                T = S;
                S = new bool[w, h];
            }

            return T;
        }

        public Bod[] sierpTrojuhelnik(int iterace, int w, int h)
        {
            bool[,] pole = pocitejSierpTroj(iterace, w, h);
            Bod[] res = booltoBod(pole);
            return transformujBody(res);
        }

        private float[] funkce(float[] bod, int k=-1)
        {
            if (k == -1)
            {
                k = r.Next(3);

            }
            float xn = (a[k] * bod[0] + b[k] * bod[1] + e[k]);
            float yn = (c[k] * bod[0] + d[k] * bod[1] + f[k]);
            return new float[] { xn, yn };
        }

        private Bod[] sierpNahodne(int iterace)
        {
            List<Bod> res = new List<Bod>();

            //body na pocatku
            float[][] body = new float[][] { new float[]{0f,0f},
                                             new float[]{0.3f,0},
                                             new float[]{0f,0.3f}
            };

            for (int i = 0; i < iterace; i++)
            {
                for (int k = 0; k < body.Length; k++)
                {
                    int p = r.Next(3);
                    float[] pn = funkce(body[k],p);

                    dbgOut(String.Format("[{2},{3}] -> {4} ->[{0},{1}]", pn[0], pn[0], body[k][0], body[k][1], p));
                    body[k] = new float[] {pn[0], pn[1]};

                    res.Add(new Bod(pn[0], pn[1], 0f, Color.Red));
                }
            }

            return res.ToArray();

        }

        private Bod[] transformujBody(Bod[] puvodni)
        {
            float xMin = puvodni[0].x, xMax = puvodni[0].x, yMin = puvodni[0].y, yMax = puvodni[0].y;

            //nalezeni maxima/minima
            for (int i = 1; i < puvodni.Length; i++)
            {
                xMax = Math.Max(puvodni[i].x, xMax);
                xMin = Math.Min(puvodni[i].x, xMin);

                yMax = Math.Max(puvodni[i].y, yMax);
                yMin = Math.Min(puvodni[i].y, yMin);
            }

            //transofrmace na interval [-2..2, -2..2]
            for (int i = 0; i < puvodni.Length; i++)
            {
                float xn, yn;
                if (xMax == xMin)
                {
                    xn = xMax;
                }
                else
                {
                    xn = (4) * (puvodni[i].x - xMin) / (xMax - xMin) - 2;
                }

                if (yMax == yMin)
                {
                    yn = yMax;
                }
                else
                {
                    yn = (4) * (puvodni[i].y - yMin) / (yMax - yMin) - 2;
                }
                puvodni[i].x = xn;
                puvodni[i].y = yn;
                dbgOut(String.Format("[{0},{1}]", xn, yn));
            }

            return puvodni;
        }

        public Bod[] sierpTrojuhelnikNahodne(int iterace)
        {
            Bod[] res = sierpNahodne(Math.Max(iterace,4));
            return transformujBody(res);
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
