using System;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace IFS_openGL_test
{
    /// <summary>
    /// Třída obstarává zobrazování pomocí OpenGL
    /// </summary>
    public class Zobrazovac
    {
        //ID vytvořeného okna
        private int winHandler;

        //rozměry
        private int width;
        private int height;

        //body které se budou vykreslovat
        Bod[] body;

        public Zobrazovac(int w, int h, Bod[] body)
        {
            this.width = w;
            this.height = h;

            inicGl();
            inicGlut();
            inicEvents();

            setBody(body);

            Glut.glutMainLoop();                        //spuštění hlavní zobrazovací smyčky
        }

        /// <summary>
        /// Metoda zinicializuje renderování
        /// </summary>
        private void inicGl()
        {
            Gl.glEnable(Gl.GL_DEPTH);                   //správné vykreslování viditelnosti/neviditelnosti
            Gl.glEnable(Gl.GL_NORMALIZE);
        }

        /// <summary>
        /// Metoda inicializuje GLUT - inicializuje fram buffer, nastaví rozměry okna, vytvoření okna...
        /// </summary>
        private void inicGlut()
        {
            Glut.glutInit();                                                                //inicializace GLUT
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH | Glut.GLUT_RGB);   //double buffering
            Glut.glutInitWindowSize(width, height);                                         //rozmery okna
            winHandler = Glut.glutCreateWindow("IFS-OpenGL test");                          //vytvornei okna
        }

        /// <summary>
        /// Metoda v GLUTu nastaví reakce na vykreslovací události.
        /// </summary>
        private void inicEvents()
        {
            Glut.glutDisplayFunc(onDisplay);
        }

        /// <summary>
        /// Reakce na událost zorbrazení.
        /// </summary>
        private void onDisplay()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45.0, 1, 1, 500);
            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Gl.glTranslatef(0f, 0f, -5f);

            if(body != null)
            {
                Gl.glBegin(Gl.GL_POINTS);
                for (int i = 0; i < body.Length; i++)
                {
                    Gl.glColor3f(body[i].r, body[i].g, body[i].b);
                    Gl.glVertex3f(body[i].x, body[i].y, body[i].z);
                }
                Gl.glEnd();
            }


            Glut.glutSwapBuffers();
        }

        public void setBody(Bod[] body)
        {
            this.body = body;
        }
    }
}
