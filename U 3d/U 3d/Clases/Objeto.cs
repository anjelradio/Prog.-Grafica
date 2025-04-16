using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public class Objeto
    {
        private int _vertexArrayHandle;
        private int _vertexBufferHandle;
        private int _elementBufferHandle;
        private float[] _vertexData;
        private uint[] _indices;
        private Figura _figura;

        private Vector3 _posicion;
        private Vector3 _rotacion;
        private Vector3 _escala;

        public Objeto(Puntos origen, float ancho, float alto, float profundidad, Vector3 color)
        {
            _posicion = origen.ToVector3();
            _rotacion = Vector3.Zero;
            _escala = Vector3.One;

            //crear la figura en el origen
            _figura = new Figura(origen, ancho, alto, profundidad, color);
            (_vertexData, _indices) = _figura.ObtenerDatosRenderizado();
        }

        public void Inicializar(Shader shader)
        {
            // generar y configurar VAO
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);

            // configurar VBO para los vértices
            _vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexData.Length * sizeof(float),
                         _vertexData, BufferUsageHint.StaticDraw);

            // configurar EBO para los índices
            _elementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint),
                         _indices, BufferUsageHint.StaticDraw);

            ConfigurarAtributos(shader);
        }

        private void ConfigurarAtributos(Shader shader)
        {
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        }

        public void Dibujar(Shader shader)
        {
            // Crear matriz de modelo
            Matrix4 model = Matrix4.CreateScale(_escala) *
                           Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotacion.X)) *
                           Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotacion.Y)) *
                           Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotacion.Z)) *
                           Matrix4.CreateTranslation(_posicion);

            // Actualizar shader
            shader.SetMatrix4("model", model);

            // dibujar líneas
            GL.BindVertexArray(_vertexArrayHandle);
            _figura.DibujarLineas(_indices.Length);
        }

        public void SetPosicion(float x, float y, float z)
        {
            _posicion = new Vector3(x, y, z);
        }

        public void SetRotacionY(float angulo)
        {
            _rotacion.Y = angulo;
        }

        public Vector3 ObtenerPosicion()
        {
            return _posicion;
        }

        public void Liberar()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(_vertexBufferHandle);
            GL.DeleteBuffer(_elementBufferHandle);
            GL.DeleteVertexArray(_vertexArrayHandle);
        }
    }
}