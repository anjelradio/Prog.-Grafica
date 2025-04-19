using OpenTK.Mathematics;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace U_3d.Clases
{
    public class Objeto : ITransformable
    {
        private Dictionary<string, Parte> _partes;
        private Vector3 _centro;
        private float _anchoPrincipal;
        private float _altoPrincipal;
        private float _profundidad;
        private Color4 _color;
        private string _nombre;
        private Matrix4 _matrizTransformacion;

        public Objeto(Vector3 centro, float ancho, float alto, float profundidad, Color4 color, string nombre)
        {
            _centro = centro;
            _anchoPrincipal = ancho;
            _altoPrincipal = alto;
            _profundidad = profundidad;
            _color = color;
            _nombre = nombre;
            _partes = new Dictionary<string, Parte>();
            _matrizTransformacion = Matrix4.Identity;

            CrearPartes();
        }

        private void CrearPartes()
        {
            // limpiar partes existentes
            _partes.Clear();

            // dimensiones de las partes
            float anchoLateral = _anchoPrincipal / 4f;
            float altoLateral = _altoPrincipal;
            float anchoBase = _anchoPrincipal - 2 * anchoLateral;
            float posXBase = _centro.X - _anchoPrincipal / 2f + anchoLateral;
            float altoBase = anchoLateral;

            // posiciones relativas al centro
            float posXIzquierdo = _centro.X - _anchoPrincipal / 2f;
            float posXDerecho = _centro.X + _anchoPrincipal / 2f - anchoLateral;
            float posYBase = _centro.Y - _altoPrincipal / 2f;
            float posZ = _centro.Z - _profundidad / 2f;

            _partes["LateralIzquierdo"] = new Parte(
                new Vector3(posXIzquierdo, posYBase, posZ),
                anchoLateral, altoLateral, _profundidad,
                _color,
                "LateralIzquierdo"
            );

            _partes["LateralDerecho"] = new Parte(
                new Vector3(posXDerecho, posYBase, posZ),
                anchoLateral, altoLateral, _profundidad,
                _color,
                "LateralDerecho"
            );

            _partes["Base"] = new Parte(
                new Vector3(posXBase, posYBase, posZ),
               anchoBase, altoBase, _profundidad,
              _color,
              "Base"
            );
        }

        public void Dibujar()
        {
            GL.PushMatrix();
            GL.MultMatrix(ref _matrizTransformacion);

            // dibujar cada parte con la matriz de identidad
            foreach (var parte in _partes.Values)
            {
                parte.Dibujar(Matrix4.Identity);
            }

            GL.PopMatrix();
        }

        // implementación de ITransformable
        public void AplicarRotacion(float anguloX, float anguloY, float anguloZ)
        {
            // Calculamos el centro como el punto medio del objeto
            Vector3 centroActual = Centro;

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

            // Actualizamos la posición de cada parte también
            CrearPartes();
        }

        public void AplicarEscalado(Vector3 escala)
        {
            // Calculamos el centro como el punto medio del objeto
            Vector3 centroActual = Centro;

            // Creamos matriz de traslación al origen, escalado, y traslación de vuelta
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 escalado = Utilidades.CrearMatrizEscalado(escala);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);

            // Combinamos las transformaciones
            Matrix4 transformacionFinal = trasladarAlOrigen * escalado * trasladarDeVuelta;

            // Aplicamos a nuestra matriz de transformación actual
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;

            // Actualizamos las dimensiones
            _anchoPrincipal *= escala.X;
            _altoPrincipal *= escala.Y;
            _profundidad *= escala.Z;

            // Recreamos las partes con las nuevas dimensiones
            CrearPartes();
        }

        public Matrix4 MatrizTransformacion => _matrizTransformacion;

        public void RestablecerTransformaciones()
        {
            _matrizTransformacion = Matrix4.Identity;
        }

        // Propiedades
        public Vector3 Centro => _centro;
        public float AnchoPrincipal => _anchoPrincipal;
        public float AltoPrincipal => _altoPrincipal;
        public float Profundidad => _profundidad;
        public Color4 Color => _color;
        public string Nombre => _nombre;
        public Dictionary<string, Parte> Partes => _partes;

        // Indexador para acceder a las partes como si fuera un diccionario
        public Parte this[string nombreParte]
        {
            get
            {
                if (_partes.ContainsKey(nombreParte))
                    return _partes[nombreParte];
                throw new KeyNotFoundException($"La parte '{nombreParte}' no existe en el objeto '{_nombre}'");
            }
        }
    }
}