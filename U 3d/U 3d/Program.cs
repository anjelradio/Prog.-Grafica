using OpenTK.Mathematics;

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
                    // Utilidades(game);
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
            var escenario = game.ObtenerEscenario();


            Clases.Utilidades.Rotar(escenario["LetraU_1"], 90f, 0f, 0f);
            Clases.Utilidades.Trasladar(escenario["LetraU_1"]["Base"]["Inferior"], new Vector3(0f, -1f, 1f));
            Clases.Utilidades.Trasladar(escenario["LetraU_2"], new Vector3(0f, 0f, 1f));
            Clases.Utilidades.Escalar(escenario["LetraU_3"]["LateralIzquierdo"], new Vector3(1f, 2f, 1f));

        }


    }
}