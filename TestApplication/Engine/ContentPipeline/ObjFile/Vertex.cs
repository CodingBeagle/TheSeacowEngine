using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    /// <summary>
    /// 
    /// </summary>
    public class Vertex
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<float> Range()
        {
            return new List<float>() {X, Y, Z};
        }
    }
}