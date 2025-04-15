using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d.Clases
{
    public class Escenario
    {
        private List<Objeto> _objetos;
        private Camara _camara;
        private Shader _shader;
        private Matrix4 _projection;
        private float _rotacionTotal = 0.0f;
        private bool _pausado = false;
        private float _separacionEntreUs = 2.5f;
        private bool _inicializado = false;

        public Escenario(int cantidadU)
        {
            _camara = new Camara();
            _shader = new Shader();
            _objetos = new List<Objeto>();

            // Crear objetos U según la cantidad especificada
            for (int i = 0; i < cantidadU; i++)
            {
                float posicionX = i * _separacionEntreUs;
                Objeto objeto = new Objeto(
                    new Puntos(posicionX, 0, 0),
                    2.0f, 2.0f, 1.0f,
                    new Vector3(1.0f, 1.0f, 1.0f)
                );
                _objetos.Add(objeto);
            }

            // Posicionar la cámara frente al primer objeto
            if (_objetos.Count > 0)
            {
                Vector3 posicionPrimerObjeto = _objetos[0].ObtenerPosicion();
                _camara.ActualizarPosicion(new Vector3(posicionPrimerObjeto.X, posicionPrimerObjeto.Y, posicionPrimerObjeto.Z + 10.0f));
            }
        }

        public void Inicializar(float width, float height)
        {
            if (_inicializado)
                return;

            GL.Enable(EnableCap.DepthTest);

            _shader.Use();

            // Inicializar todos los objetos
            foreach (var objeto in _objetos)
            {
                objeto.Inicializar(_shader);
            }

            ActualizarProyeccion(width, height);
            _inicializado = true;
        }

        public void ActualizarProyeccion(float width, float height)
        {
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                width / height,
                0.1f,
                100.0f
            );

            _shader.Use();
            _shader.SetMatrix4("projection", _projection);
        }

        public void Actualizar(KeyboardState input, float deltaTime)
        {
            // Procesar pausado de rotación
            if (input.IsKeyPressed(Keys.Space))
                _pausado = !_pausado;

            // Actualizar cámara
            _camara.ProcesarMovimiento(input, deltaTime);

            // Actualizar rotación global si no está pausado
            if (!_pausado)
                _rotacionTotal += 15.0f * deltaTime;

            // Actualizar rotación de cada objeto
            foreach (var objeto in _objetos)
            {
                objeto.SetRotacionY(_rotacionTotal);
            }
        }

        public void Renderizar()
        {
            _shader.Use();

            // Configurar matriz de vista
            var view = _camara.ObtenerMatrizVista();
            _shader.SetMatrix4("view", view);

            // Dibujar ejes de coordenadas
            DibujarEjes();

            // Renderizar cada objeto
            foreach (var objeto in _objetos)
            {
                objeto.Dibujar(_shader);
            }
        }

        private void DibujarEjes()
        {
            // Dibujar ejes X, Y, Z para referencia
            GL.LineWidth(3.0f);
            GL.Begin(PrimitiveType.Lines);

            // eje X (rojo)
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-5.0f, 0.0f, 0.0f);
            GL.Vertex3(5.0f, 0.0f, 0.0f);

            // eje Y (verde)
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, -5.0f, 0.0f);
            GL.Vertex3(0.0f, 5.0f, 0.0f);

            // eje Z (azul)
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, -5.0f);
            GL.Vertex3(0.0f, 0.0f, 5.0f);

            GL.End();
        }

        public void Liberar()
        {
            foreach (var objeto in _objetos)
            {
                objeto.Liberar();
            }

            GL.UseProgram(0);
            GL.DeleteProgram(_shader.Handle);
        }
    }
}