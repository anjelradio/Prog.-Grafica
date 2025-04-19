using OpenTK.Mathematics;

namespace U_3d.Clases
{
    public interface ITransformable
    {
        // transformaciones
        void AplicarRotacion(float anguloX, float anguloY, float anguloZ);
        void AplicarTraslacion(Vector3 traslacion);
        void AplicarEscalado(Vector3 escala);

        // obtener la matriz de transformación acumulada
        Matrix4 MatrizTransformacion { get; }

        // restablecer transformaciones a sus valores iniciales
        void RestablecerTransformaciones();

        // centro de transformación
        Vector3 Centro { get; }
    }
}