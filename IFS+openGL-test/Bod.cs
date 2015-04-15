using System;
using System.Drawing;

namespace IFS_openGL_test
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
    }
}
