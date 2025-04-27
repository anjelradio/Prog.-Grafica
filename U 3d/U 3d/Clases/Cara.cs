using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public class Cara : ITransformable
    {
        private Vector3[] _vertices;
        private Vector3[] _verticesOriginales;
        private Color4 _color;
        private string _nombre;
        private Vector3 _centro;
        private Matrix4 _matrizTransformacion;

        public Cara(Vector3[] vertices, Color4 color, string nombre)
        {
            _verticesOriginales = vertices;
            _vertices = new Vector3[vertices.Length];
            Array.Copy(vertices, _vertices, vertices.Length);

            _color = color;
            _nombre = nombre;
            _matrizTransformacion = Matrix4.Identity;

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

            Matrix4 matrizFinal = matrizPadre * _matrizTransformacion;

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(_color);

            foreach (var vertice in _verticesOriginales)
            {
                Vector3 transformado = Vector3.TransformPosition(vertice, matrizFinal);
                GL.Vertex3(transformado);
            }

            GL.End();
            GL.PopMatrix();
        }

        public void AplicarRotacion(float anguloX, float anguloY, float anguloZ)
        {
            Vector3 centroActual = _centro;
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 rotacion = Utilidades.CrearMatrizRotacion(anguloX, anguloY, anguloZ);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);
            Matrix4 transformacionFinal = trasladarAlOrigen * rotacion * trasladarDeVuelta;
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;
        }

        public void AplicarTraslacion(Vector3 traslacion)
        {
            Matrix4 matrizTraslacion = Utilidades.CrearMatrizTraslacion(traslacion);
            _matrizTransformacion = matrizTraslacion * _matrizTransformacion;
            _centro += traslacion;
        }

        public void AplicarEscalado(Vector3 escala)
        {
            Vector3 centroActual = _centro;
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 escalado = Utilidades.CrearMatrizEscalado(escala);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);
            Matrix4 transformacionFinal = trasladarAlOrigen * escalado * trasladarDeVuelta;
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