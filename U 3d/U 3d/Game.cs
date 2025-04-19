using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using System;
using ImGuiNET;

namespace U_3d.Clases
{
    public class Game : GameWindow
    {
        private Escenario _escenario;
        private Matrix4 _projection;
        private float _tiempoAcumulado = 0;
        private bool _demostracionActiva = true;

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
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f); 
            GL.Enable(EnableCap.DepthTest);

            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                Size.X / (float)Size.Y,
                0.1f,
                100.0f
            );

            

        }
        private void Utilidades()
        {
            //if (_escenario.Objetos.ContainsKey("LetraU_0"))
            //Clases.Utilidades.Rotar(_escenario, 0, 90, 0);
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

            // Configurar matrices de proyección y vista
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projectionMatrix = _projection;
            GL.LoadMatrix(ref projectionMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 viewMatrix = _escenario.ObtenerVistaMatriz();
            GL.LoadMatrix(ref viewMatrix);

            // Renderizar escenario
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

        // Método público para acceder al escenario (útil para pruebas)
        public Escenario ObtenerEscenario()
        {
            return _escenario;
        }
    }
}