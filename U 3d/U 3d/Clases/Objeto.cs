using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

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
            _partes.Clear();

            float anchoLateral = _anchoPrincipal / 4f;
            float altoLateral = _altoPrincipal;
            float anchoBase = _anchoPrincipal - 2 * anchoLateral;
            float posXBase = _centro.X - _anchoPrincipal / 2f + anchoLateral;
            float altoBase = anchoLateral;

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
            Matrix4 matrizIdentidad = Matrix4.Identity;

            foreach (var parte in _partes.Values)
            {
                parte.Dibujar(_matrizTransformacion * matrizIdentidad);
            }
        }

        public void AplicarRotacion(float anguloX, float anguloY, float anguloZ)
        {
            Vector3 centroActual = Centro;
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
            Vector3 centroActual = Centro;
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 escalado = Utilidades.CrearMatrizEscalado(escala);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);
            Matrix4 transformacionFinal = trasladarAlOrigen * escalado * trasladarDeVuelta;
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;

            _anchoPrincipal *= escala.X;
            _altoPrincipal *= escala.Y;
            _profundidad *= escala.Z;
            CrearPartes();
        }

        public Matrix4 MatrizTransformacion => _matrizTransformacion;

        public void RestablecerTransformaciones()
        {
            _matrizTransformacion = Matrix4.Identity;
        }

        public Vector3 Centro => _centro;
        public float AnchoPrincipal => _anchoPrincipal;
        public float AltoPrincipal => _altoPrincipal;
        public float Profundidad => _profundidad;
        public Color4 Color => _color;
        public string Nombre => _nombre;
        public Dictionary<string, Parte> Partes => _partes;

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