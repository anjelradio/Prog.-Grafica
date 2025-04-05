using System;
using OpenTK.Mathematics;
namespace U_3d.Clases
{
    public class Puntos
    {
        public float[] OriginalVertices { get; private set; }
        public float[] CurrentVertices { get; private set; }
        public Puntos()
        {
            OriginalVertices = new float[] {
                // Posiciones XYZ           // Colores RGB 
                // Cara frontal de la U
                -1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 0
                -1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 1
                -0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 2
                -0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 3
                 0.6f, -0.6f,  0.5f,        1.0f, 1.0f, 1.0f, // 4
                 0.6f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 5
                 1.0f,  1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 6
                 1.0f, -1.0f,  0.5f,        1.0f, 1.0f, 1.0f, // 7

                // Cara trasera de la U
                 -1.0f, -1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 8
                 -1.0f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 9
                 -0.6f,  1.0f,  -0.5f,        1.0f, 1.0f, 1.0f, // 10
                 -0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 11
                 0.6f, -0.6f, -0.5f,        1.0f, 1.0f, 1.0f, // 12
                 0.6f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 13
                 1.0f,  1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 14
                 1.0f, -1.0f, -0.5f,        1.0f, 1.0f, 1.0f, // 15
            };
            CurrentVertices = new float[OriginalVertices.Length];
            Array.Copy(OriginalVertices, CurrentVertices, OriginalVertices.Length);
        }
        public void ActualizarVertices(float setX, float setY, float setZ)
        {
            for (int i = 0; i < OriginalVertices.Length; i += 6)
            {
                CurrentVertices[i] = OriginalVertices[i] + setX;
                CurrentVertices[i + 1] = OriginalVertices[i + 1] + setY;
                CurrentVertices[i + 2] = OriginalVertices[i + 2] + setZ;
            }
        }
        public int GetVerticesLength() => CurrentVertices.Length;
    }
}