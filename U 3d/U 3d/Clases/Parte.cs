using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public class Parte : ITransformable
    {
        private Dictionary<string, Cara> _caras;
        private Vector3 _posicion;
        private Vector3 _dimension;
        private string _nombre;
        private Color4 _color;
        private Matrix4 _matrizTransformacion;

        public Parte(Vector3 posicion, float ancho, float alto, float profundidad, Color4 color, string nombre)
        {
            _posicion = posicion;
            _dimension = new Vector3(ancho, alto, profundidad);
            _nombre = nombre;
            _color = color;
            _caras = new Dictionary<string, Cara>();
            _matrizTransformacion = Matrix4.Identity;

            CrearCubo();
        }

        private void CrearCubo()
        {
            List<Vector3> vertices = new List<Vector3>
            {
                new Vector3(_posicion.X, _posicion.Y, _posicion.Z),
                new Vector3(_posicion.X + _dimension.X, _posicion.Y, _posicion.Z),
                new Vector3(_posicion.X + _dimension.X, _posicion.Y + _dimension.Y, _posicion.Z),
                new Vector3(_posicion.X, _posicion.Y + _dimension.Y, _posicion.Z),
                new Vector3(_posicion.X, _posicion.Y, _posicion.Z + _dimension.Z),
                new Vector3(_posicion.X + _dimension.X, _posicion.Y, _posicion.Z + _dimension.Z),
                new Vector3(_posicion.X + _dimension.X, _posicion.Y + _dimension.Y, _posicion.Z + _dimension.Z),
                new Vector3(_posicion.X, _posicion.Y + _dimension.Y, _posicion.Z + _dimension.Z)
            };

            _caras["Trasera"] = new Cara(new[] { vertices[0], vertices[1], vertices[2], vertices[3] }, _color, "Trasera");
            _caras["Frontal"] = new Cara(new[] { vertices[5], vertices[4], vertices[7], vertices[6] }, _color, "Frontal");
            _caras["Izquierda"] = new Cara(new[] { vertices[4], vertices[0], vertices[3], vertices[7] }, _color, "Izquierda");
            _caras["Derecha"] = new Cara(new[] { vertices[1], vertices[5], vertices[6], vertices[2] }, _color, "Derecha");
            _caras["Superior"] = new Cara(new[] { vertices[3], vertices[2], vertices[6], vertices[7] }, _color, "Superior");
            _caras["Inferior"] = new Cara(new[] { vertices[4], vertices[5], vertices[1], vertices[0] }, _color, "Inferior");
        }

        public void Dibujar(Matrix4 matrizPadre)
        {
            Matrix4 matrizFinal = _matrizTransformacion * matrizPadre;

            foreach (var cara in _caras.Values)
            {
                cara.Dibujar(matrizFinal);
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
            _posicion += traslacion;
        }

        public void AplicarEscalado(Vector3 escala)
        {
            Vector3 centroActual = Centro;
            Matrix4 trasladarAlOrigen = Matrix4.CreateTranslation(-centroActual);
            Matrix4 escalado = Utilidades.CrearMatrizEscalado(escala);
            Matrix4 trasladarDeVuelta = Matrix4.CreateTranslation(centroActual);
            Matrix4 transformacionFinal = trasladarAlOrigen * escalado * trasladarDeVuelta;
            _matrizTransformacion = transformacionFinal * _matrizTransformacion;
            _dimension *= escala;
        }

        public Matrix4 MatrizTransformacion => _matrizTransformacion;

        public void RestablecerTransformaciones()
        {
            _matrizTransformacion = Matrix4.Identity;
        }

        public Vector3 Posicion => _posicion;
        public Vector3 Centro => _posicion + _dimension / 2.0f;
        public string Nombre => _nombre;
        public Dictionary<string, Cara> Caras => _caras;

        public Cara this[string nombreCara]
        {
            get
            {
                if (_caras.ContainsKey(nombreCara))
                    return _caras[nombreCara];
                throw new KeyNotFoundException($"La cara '{nombreCara}' no existe en la parte '{_nombre}'");
            }
        }
    }
}