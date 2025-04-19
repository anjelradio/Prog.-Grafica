using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;

namespace U_3d.Clases
{
    public class Cara : ITransformable
    {
        private Vector3[] _vertices;
        private Vector3[] _verticesOriginales; // Mantener copia de los vértices originales
        private Color4 _color;
        private string _nombre;
        private Vector3 _centro;
        private Matrix4 _matrizTransformacion;

        public Cara(Vector3[] vertices, Color4 color, string nombre)
        {
            if (vertices == null || vertices.Length < 3)
                throw new ArgumentException("Una cara debe tener al menos 3 vértices.");

            _verticesOriginales = vertices;
            _vertices = new Vector3[vertices.Length];
            Array.Copy(vertices, _vertices, vertices.Length);

            _color = color;
            _nombre = nombre;
            _matrizTransformacion = Matrix4.Identity;

            // Calcular el centro de la cara
            CalcularCentro();
        }

        private void CalcularCentro()
        {
            Vector3 suma = new Vector3();
            foreach (var v in _vertices)
            {
                suma += v;
            }
            _centro = suma / _vertices.Length;
        }

        public void Dibujar(Matrix4 matrizPadre)
        {
            // La matriz final combina la transformación padre con la transformación propia
            Matrix4 matrizFinal = _matrizTransformacion * matrizPadre;

            PrimitiveType tipoPrimitiva = _vertices.Length switch
            {
                2 => PrimitiveType.Lines,
                3 => PrimitiveType.Triangles,
                4 => PrimitiveType.Quads,
                _ => PrimitiveType.Polygon
            };

            GL.PushMatrix();
            GL.MultMatrix(ref matrizFinal);

            GL.Begin(tipoPrimitiva);
            GL.Color4(_color);

            foreach (var vertice in _verticesOriginales)
            {
                GL.Vertex3(vertice);
            }

            GL.End();
            GL.PopMatrix();
        }

        // Implementación de ITransformable
        public void AplicarRotacion(float anguloX, float anguloY, float anguloZ)
        {
            // Guardamos el centro actual
            Vector3 centroActual = _centro;

            // Creamos matriz de traslación al origen, rotación, y traslación de vuelta
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 rotacion = Utilidades.CrearMatrizRotacion(anguloX, anguloY, anguloZ);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);

            // Combinamos las transformaciones
            Matrix4 transformacionFinal = trasladarAlOrigen * rotacion * trasladarDeVuelta;

            // Aplicamos a nuestra matriz de transformación actual
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;
        }

        public void AplicarTraslacion(Vector3 traslacion)
        {
            Matrix4 matrizTraslacion = Utilidades.CrearMatrizTraslacion(traslacion);
            _matrizTransformacion = matrizTraslacion * _matrizTransformacion;
            _centro += traslacion; // Actualizamos el centro
        }

        public void AplicarEscalado(Vector3 escala)
        {
            // Guardamos el centro actual
            Vector3 centroActual = _centro;

            // Creamos matriz de traslación al origen, escalado, y traslación de vuelta
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 escalado = Utilidades.CrearMatrizEscalado(escala);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);

            // Combinamos las transformaciones
            Matrix4 transformacionFinal = trasladarAlOrigen * escalado * trasladarDeVuelta;

            // Aplicamos a nuestra matriz de transformación actual
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;
        }

        public Matrix4 MatrizTransformacion => _matrizTransformacion;

        public void RestablecerTransformaciones()
        {
            _matrizTransformacion = Matrix4.Identity;
        }

        public Vector3 Centro => _centro;
        public string Nombre => _nombre;
        public Color4 Color => _color;
    }
}