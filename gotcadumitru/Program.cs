using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

namespace gotcadumitru
{
    class SimpleWindow
    {


        [STAThread]
        static void Main(string[] args)
        {

            MainWindow m_window = new MainWindow("Main window");
            m_window.VSync = OpenTK.VSyncMode.Adaptive;
            m_window.Run();
        }
    }
}
