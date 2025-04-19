using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public class Puntos
    {
  
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Puntos(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //facilitar conversión a Vector3
        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}