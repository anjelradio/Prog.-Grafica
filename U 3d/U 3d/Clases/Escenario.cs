using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;
using System.Linq;


namespace U_3d.Clases
{
    public class Escenario
    {
        private Dictionary<string, Objeto> _objetos;
        private Camara _camara;
        private bool _pause;
        private float _separacionEntreUs = 2.5f;

        public Escenario()
        {
            _objetos = new Dictionary<string, Objeto>();
            _camara = new Camara();
            _pause = false;
        }

        public void Inicializar(int cantidadObjetos, Color4 color)
        {
            for (int i = 1; i <= cantidadObjetos; i++)
            {
                // Posición base
                float x = i * _separacionEntreUs; // Separar en eje X
                float y = 0.0f;
                float z = 0.0f;  

                string nombreObjeto = $"LetraU_{i}";
                _objetos[nombreObjeto] = new Objeto(
                    new Vector3(x, y, z),
                    1.5f, 1.5f, 0.3f,
                    color,
                    nombreObjeto
                );
            }
        }

        public void Renderizar()
        {
            foreach (var objeto in _objetos.Values)
            {
                objeto.Dibujar();
            }
        }

        public void Actualizar(KeyboardState input, float deltaTime)
        {
            if (input.IsKeyPressed(Keys.Space))
            {
                _pause = !_pause;
            }

            _camara.ProcesarMovimiento(input, deltaTime);
        }

        public Matrix4 ObtenerVistaMatriz()
        {
            return _camara.ObtenerMatrizVista();
        }

        // guardar y cargar escenas
        public void Guardar(string rutaArchivo)
        {
            var datos = new
            {
                Objetos = _objetos.Select(kvp => new
                {
                    Nombre = kvp.Key,
                    Centro = new { kvp.Value.Centro.X, kvp.Value.Centro.Y, kvp.Value.Centro.Z },
                    kvp.Value.AnchoPrincipal,
                    kvp.Value.AltoPrincipal,
                    kvp.Value.Profundidad,
                    Color = new[] { kvp.Value.Color.R, kvp.Value.Color.G, kvp.Value.Color.B, kvp.Value.Color.A }
                }).ToList(),
            };

            File.WriteAllText(rutaArchivo,
                JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void Cargar(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("El archivo no existe.", rutaArchivo);
            }

            var json = File.ReadAllText(rutaArchivo);
            var datos = JsonSerializer.Deserialize<JsonElement>(json);

            _objetos.Clear();

            foreach (var obj in datos.GetProperty("Objetos").EnumerateArray())
            {
                var colorArray = obj.GetProperty("Color").EnumerateArray().ToArray();
                var centro = new Vector3(
                    obj.GetProperty("Centro").GetProperty("X").GetSingle(),
                    obj.GetProperty("Centro").GetProperty("Y").GetSingle(),
                    obj.GetProperty("Centro").GetProperty("Z").GetSingle()
                );

                var color = new Color4(
                    colorArray[0].GetSingle(),
                    colorArray[1].GetSingle(),
                    colorArray[2].GetSingle(),
                    colorArray[3].GetSingle()
                );

                string nombre = obj.GetProperty("Nombre").GetString();
                _objetos[nombre] = new Objeto(
                    centro,
                    obj.GetProperty("AnchoPrincipal").GetSingle(),
                    obj.GetProperty("AltoPrincipal").GetSingle(),
                    obj.GetProperty("Profundidad").GetSingle(),
                    color,
                    nombre
                );
            }
        }

        // acceder a objetos, partes y caras
        public Objeto ObtenerObjeto(string nombreObjeto)
        {
            if (_objetos.ContainsKey(nombreObjeto))
                return _objetos[nombreObjeto];
            throw new KeyNotFoundException($"El objeto '{nombreObjeto}' no existe en el escenario");
        }

        // index para acceder a los objetos
        public Objeto this[string nombreObjeto]
        {
            get => ObtenerObjeto(nombreObjeto);
        }
        public Dictionary<string, Objeto> Objetos => _objetos;
    }
}