using System;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;
using IFS_openGL_test.ifs;
using System.Diagnostics;

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

        private Stopwatch watch;

        /*úroveň zoomu
         * 1 = žádný zoom
         * >1 = větší zoom
         * <1 = menší zoom
         */
        private float zoomRatio = 1f;

        //změna zoomu při pohybu kolečkem
        private float zoomDelta = 0.25f;

        //body které se budou vykreslovat
        Bod[] body;

        //pro zobrazeni
        //nejmensi vzdalenost pro vykresleni
        private float zNear = 0;

        //nejvetsi vzdalenost pro vykresleni
        private float zFar = 100;

        //úhel, který zabírá kamera
        private float fovy = 60;

        //rotace pomocí myši
        private float angleX = 0f;
        private float deltaAngleX = 0f;
        private float deltaAngleXOld = 0f;

        private float angleY = 0f;
        private float deltaAngleY = 0f;
        private float deltaAngleYOld = 0f;

        private int xOrigin = -1;
        private int yOrigin = -1;

        public Zobrazovac(int w, int h, Bod[] body)
        {
            this.width = w;
            this.height = h;

            inicGl();
            inicGlut();
            inicEvents();

            setBody(body);

            watch = Stopwatch.StartNew();

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
            Glut.glutCloseFunc(onClose);
            Glut.glutSetOption(Glut.GLUT_ACTION_ON_WINDOW_CLOSE, Glut.GLUT_ACTION_GLUTMAINLOOP_RETURNS);
            winHandler = Glut.glutCreateWindow("IFS Visualiser");
        }

        /// <summary>
        /// Metoda v GLUTu nastaví reakce na vykreslovací události.
        /// </summary>
        private void inicEvents()
        {
            Glut.glutIdleFunc(onDisplay);
            Glut.glutDisplayFunc(onDisplay);
            Glut.glutMouseFunc(onMouseButton);
            Glut.glutMotionFunc(onMouseMove);
            Glut.glutMouseWheelFunc(onMouseWheel);
        }

        /// <summary>
        /// Reakce na zavření okna.
        /// </summary>
        private void onClose()
        {
            Glut.glutHideWindow();
        }

        /// <summary>
        /// Reakce na událost zobrazení.
        /// </summary>
        private void onDisplay()
        {
            watch.Stop();
            float deltaTime = watch.ElapsedMilliseconds / 250f;
            watch.Restart();

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(fovy * zoomRatio, width/height, zNear, zFar);
            Gl.glTranslatef(0f, 0f, -5f);
            Gl.glRotatef(angleX, 0f, 1f, 0f);
            Gl.glRotatef(angleY, 1f, 0f, 0f);

            if(body != null)
            {
                //osy
                Gl.glBegin(Gl.GL_LINES);
                    //x
                    Gl.glColor3f(1f, 0f, 0f);
                    Gl.glVertex3f(0f, 0f, 0f);
                    Gl.glColor3f(1f, 0f, 0f);
                    Gl.glVertex3f(1f, 0f, 0f);
                
                    //y
                    Gl.glColor3f(0f, 1f, 0f);
                    Gl.glVertex3f(0f, 0f, 0f);
                    Gl.glColor3f(0f, 1f, 0f);
                    Gl.glVertex3f(0f, 1f, 0f);

                    //z
                    Gl.glColor3f(0f, 0f, 1f);
                    Gl.glVertex3f(0f, 0f, 0f);
                    Gl.glColor3f(0f, 0f, 1f);
                    Gl.glVertex3f(0f, 0f, 1f);
                Gl.glEnd();

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
        
        /// <summary>
        /// Reakce na událost pohnutí kolečkem myši - přiblížení a oddálení.
        /// </summary>
        private void onMouseWheel(int button, int state, int x, int y)
        {
            //zoom ratio násobí úhel, který je x°. Nejméně může být 0 a nejvíce 180/x
            //kolečko nahoru
            if (state > 0)
            {
                zoomRatio = Math.Max(zoomRatio - zoomDelta,0);
            }

            //kolečko dolu
            else
            {
                zoomRatio = Math.Min(zoomRatio + zoomDelta, 180/fovy);
            }

            Console.WriteLine(zoomRatio);
        }

        /// <summary>
        /// Reakce na stisknutí tlačítka myši.
        /// </summary>
        private void onMouseButton(int button, int state, int x, int y)
        {
            //pohyb pouze při levém tlačítku myši
            if (button == Glut.GLUT_LEFT_BUTTON)
            {
                //tlačítko je puštěno
                if (state == Glut.GLUT_UP)
                {
                    angleX += deltaAngleX;
                    angleY += deltaAngleY;
                    xOrigin = -1;
                    yOrigin = -1;
                }
                else
                {
                    //stiskuntí tlačítka myši = počátek pohybu
                    xOrigin = x;
                    yOrigin = y;
                }
            }
        }

        /// <summary>
        /// Reakce na pohyb myší.
        /// </summary>
        private void onMouseMove(int x, int y)
        {
            //je stisknuto tlačítko
            if ((xOrigin >=0) && (yOrigin >=0))
            {
                deltaAngleXOld = deltaAngleX;
                deltaAngleYOld = deltaAngleY;

                deltaAngleX = (x - xOrigin) * 0.02f;
                deltaAngleY = (y - yOrigin) * 0.02f;

                //nutné řešit změnu pohybu
                if(Math.Abs(deltaAngleX) < Math.Abs(deltaAngleXOld))
                {
                    xOrigin = x;
                    angleX -= deltaAngleX;  
                }
                else
                {
                    angleX += deltaAngleX;
                }
                if (Math.Abs(deltaAngleY) < Math.Abs(deltaAngleYOld))
                {
                    yOrigin = y;
                    angleY -= deltaAngleY;
                }
                else
                {
                    angleY += deltaAngleY;
                }


                //Console.WriteLine(String.Format("deltaAngleX:{0:5} deltaAngleY:{1}", deltaAngleX, deltaAngleY));
                //Console.WriteLine(String.Format("angleX:{0:5} angleY:{1}", angleX, angleY));
            }
        }

        public void setBody(Bod[] body)
        {
            this.body = body;
        }
    }
}
