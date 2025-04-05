using System;
using OpenTK.Graphics.OpenGL;
namespace U_3d.Clases
{
    public class Caras
    {
        public uint[] Edges { get; private set; }
        public Caras()
        {
            Edges = new uint[] {
                // Parte Frontal de la U
                0, 1, 1, 2, 2, 3,
                0, 1, 1, 2, 2, 3,
                3, 4, 4, 5, 5, 6,
                6, 7, 7, 0,
                // Parte Trasera de la U
                8, 9, 9, 10, 10, 11,
                11, 12, 12, 13, 13, 14,
                14, 15, 15, 8,
                // Conexiones Trasera y frontal
                1, 9, 0, 8, 2, 10, 3, 11,
                6, 14, 7, 15, 5, 13, 4, 12
            };
        }
        public int GetEdgesLength() => Edges.Length;
    }
}