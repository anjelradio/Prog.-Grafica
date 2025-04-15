using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace U_3d.Clases
{
    public class Figura
    {
        // struct para vértices
        private struct Vertice
        {
            public Vector3 Posicion;
            public Vector3 Color;

            public Vertice(Vector3 posicion, Vector3 color)
            {
                Posicion = posicion;
                Color = color;
            }
        }

        private List<Vertice> _vertices;
        private List<uint> _indices;
        private Vector3 _origen;
        private float _ancho;
        private float _alto;
        private float _profundidad;
        private Vector3 _color;

        public Figura(Puntos origen, float ancho, float alto, float profundidad, Vector3 color)
        {
            _origen = origen.ToVector3();
            _ancho = ancho;
            _alto = alto;
            _profundidad = profundidad;
            _color = color;

            InicializarGeometria();
        }

        private void InicializarGeometria()
        {
            _vertices = new List<Vertice>();
            _indices = new List<uint>();

            //crear vertices para la forma U
            CrearVertices();
            //definir índices para las líneas
            CrearIndices();
        }

        private void CrearVertices()
        {
            //calcular posiciones 
            float x = _origen.X;
            float y = _origen.Y;
            float z = _origen.Z;

            //cara frontal
            _vertices.Add(new Vertice(new Vector3(x, y, z + _profundidad / 2), _color));                        // 0
            _vertices.Add(new Vertice(new Vector3(x, y + _alto, z + _profundidad / 2), _color));                // 1
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.2f, y + _alto, z + _profundidad / 2), _color));  // 2
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.2f, y + _alto * 0.2f, z + _profundidad / 2), _color)); // 3
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.8f, y + _alto * 0.2f, z + _profundidad / 2), _color)); // 4
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.8f, y + _alto, z + _profundidad / 2), _color));  // 5
            _vertices.Add(new Vertice(new Vector3(x + _ancho, y + _alto, z + _profundidad / 2), _color));       // 6
            _vertices.Add(new Vertice(new Vector3(x + _ancho, y, z + _profundidad / 2), _color));               // 7

            //cara trasera
            _vertices.Add(new Vertice(new Vector3(x, y, z - _profundidad / 2), _color));                        // 8
            _vertices.Add(new Vertice(new Vector3(x, y + _alto, z - _profundidad / 2), _color));                // 9
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.2f, y + _alto, z - _profundidad / 2), _color));  // 10
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.2f, y + _alto * 0.2f, z - _profundidad / 2), _color)); // 11
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.8f, y + _alto * 0.2f, z - _profundidad / 2), _color)); // 12
            _vertices.Add(new Vertice(new Vector3(x + _ancho * 0.8f, y + _alto, z - _profundidad / 2), _color));  // 13
            _vertices.Add(new Vertice(new Vector3(x + _ancho, y + _alto, z - _profundidad / 2), _color));       // 14
            _vertices.Add(new Vertice(new Vector3(x + _ancho, y, z - _profundidad / 2), _color));               // 15
        }

        private void CrearIndices()
        {
            //parte Frontal de la U
            _indices.AddRange(new uint[] { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 0 });

            // parte Trasera de la U
            _indices.AddRange(new uint[] { 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 8 });

            // conexiones entre la parte frontal y trasera
            _indices.AddRange(new uint[] { 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6, 14, 7, 15 });
        }

        public (float[] VertexData, uint[] Indices) ObtenerDatosRenderizado()
        {
            // convertir lista de vértices a formato para el buffer
            List<float> vertexData = new List<float>();
            foreach (var vertice in _vertices)
            {
                vertexData.Add(vertice.Posicion.X);
                vertexData.Add(vertice.Posicion.Y);
                vertexData.Add(vertice.Posicion.Z);
                vertexData.Add(vertice.Color.X);
                vertexData.Add(vertice.Color.Y);
                vertexData.Add(vertice.Color.Z);
            }

            return (vertexData.ToArray(), _indices.ToArray());
        }

        // Configuración para dibujar las líneas
        public void DibujarLineas(int indicesLength)
        {
            GL.LineWidth(2.0f);
            GL.Enable(EnableCap.LineSmooth);
            GL.DrawElements(PrimitiveType.Lines, indicesLength, DrawElementsType.UnsignedInt, 0);
        }
    }
}