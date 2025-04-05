using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U_3d.Clases
{
    public class Shader
    {
        public readonly int Handle;
        private readonly Dictionary<string, int> _uniformLocations;

        public Shader(string vertexPath, string fragmentPath)
        {
            // Crear shader vertex y fragment
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexPath);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentPath);

            // Crear programa y vincular shaders
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            // Verificar errores de enlace
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine($"ERROR: Fallo al enlazar el programa: {infoLog}");
            }

            // Eliminar shaders que ya están vinculados y no necesitamos más
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Mapear todas las ubicaciones de uniforms
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            _uniformLocations = new Dictionary<string, int>();

            for (int i = 0; i < uniformCount; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        private int CompileShader(ShaderType type, string path)
        {
            string source;
            if (type == ShaderType.VertexShader)
            {
                source = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                layout(location = 1) in vec3 aColor;
                
                out vec3 vertexColor;
                
                uniform mat4 model;
                uniform mat4 view;
                uniform mat4 projection;
                
                void main()
                {
                    gl_Position = projection * view * model * vec4(aPosition, 1.0);
                    vertexColor = aColor;
                }";
            }
            else // Fragmento
            {
                source = @"
                #version 330 core
                in vec3 vertexColor;
                out vec4 FragColor;
                
                void main()
                {
                    FragColor = vec4(vertexColor, 1.0);
                }";
            }

            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            // Verificar errores de compilación
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"ERROR: Error al compilar shader {type}: {infoLog}");
            }

            return shader;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], false, ref matrix);
        }
    }
}