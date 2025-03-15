
using System;

namespace U_3d
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720, "Tarea 1 - U en 3D"))
            {
                game.Run();
            }  
        }
    }
}

