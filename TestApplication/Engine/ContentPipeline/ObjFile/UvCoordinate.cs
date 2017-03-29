using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    /// <summary>
    /// 
    /// </summary>
    public class UvCoordinate
    {
        public float U { get; }
        public float V { get; }    

        public UvCoordinate(float u, float v)
        {
            U = u;
            V = v;
        }

        public IEnumerable<float> Range()
        {
            return new List<float>() {U, V};
        }
    }
}