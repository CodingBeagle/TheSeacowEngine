using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    /// <summary>
    /// 
    /// </summary>
    public class FacePart
    {
        public int VertexIndex { get; }
        public int UvIndex { get; }

        public FacePart(int vertexIndex, int uvIndex)
        {
            VertexIndex = vertexIndex;
            UvIndex = uvIndex;
        }

        public override string ToString()
        {
            return $"{VertexIndex},{UvIndex}";
        }
    }
}