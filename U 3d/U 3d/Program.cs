using System;
using OpenTK.Mathematics;
using U_3d.Clases;
using System.Threading;
using ImGuiNET;

namespace U_3d.Clases
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("Ingrese la cantidad de U's a dibujar: ");
            if (int.TryParse(Console.ReadLine(), out int cantidadU) && cantidadU > 0)
            {
             
                using (Game game = new Game(1280, 720, "Manipulación de Objetos 3D", cantidadU))
                {
                    Utilidades(game);
                    game.Run();
                    
                }
            }
            else
            {
                Console.WriteLine("Cantidad inválida. Por favor, ingrese un número positivo.");
            }
        }

        private static void Utilidades(Game game)
        {
            // Obtenemos el escenario para realizar pruebas
            var escenario = game.ObtenerEscenario();
            Clases.Utilidades.Rotar(escenario["LetraU_2"],0f, 0f, 45f);
            Clases.Utilidades.Rotar(escenario["LetraU_2"]["LateralDerecho"], 0f, 0f, 45f);
            Clases.Utilidades.Trasladar(escenario["LetraU_2"]["LateralDerecho"]["Frontal"], new Vector3(0f,0f,3f));
            Clases.Utilidades.Trasladar(escenario["LetraU_2"]["LateralDerecho"]["Inferior"], new Vector3(0f,-1.2f, 0f));
        }

        
    }
}