using System;
using System.Collections.Generic;
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

        public Matrix(float[,] matice, float dx, float dy, float dz, double probability)
        {
            this.matice = matice;
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            this.probability = probability;
        }
    }
}
