using System.Collections.Generic;
using GlmSharp;

namespace TestApplication.Engine
{
    public class Renderer
    {
        public List<Mesh> WorldMeshes { get; set; }

        public Renderer()
        {
            WorldMeshes = new List<Mesh>();
        }

        public void Render(float state)
        {
            // Create perspective matrix
            mat4 perspectiveMatrix = mat4.PerspectiveFov(
                glm.Radians(45.0f),
                800.0f,
                600.0f,
                0.1f,
                500.0f);

            // Create view matrix
            mat4 viewMatrix = mat4.LookAt(
                new vec3(0.0f, 0.0f, 3.0f),
                vec3.Zero,
                vec3.UnitY);

            foreach (Mesh worldMesh in WorldMeshes)
            {
                worldMesh.Activate();
                worldMesh.Draw(perspectiveMatrix, viewMatrix, state);
                worldMesh.DeActivate();
            }
        }
    }
}