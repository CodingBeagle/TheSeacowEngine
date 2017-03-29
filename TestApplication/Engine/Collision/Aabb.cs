using TestApplication.Engine.Physics;

namespace TestApplication.Engine.Collision
{
    /// <summary>
    /// The Axis-Aligned Bounding Box (AABB) is one of the most
    /// Common bounding volumes.
    /// 
    /// It is a rectangular six-sided box (in 3D) categorized by having
    /// Its faces oriented in such a way that its face normals are at all
    /// Times parallel with the axes of the given coordinate system.
    /// 
    /// The best feature of the AABB is its fast overlap check, which
    /// Simply involves direct comparison of individual coordinate values.
    /// </summary>
    public class Aabb
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public Aabb(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }
    }
}