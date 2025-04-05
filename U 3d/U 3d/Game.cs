using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using U_3d.Clases;
using System.Collections.Generic;

namespace U_3d
{
    public class Game : GameWindow
    {
        private Shader _shader;
        private Camara _camara;
        private List<Objeto> _objetos;
        private Lineas _lineas;
        private float _totalRotation = 0.0f;
        private bool _paused = false;
        private Matrix4 _projection;
        private float _separacion = 2.5f; // Separación entre U's

        public Game(int width, int height, string title, int cantidadU) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title, APIVersion = new Version(4, 6) })
        {
            CenterWindow();

            _lineas = new Lineas();
            _camara = new Camara();
            _objetos = new List<Objeto>();

            // Crear múltiples objetos U
            for (int i = 0; i < cantidadU; i++)
            {
                Puntos puntos = new Puntos();
                Caras caras = new Caras();
                Objeto objeto = new Objeto(puntos, caras);

                // Desplazar cada U horizontalmente
                objeto.DesplazarHorizontalmente(i * _separacion);

                _objetos.Add(objeto);
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));
            GL.Enable(EnableCap.DepthTest);

            _lineas.ConfigurarLineas();

            _shader = new Shader("shader.vert", "shader.frag");
            _shader.Use();

            // Inicializar todos los objetos
            foreach (var objeto in _objetos)
            {
                objeto.Inicializar(_shader);
            }

            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                Size.X / (float)Size.Y,
                0.1f,
                100.0f
            );
            _shader.SetMatrix4("projection", _projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
                Close();
            if (input.IsKeyPressed(Keys.Space))
                _paused = !_paused;

            // Control de cámara
            _camara.ProcesarMovimiento(input, (float)args.Time);

            // Rotación automática
            if (!_paused)
                _totalRotation += 15.0f * (float)args.Time;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

            // Actualizar matriz de vista
            var view = _camara.ObtenerMatrizVista();
            _shader.SetMatrix4("view", view);

            // Dibujar todas las U's
            foreach (var objeto in _objetos)
            {
                // Rotar cada U
                Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_totalRotation));
                _shader.SetMatrix4("model", model);

                objeto.DibujarObjeto(_lineas);
            }

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
            _shader.SetMatrix4("projection", _projection);
        }

        protected override void OnUnload()
        {
            // Liberar todos los objetos
            foreach (var objeto in _objetos)
            {
                objeto.Liberar();
            }

            GL.UseProgram(0);
            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }
    }
}