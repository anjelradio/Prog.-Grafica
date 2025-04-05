using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using U_3d.Clases;

namespace U_3d
{
    public class Objeto
    {
        private int _vertexArrayHandle;
        private int _vertexBufferHandle;
        private int _elementBufferHandle;
        private Puntos _puntos;
        private Caras _caras;

        public Objeto(Puntos puntos, Caras caras)
        {
            _puntos = puntos;
            _caras = caras;
        }

        public void DesplazarHorizontalmente(float desplazamiento)
        {
            // Modificar los vértices para desplazar la U horizontalmente
            for (int i = 0; i < _puntos.OriginalVertices.Length; i += 6)
            {
                _puntos.CurrentVertices[i] = _puntos.OriginalVertices[i] + desplazamiento;
            }
        }

        public void Inicializar(Shader shader)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);

            _vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _puntos.GetVerticesLength() * sizeof(float), _puntos.CurrentVertices, BufferUsageHint.DynamicDraw);

            _elementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _caras.GetEdgesLength() * sizeof(uint), _caras.Edges, BufferUsageHint.StaticDraw);

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

        public void DibujarObjeto(Lineas lineas)
        {
            GL.BindVertexArray(_vertexArrayHandle);
            lineas.DibujarLineas(_caras.GetEdgesLength());
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