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

        float[] a = new float[] { 0.5f, 0.5f, 0.5f };
        float[] b = new float[] { 0, 0, 0, 0 };
        float[] c = new float[] { 0, 0, 0, 0 };
        float[] d = new float[] { 0.5f, 0.5f, 0.5f };
        float[] e = new float[] { 0f, 0.5f, 0.5f };
        float[] f = new float[] { 0f, 0f, 0.5f };

        private Random r;
        private bool dbg = false;
        private int pocetTransformaci = 5;

        public IFS()
        {
            r = new Random();
        }

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

            return body;
        }

        public Bod[] sierpTrojuhelnikNahodne3D(int iterace)
        {
            Bod[] res = sierpNahodne3D(iterace);
            return transformujBody3D(res);
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
                                             /*new float[]{0.3f,0},
                                             new float[]{0f,0.3f}*/
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
