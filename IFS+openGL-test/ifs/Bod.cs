using System;
using System.Drawing;

namespace IFS_openGL_test.ifs
{
    /// <summary>
    /// Třída představuje bod ve 3d prostoru. Bod je reprezentován float souřadnicemi x,y,z a barvou
    /// složenou z float složek r,g,b.
    /// </summary>
    public class Bod
    {
        //souřadnice
        public float x;
        public float y;
        public float z;

        //barva
        public float r;
        public float g;
        public float b;

        public Bod()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;

            this.r = 0f;
            this.g = 0f;
            this.b = 0f;
        }

        public Bod(float x, float y, float z, Color c)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this.r = c.R / 255f;
            this.g = c.G / 255f;
            this.b = c.B / 255f;
        }

        /// <summary>
        /// Metoda vynásobí bod zadanou maticí a vrátí jej. Pokud bude mít matice špatný rozměr, vrátí nezměněný bod.
        /// </summary>
        /// <param name="m">Matice, kerou bude bod vynásoben</param>
        /// <returns></returns>
        public Bod vynasob(Matrix m)
        {
            //kontrola rozměrů 
            if(m.matice.GetLength(0) != 3 || m.matice.GetLength(1) != 3)
            {
                return this;
            }

            Bod novy = new Bod();
            novy.x = m.matice[0, 0] * this.x + m.matice[0, 1] * this.y + m.matice[0, 2] * this.z + m.dx;
            novy.y = m.matice[1, 0] * this.x + m.matice[1, 1] * this.y + m.matice[1, 2] * this.z + m.dy;
            novy.z = m.matice[2, 0] * this.x + m.matice[2, 1] * this.y + m.matice[2, 2] * this.z + m.dz;

            return novy;
        }

    }
}
