using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d.Clases
{
    public class Game : GameWindow
    {
        private Escenario _escenario;
        private Matrix4 _projection;

        public Game(int width, int height, string title, int cantidadObjetos) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = (width, height),
                Title = title,
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Compatability
            })
        {
            CenterWindow();
            _escenario = new Escenario();
          
            _escenario.Inicializar(cantidadObjetos, new Color4(1.0f, 1.0f, 1.0f, 1.0f));
            //_escenario.Guardar("C:\\Users\\Anjel\\Semestre 1 - 2025\\Prog. Grafica\\Semestre\\U 3d\\escenario.json");
            //_escenario.Cargar("C:\\Users\\Anjel\\Semestre 1 - 2025\\Prog. Grafica\\Semestre\\U 3d\\escenario.json");
            new Interfaz { Escenario = _escenario }.Start();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),  // FOV
                Size.X / (float)Size.Y,              // ancho / alto
                0.1f,                                // dist. minima visible
                100.0f                               // dist. maxima visible
            );


        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
                Close();

            if ((input.IsKeyDown(Keys.LeftControl)) && (input.IsKeyDown(Keys.S)))
                _escenario.Guardar("C:\\Users\\Anjel\\Semestre 1 - 2025\\Prog. Grafica\\Semestre\\U 3d\\escenario.json");

            _escenario.Actualizar(input, (float)args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // config matrices de proyección 
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projectionMatrix = _projection;
            GL.LoadMatrix(ref projectionMatrix);

            // config de vista
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 viewMatrix = _escenario.ObtenerVistaMatriz();
            GL.LoadMatrix(ref viewMatrix);

            _escenario.Renderizar();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                e.Width / (float)e.Height,
                0.1f,
                100.0f
            );
        }
        public Escenario ObtenerEscenario()
        {
            return _escenario;
        }
    }
}