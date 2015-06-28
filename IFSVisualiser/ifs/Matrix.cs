using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_openGL_test.ifs
{
    /// <summary>
    /// Třída představuje matici, kterou lze provádět transformace bodů.
    /// </summary>
    public class Matrix
    {
        public float[,] matice;
        public float dx, dy, dz;
        public double probability;

        //barva kterou budou obraveny body na které byla tato transformace aplikována
        public Color barva;

        public Matrix(float[,] matice, float dx, float dy, float dz, double probability, Color barva)
        {
            this.matice = matice;
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            this.probability = probability;
            this.barva = barva;
        }

        /// <summary>
        /// Metoda vrátí řetězec reprezentující matici ve tvaru, který se dá uložit do souboru.
        /// </summary>
        public String toString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < matice.GetLength(0); i++)
            {
                for (int j = 0; j < matice.GetLength(1); j++)
                {
                    sb.Append(matice[i, j].ToString("0.0000")+";");
                }
            }

            sb.Append(dx.ToString("0.0000") + ";");
            sb.Append(dy.ToString("0.0000") + ";");
            sb.Append(dz.ToString("0.0000") + ";");

            sb.Append(probability.ToString("0.0000") + ";");
            String r = barva.R.ToString("X2");
            String g = barva.G.ToString("X2");
            String b = barva.B.ToString("X2");
            sb.Append(r+g+b);

            return sb.ToString();
        }
    }
}
