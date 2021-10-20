using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK.Graphics;

namespace gotcadumitru
{
    class MainWindow : GameWindow
    {
        private int varfuriBufferHandler;
        private int shaderProgramHandler;
        private int varfuriArrayHandler;
        // Constructor.
        public MainWindow(string title,int width = 800, int height = 600)
        {
            Title = title;
            Width = width;
            Height = height;
            KeyDown += Keyboard_KeyDown;
        }

        // Tratează evenimentul generat de apăsarea unei taste. Mecanismul standard oferit de .NET
        // este cel utilizat.
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Exit();

            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
        }

        // Setare mediu OpenGL și încarcarea resurselor (dacă e necesar) - de exemplu culoarea de
        // fundal a ferestrei 3D.
        // Atenție! Acest cod se execută înainte de desenarea efectivă a scenei 3D.
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.MidnightBlue);

            float[] varfuri = new float[]
            {
                0.0f,0.10f,0f,
                0.5f,-0.5f,0f,
                -0.5f, -0.5f, 0f,
            };
            this.varfuriBufferHandler = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.varfuriBufferHandler);
            GL.BufferData(BufferTarget.ArrayBuffer, varfuri.Length * sizeof(float), varfuri, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.varfuriArrayHandler = GL.GenVertexArray();
            GL.BindVertexArray(this.varfuriArrayHandler);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.varfuriBufferHandler);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            GL.BindVertexArray(0);


            string varfuriShaderCode =
                @"
                #version 300 core
                layout (location = 0) in vec3 aPosition;
                   
                  void main(){
                        gl_Position = vector4(aPosition, 1f);
                  }
                ";

            string pixelShaderCode =
                 @"
                #version 300 core
                out vec4 pixelColor;
                   
                  void main(){
                        pixelColor = vec4(0.8f,0.8f,0.1f,1f);
                  }
                ";
            int varfShader  = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(varfShader, varfuriShaderCode);
            GL.CompileShader(varfShader);

            int pixelShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShader, pixelShaderCode);
            GL.CompileShader(pixelShader);

            this.shaderProgramHandler = GL.CreateProgram();
            GL.AttachShader(this.shaderProgramHandler, varfShader);
            GL.AttachShader(this.shaderProgramHandler, pixelShader);

            GL.LinkProgram(this.shaderProgramHandler);

            GL.DetachShader(this.shaderProgramHandler, varfShader);
            GL.DetachShader(this.shaderProgramHandler, pixelShader);


            GL.DeleteShader(varfShader);
            GL.DeleteShader(pixelShader);

            



        }

        // Inițierea afișării și setarea viewport-ului grafic. Metoda este invocată la redimensionarea
        // ferestrei. Va fi invocată o dată și imediat după metoda ONLOAD!
        // Viewport-ul va fi dimensionat conform mărimii ferestrei active (cele 2 obiecte pot avea și mărimi 
        // diferite). 
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        // Secțiunea pentru "game logic"/"business logic". Tot ce se execută în această secțiune va fi randat
        // automat pe ecran în pasul următor - control utilizator, actualizarea poziției obiectelor, etc.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Momentan aplicația nu face nimic!
        }

        // Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        // Parametrul de intrare "e" conține informatii de timing pentru randare.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color4.CornflowerBlue);

            GL.UseProgram(this.shaderProgramHandler);
            GL.BindVertexArray(this.varfuriArrayHandler);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
           

            this.SwapBuffers();
        }

    }
}
