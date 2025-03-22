using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

namespace U_3d
{
    public class Game : GameWindow
    {
        private int vertexBufferHandle;
        private int elementBufferHandle;
        private int vertexArrayHandle;
        private Shader shader;
        private Matrix4 view;
        private Matrix4 projection;
        private Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, 5.0f);
        private float _cameraYaw = -90.0f;
        private float _cameraPitch = 0.0f;
        private Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 _cameraUp = Vector3.UnitY;
        private float _cameraSpeed = 1.5f;
        private float rotationSpeed = 45.0f;
        private float totalRotation = 0.0f;

        // para setear las coordenas
        private float setX = 0.0f;
        private float setY = 0.0f;
        private float setZ = 0.0f;
        private float speedSet = 0.00020f; 

        private readonly float[] originalVertices = {
            // Posiciones XYZ           // Colores RGB 
            // Cara frontal de la U
            -1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 0
            -1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 1
            -0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 2
            -0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 3
             0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 4
             0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 5
             1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 6
             1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 7
            
            // Cara trasera de la U
             -1.0f, -1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 8
             -1.0f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 9
             -0.6f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 10
             -0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 11
             0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 12
             0.6f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 13
             1.0f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 14
             1.0f, -1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 15
        };

        // Current vertices with applied offsets
        private float[] _vertices;

        // Pares de índices que definen cada línea
        private readonly uint[] edges = {
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

            _vertices = new float[originalVertices.Length];
            Array.Copy(originalVertices, _vertices, originalVertices.Length);
        }


        // ACTUALIZAR LOS VERTICES DE LA U
        private void UpdateVertices()
        {
            for (int i = 0; i < originalVertices.Length; i += 6)
            {
                _vertices[i] = originalVertices[i] + setX;
                _vertices[i + 1] = originalVertices[i + 1] + setY;
                _vertices[i + 2] = originalVertices[i + 2] + setZ;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
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
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);

            vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);

            elementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, edges.Length * sizeof(uint), edges, BufferUsageHint.StaticDraw);

            // Crear y compilar shaders
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            // Definir el layout de los vértices
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            // Configuración de matrices para proyección 3D
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            shader.SetMatrix4("projection", projection);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(vertexBufferHandle);
            GL.DeleteBuffer(elementBufferHandle);
            GL.DeleteVertexArray(vertexArrayHandle);
            GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }
        private bool Paused = false;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused)
                return;

            var input = KeyboardState;
            bool uUpdated = false;

            if (input.IsKeyDown(Keys.Escape))
                Close();
            if (input.IsKeyPressed(Keys.Space))
            {
                Paused = !Paused;
            }

            // Mover los vertices de la U 
            if (input.IsKeyDown(Keys.U)) 
            {
                setY += speedSet;
                uUpdated = true;
            }
            if (input.IsKeyDown(Keys.J)) 
            {
                setY -= speedSet;
                uUpdated = true;
            }
            if (input.IsKeyDown(Keys.H)) 
            {
                setX -= speedSet;
                uUpdated = true;
            }
            if (input.IsKeyDown(Keys.K)) 
            {
                setX += speedSet;
                uUpdated = true;
            }

            if (input.IsKeyDown(Keys.N))
            {
                setZ -= speedSet;
                uUpdated = true;
            }
            if (input.IsKeyDown(Keys.M))
            {
                setZ += speedSet;
                uUpdated = true;
            }

            // si los vertices fueron alterados se actulizan
            if (uUpdated)
            {
                UpdateVertices();
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
                _cameraPitch += rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Down))
                _cameraPitch -= rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Left))
                _cameraYaw -= rotationSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Right))
                _cameraYaw += rotationSpeed * (float)args.Time;

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
                totalRotation += 15.0f * (float)args.Time;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            // Actualizar matriz de vista desde la posición de la cámara
            view = Matrix4.LookAt(_cameraPosition, _cameraPosition + _cameraFront, _cameraUp);
            shader.SetMatrix4("view", view);

            // Dibujar la U
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(totalRotation));
            shader.SetMatrix4("model", model);

            GL.BindVertexArray(vertexArrayHandle);
            // Usar GL.Lines en lugar de GL.Triangles para dibujar líneas
            GL.DrawElements(PrimitiveType.Lines, edges.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            // Recalcular matriz de proyección al cambiar el tamaño de la ventana
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            shader.SetMatrix4("projection", projection);
        }
    }
}