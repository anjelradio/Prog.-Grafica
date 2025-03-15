using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d
{
    public class Game : GameWindow
    {
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;
        private Matrix4 _view;
        private Matrix4 _projection;
        private Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, 5.0f);
        private float _cameraYaw = -90.0f;
        private float _cameraPitch = 0.0f;
        private Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 _cameraUp = Vector3.UnitY;
        private float _cameraSpeed = 1.5f;
        private float _rotationSpeed = 45.0f;
        private float _totalRotation = 0.0f;


        private readonly float[] _vertices = {
            // Posiciones XYZ           // Colores RGB
            // Pilar izquierdo - frente y atrás
            -1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 0
            -1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 1
            -0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 2
            -0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 3
             0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 4
             0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 5
             1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 6
             1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 7
            
            // Pilar derecho - frente y atrás
             -1.0f, -1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 8
             -1.0f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 9
             -0.6f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 10
             -0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 11
             0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 12
             0.6f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 13
             1.0f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 14
             1.0f, -1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 15
    
        };

        // Aristas para wireframe - Pares de índices que definen cada línea
        private readonly uint[] _edges = {
            // Parte Frontal de la U
            0, 1, 1, 2, 2, 3,
            0, 1, 1, 2, 2, 3,
            3, 4, 4,5, 5,6,
            6,7, 7, 0,

            // Parte Trasera de la U
            8,9, 9,10, 10,11,
            11,12, 12,13, 13,14,
            14,15, 15, 8,

            // Conexiones Trasera y frontal

            // Izquierda Externa e Interna
            1, 9, 0, 8, 2, 10, 3 ,11,
            
            // Derecha Externa e Interna
            6, 14, 7, 15, 5, 13, 4,12
        };

        public Game(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title, APIVersion = new Version(4, 6) })
        {
            this.CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));
            GL.Enable(EnableCap.DepthTest);

            // Para ver las líneas mejor
            GL.Enable(EnableCap.LineSmooth);
            GL.LineWidth(2.0f);

            // Crear los objetos buffer y asignar datos
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _edges.Length * sizeof(uint), _edges, BufferUsageHint.StaticDraw);

            // Crear y compilar shaders
            _shader = new Shader("shader.vert", "shader.frag");
            _shader.Use();

            // Definir el layout de los vértices
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var colorLocation = _shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            // Configuración de matrices para proyección 3D
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            _shader.SetMatrix4("projection", _projection);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }
        private bool Paused = false;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused)
                return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
                Close();
            if (input.IsKeyPressed(Keys.Space))
            {
                Paused = !Paused;
            }
            // Control de cámara
            float cameraSpeed = _cameraSpeed * (float)args.Time;

            if (input.IsKeyDown(Keys.W))
                _cameraPosition += cameraSpeed * _cameraFront;
            if (input.IsKeyDown(Keys.S))
                _cameraPosition -= cameraSpeed * _cameraFront;
            if (input.IsKeyDown(Keys.A))
                _cameraPosition -= Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * cameraSpeed;
            if (input.IsKeyDown(Keys.D))
                _cameraPosition += Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * cameraSpeed;

            if (input.IsKeyDown(Keys.Up))
                _cameraPitch += _rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Down))
                _cameraPitch -= _rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Left))
                _cameraYaw -= _rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Right))
                _cameraYaw += _rotationSpeed * (float)args.Time;

            // Limitar pitch para evitar el giro completo
            _cameraPitch = Math.Clamp(_cameraPitch, -89.0f, 89.0f);

            // Actualizar dirección de cámara
            Vector3 direction;
            direction.X = (float)Math.Cos(MathHelper.DegreesToRadians(_cameraYaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_cameraPitch));
            direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_cameraPitch));
            direction.Z = (float)Math.Sin(MathHelper.DegreesToRadians(_cameraYaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_cameraPitch));
            _cameraFront = Vector3.Normalize(direction);

            // Rotación automática
            if (!Paused)
            {
                _totalRotation += 15.0f * (float)args.Time;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

            // Actualizar matriz de vista desde la posición de la cámara
            _view = Matrix4.LookAt(_cameraPosition, _cameraPosition + _cameraFront, _cameraUp);
            _shader.SetMatrix4("view", _view);

            // Dibujar la U
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_totalRotation));
            _shader.SetMatrix4("model", model);

            GL.BindVertexArray(_vertexArrayObject);
            // Usar GL.Lines en lugar de GL.Triangles para dibujar líneas
            GL.DrawElements(PrimitiveType.Lines, _edges.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            // Recalcular matriz de proyección al cambiar el tamaño de la ventana
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            _shader.SetMatrix4("projection", _projection);
        }
    }
}