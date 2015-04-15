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
    class Matrix
    {
        public float[,] matice;
        public float dx, dy, dz;

        public Matrix(float[,] matice, float dx, float dz, float dy)
        {
            this.matice = matice;
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
        }
    }
}
