using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public static class Utilidades
    {
        public static void Rotar<T>(T elemento, float anguloX = 0, float anguloY = 0, float anguloZ = 0) where T : ITransformable
        {
            elemento.AplicarRotacion(anguloX, anguloY, anguloZ);
        }

        public static void Rotar(Escenario escenario, float anguloX = 0, float anguloY = 0, float anguloZ = 0)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarRotacion(anguloX, anguloY, anguloZ);
            }
        }

        public static void Trasladar<T>(T elemento, Vector3 traslacion) where T : ITransformable
        {
            Matrix4 rotacionActual = elemento.MatrizTransformacion;

            Matrix3 matrizRotacion = new Matrix3(
                rotacionActual.M11, rotacionActual.M12, rotacionActual.M13,
                rotacionActual.M21, rotacionActual.M22, rotacionActual.M23,
                rotacionActual.M31, rotacionActual.M32, rotacionActual.M33
            );
            Vector3 traslacionLocal = matrizRotacion * traslacion;
            elemento.AplicarTraslacion(traslacionLocal);
        }

        public static void Trasladar(Escenario escenario, Vector3 traslacion)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarTraslacion(traslacion);
            }
        }

        public static void Escalar<T>(T elemento, Vector3 escala) where T : ITransformable
        {
            elemento.AplicarEscalado(escala);
        }

        public static void Escalar(Escenario escenario, Vector3 escala)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarEscalado(escala);
            }
        }

        public static Matrix4 CrearMatrizRotacion(float anguloX, float anguloY, float anguloZ)
        {
            Matrix4 rotX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-anguloX));
            Matrix4 rotY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-anguloY));
            Matrix4 rotZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-anguloZ));

            return rotX * rotY * rotZ;
        }

        public static Matrix4 CrearMatrizTraslacion(Vector3 traslacion)
        {
            return Matrix4.CreateTranslation(traslacion);
        }

        public static Matrix4 CrearMatrizEscalado(Vector3 escala)
        {
            return Matrix4.CreateScale(escala);
        }
    }
}