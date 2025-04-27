using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d.Clases
{
    public class Camara
    {
        private Vector3 _posicion;
        private Vector3 _frente;
        private Vector3 _arriba;
        private float _yaw;
        private float _pitch;
        private float _velocidad;
        private float _velocidadRotacion;

        public Camara()
        {
            _posicion = new Vector3(2.5f, 0.0f, 3.0f);
            _frente = new Vector3(0.0f, 0.0f, -1.0f);
            _arriba = Vector3.UnitY;
            _yaw = -90.0f;
            _pitch = 0.0f;
            _velocidad = 2.5f;
            _velocidadRotacion = 45.0f;
        }

        public void ProcesarMovimiento(KeyboardState input, float deltaTime)
        {
            float velocidadCamara = _velocidad * deltaTime;

            if (input.IsKeyDown(Keys.W))
                _posicion += velocidadCamara * _frente;
            if (input.IsKeyDown(Keys.S))
                _posicion -= velocidadCamara * _frente;
            if (input.IsKeyDown(Keys.A))
                _posicion -= Vector3.Normalize(Vector3.Cross(_frente, _arriba)) * velocidadCamara;
            if (input.IsKeyDown(Keys.D))
                _posicion += Vector3.Normalize(Vector3.Cross(_frente, _arriba)) * velocidadCamara;
            if (input.IsKeyDown(Keys.E))
                _posicion += velocidadCamara * _arriba;
            if (input.IsKeyDown(Keys.Q))
                _posicion -= velocidadCamara * _arriba;

            ProcesarRotacion(input, deltaTime);
        }

        private void ProcesarRotacion(KeyboardState input, float deltaTime)
        {
            if (input.IsKeyDown(Keys.Up))
                _pitch += _velocidadRotacion * deltaTime;
            if (input.IsKeyDown(Keys.Down))
                _pitch -= _velocidadRotacion * deltaTime;
            if (input.IsKeyDown(Keys.Left))
                _yaw -= _velocidadRotacion * deltaTime;
            if (input.IsKeyDown(Keys.Right))
                _yaw += _velocidadRotacion * deltaTime;

            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

            ActualizarVectores();
        }

        private void ActualizarVectores()
        {
            Vector3 direccion;
            direccion.X = (float)Math.Cos(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            direccion.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            direccion.Z = (float)Math.Sin(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            _frente = Vector3.Normalize(direccion);
        }

        public Matrix4 ObtenerMatrizVista()
        {
            return Matrix4.LookAt(_posicion, _posicion + _frente, _arriba);
        }
    }
}