using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using U_3d.Clases;

namespace U_3d
{
    public class Game : GameWindow
    {
        private Escenario _escenario;

        public Game(int width, int height, string title, int cantidadU) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = (width, height),
                Title = title,
                APIVersion = new Version(4, 1)
            })
        {
            CenterWindow();
            _escenario = new Escenario(cantidadU);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            _escenario.Inicializar(Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
                Close();

            _escenario.Actualizar(input, (float)args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _escenario.Renderizar();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _escenario.ActualizarProyeccion(e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            _escenario.Liberar();
            base.OnUnload();
        }
    }
}