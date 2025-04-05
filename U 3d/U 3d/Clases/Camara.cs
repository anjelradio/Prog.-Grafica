using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d
{
    public class Camara
    {
        public Vector3 Position { get; private set; } = new Vector3(0.0f, 0.0f, 5.0f);
        public Vector3 Front { get; private set; } = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 Up { get; private set; } = Vector3.UnitY;

        private float _yaw = -90.0f;
        private float _pitch = 0.0f;
        private float _speed = 1.5f;
        private float _rotationSpeed = 45.0f;

        public void ProcesarMovimiento(KeyboardState input, float deltaTime)
        {
            float cameraSpeed = _speed * deltaTime;

            if (input.IsKeyDown(Keys.W))
                Position += cameraSpeed * Front;
            if (input.IsKeyDown(Keys.S))
                Position -= cameraSpeed * Front;
            if (input.IsKeyDown(Keys.A))
                Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * cameraSpeed;
            if (input.IsKeyDown(Keys.D))
                Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * cameraSpeed;

            ProcesarRotacion(input, deltaTime);
        }

        private void ProcesarRotacion(KeyboardState input, float deltaTime)
        {
            if (input.IsKeyDown(Keys.Up))
                _pitch += _rotationSpeed * deltaTime;
            if (input.IsKeyDown(Keys.Down))
                _pitch -= _rotationSpeed * deltaTime;
            if (input.IsKeyDown(Keys.Left))
                _yaw -= _rotationSpeed * deltaTime;
            if (input.IsKeyDown(Keys.Right))
                _yaw += _rotationSpeed * deltaTime;

            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

            ActualizarDireccionCamara();
        }

        private void ActualizarDireccionCamara()
        {
            Vector3 direction;
            direction.X = (float)Math.Cos(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            direction.Z = (float)Math.Sin(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            Front = Vector3.Normalize(direction);
        }

        public Matrix4 ObtenerMatrizVista() => Matrix4.LookAt(Position, Position + Front, Up);
    }
}