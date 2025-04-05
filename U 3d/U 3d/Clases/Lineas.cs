using OpenTK.Graphics.OpenGL;
namespace U_3d.Clases
{
    public class Lineas
    {
        public void ConfigurarLineas()
        {
            GL.Enable(EnableCap.LineSmooth);
            GL.LineWidth(2.0f);
        }
        public void DibujarLineas(int edgesLength)
        {
            GL.DrawElements(PrimitiveType.Lines, edgesLength, DrawElementsType.UnsignedInt, 0);
        }
    }
}