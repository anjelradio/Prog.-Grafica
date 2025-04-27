using ClickableTransparentOverlay;
using ImGuiNET;

using U_3d.Clases;
using OpenTK.Mathematics;

namespace U_3d
{
    public class Interfaz : Overlay
    {
        public Escenario? Escenario { get; set; }
        private readonly Dictionary<string, Vector3> rotaciones = new();
        private readonly Dictionary<string, Vector3> traslaciones = new();
        private readonly Dictionary<string, Vector3> posicionesOriginales = new();

        // Valor para traslación con ajuste fino
        private const float AJUSTE_FINO = 0.1f;

        protected override void Render()
        {
            ImGui.Begin("Control del Escenario");

            if (Escenario != null && ImGui.BeginTabBar("PestañasTransformacion"))
            {
                // PESTAÑA DE ROTACIÓN
                if (ImGui.BeginTabItem("Rotación"))
                {
                    MostrarPanelRotacion();
                    ImGui.EndTabItem();
                }

                // PESTAÑA DE TRASLACIÓN
                if (ImGui.BeginTabItem("Traslación"))
                {
                    
                    ImGui.EndTabItem();
                }

                // PESTAÑA DE ESCALADO (pendiente de implementar)
                if (ImGui.BeginTabItem("Escalado"))
                {
                    ImGui.Text("Aquí irá el panel de escalado.");
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
            else if (Escenario == null)
            {
                ImGui.Text("No hay escenario cargado.");
            }

            ImGui.End();
        }

        private void MostrarPanelRotacion()
        {
            // ROTAR TODO EL ESCENARIO
            MostrarControlesRotacionTiempoReal("ESCENARIO_COMPLETO", "Todo el Escenario", RotarTodoElEscenario);

            // ROTAR OBJETOS, PARTES Y CARAS
            foreach (var kvpObj in Escenario.Objetos)
            {
                string nombreObj = kvpObj.Key;
                var objeto = kvpObj.Value;

                if (ImGui.TreeNode(nombreObj))
                {
                    // Rotar objeto
                    MostrarControlesRotacionTiempoReal(nombreObj, $"Objeto {nombreObj}",
                        (rotX, rotY, rotZ) => AplicarRotacionAbsoluta(objeto, nombreObj, rotX, rotY, rotZ));

                    foreach (var kvpParte in objeto.Partes)
                    {
                        string nombreParte = kvpParte.Key;
                        var parte = kvpParte.Value;
                        string idParte = $"{nombreObj}.{nombreParte}";

                        if (ImGui.TreeNode(nombreParte))
                        {
                            // Controles de rotación para la parte
                            MostrarControlesRotacionTiempoReal(idParte, $"Parte {nombreParte}",
                                (rotX, rotY, rotZ) => AplicarRotacionAbsoluta(parte, idParte, rotX, rotY, rotZ));

                            if (ImGui.TreeNode("Caras"))
                            {
                                foreach (var kvpCara in parte.Caras)
                                {
                                    string nombreCara = kvpCara.Key;
                                    var cara = kvpCara.Value;
                                    string idCara = $"{idParte}.{nombreCara}";

                                    MostrarControlesRotacionTiempoReal(idCara, $"Cara {nombreCara}",
                                        (rotX, rotY, rotZ) => AplicarRotacionAbsoluta(cara, idCara, rotX, rotY, rotZ));
                                }

                                ImGui.TreePop();
                            }

                            ImGui.TreePop();
                        }
                    }

                    ImGui.TreePop();
                }
            }
        }

        private void MostrarControlesRotacionTiempoReal(string id, string titulo, Action<float, float, float> aplicarRotacion)
        {
            if (!rotaciones.ContainsKey(id))
            {
                rotaciones[id] = new Vector3(0, 0, 0);
            }

            ImGui.Separator();
            ImGui.Text(titulo);

            var rot = rotaciones[id];
            bool cambioX = ImGui.SliderFloat($"Rot X##{id}", ref rot.X, -180f, 180f);
            bool cambioY = ImGui.SliderFloat($"Rot Y##{id}", ref rot.Y, -180f, 180f);
            bool cambioZ = ImGui.SliderFloat($"Rot Z##{id}", ref rot.Z, -180f, 180f);

            rotaciones[id] = rot;

            if (cambioX || cambioY || cambioZ)
            {
                try
                {
                    aplicarRotacion(rot.X, rot.Y, rot.Z);
                }
                catch (Exception ex)
                {
                    // Restaurar valores anteriores en caso de error
                    rotaciones[id] = new Vector3(0, 0, 0);
                    Console.WriteLine($"Error al rotar: {ex.Message}");
                }
            }

            if (ImGui.Button($"Reiniciar Rotación##{id}"))
            {
                rotaciones[id] = new Vector3(0, 0, 0);
                aplicarRotacion(0, 0, 0);
            }
        }

        private void RotarTodoElEscenario(float rotX, float rotY, float rotZ)
        {
            if (Escenario == null)
                return;

            foreach (var objeto in Escenario.Objetos.Values)
            {
                objeto.RestablecerTransformaciones();
            }

            if (rotX != 0 || rotY != 0 || rotZ != 0)
            {
                Utilidades.Rotar(Escenario, rotX, rotY, rotZ);
            }
        }

        private void AplicarRotacionAbsoluta<T>(T elemento, string id, float rotX, float rotY, float rotZ) where T : ITransformable
        {
            elemento.RestablecerTransformaciones();

            if (rotX != 0 || rotY != 0 || rotZ != 0)
            {
                Utilidades.Rotar(elemento, rotX, rotY, rotZ);
            }
        }



        
    }
}