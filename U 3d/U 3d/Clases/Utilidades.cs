using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace U_3d.Clases
{
    public static class Utilidades
    {
        // Rotación: funciona con cualquier elemento transformable o con todos si se pasa una cadena vacía
        public static void Rotar<T>(T elemento, float anguloX = 0, float anguloY = 0, float anguloZ = 0) where T : ITransformable
        {
            // Aplicar rotación al elemento
            elemento.AplicarRotacion(anguloX, anguloY, anguloZ);
        }

        // Sobrecarga para rotar todos los objetos del escenario
        public static void Rotar(Escenario escenario, float anguloX = 0, float anguloY = 0, float anguloZ = 0)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarRotacion(anguloX, anguloY, anguloZ);
            }
        }

  

        // Traslación: funciona con cualquier elemento transformable
        public static void Trasladar<T>(T elemento, Vector3 traslacion) where T : ITransformable
        {
            // Aplicar traslación al elemento
            elemento.AplicarTraslacion(traslacion);
        }

        // Sobrecarga para trasladar todos los objetos del escenario
        public static void Trasladar(Escenario escenario, Vector3 traslacion)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarTraslacion(traslacion);
            }
        }


        // Escalado: funciona con cualquier elemento transformable
        public static void Escalar<T>(T elemento, Vector3 escala) where T : ITransformable
        {
            // Aplicar escalado al elemento
            elemento.AplicarEscalado(escala);
        }

        // Sobrecarga para escalar todos los objetos del escenario
        public static void Escalar(Escenario escenario, Vector3 escala)
        {
            foreach (var objeto in escenario.Objetos.Values)
            {
                objeto.AplicarEscalado(escala);
            }
        }


        // Métodos de ayuda para transformaciones de matrices
        public static Matrix4 CrearMatrizRotacion(float anguloX, float anguloY, float anguloZ)
        {
            Matrix4 rotX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-anguloX)); // Invertir el signo aquí
            Matrix4 rotY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-anguloY));
            Matrix4 rotZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(anguloZ));

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