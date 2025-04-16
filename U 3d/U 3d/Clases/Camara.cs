using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d.Clases
{
    public class Camara
    {
        public Vector3 Posicion { get; private set; } = new Vector3(0.0f, 0.0f, 5.0f);
        public Vector3 Frente { get; private set; } = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 Arriba { get; private set; } = Vector3.UnitY;

        private float _yaw = -90.0f;
        private float _pitch = 0.0f;
        private float _velocidad = 2.5f;
        private float _velocidadRotacion = 45.0f;

        public void ActualizarPosicion(Vector3 nuevaPosicion)
        {
            Posicion = nuevaPosicion;
        }

        public void ProcesarMovimiento(KeyboardState input, float deltaTime)
        {
            float velocidadCamara = _velocidad * deltaTime;

            if (input.IsKeyDown(Keys.W))
                Posicion += velocidadCamara * Frente;
            if (input.IsKeyDown(Keys.S))
                Posicion -= velocidadCamara * Frente;
            if (input.IsKeyDown(Keys.A))
                Posicion -= Vector3.Normalize(Vector3.Cross(Frente, Arriba)) * velocidadCamara;
            if (input.IsKeyDown(Keys.D))
                Posicion += Vector3.Normalize(Vector3.Cross(Frente, Arriba)) * velocidadCamara;

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

            // Limitar el pitch para evitar inversiones de la cámara
            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

            ActualizarVectores();
        }

        private void ActualizarVectores()
        {
            // Calcular nueva dirección frontal
            Vector3 direccion;
            direccion.X = (float)Math.Cos(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            direccion.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            direccion.Z = (float)Math.Sin(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            Frente = Vector3.Normalize(direccion);
        }

        public Matrix4 ObtenerMatrizVista()
        {
            return Matrix4.LookAt(Posicion, Posicion + Frente, Arriba);
        }
    }
}