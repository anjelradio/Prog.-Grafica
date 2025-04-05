using System;
using U_3d;

namespace U_3d
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("Ingrese la cantidad de U's a dibujar: ");
            if (int.TryParse(Console.ReadLine(), out int cantidadU) && cantidadU > 0)
            {
                using (Game game = new Game(1280, 720, "U en 3D", cantidadU))
                {
                    game.Run();
                }
            }
            else
            {
                Console.WriteLine("Cantidad inválida. Por favor, ingrese un número positivo.");
            }
        }
    }
}