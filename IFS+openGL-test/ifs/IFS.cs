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
        private bool[,] T, S;

        //transformace
        Matrix m1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0, 0);
        Matrix m1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0, 0);
        Matrix m1 = new Matrix(new float[,]{
                                            {0.5f,0,0},
                                            {0,0.5f,0},
                                            {0,0,0.5f}
        }, 0, 0, 0);

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

        /// <summary>
        /// Metoda náhodně vybere jednu ze třech tranformací a aplikuje ji na zadaný bod.
        /// </summary>
        /// <param name="bod">Bod, který bude transformován.</param>
        /// <param name="k">Nepovinné, transformace, tkerá bdue vybrána.</param>
        /// <returns>Transformovaný bod.</returns>
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

            //počítání jednotlivých iterací
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
