using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelData
    {
        public List<Vertex> Vertices { get; }
        public List<UvCoordinate> UvCoordinates { get; }
        public List<Face> Faces { get; }

        public ModelData(List<Vertex> vertices, List<UvCoordinate> uvCoordinates, List<Face> faces)
        {
            Vertices = vertices;
            UvCoordinates = uvCoordinates;
            Faces = faces;
        }
    }
}