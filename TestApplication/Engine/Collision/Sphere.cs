using TestApplication.Engine.Physics;

namespace TestApplication.Engine.Collision
{
    /// <summary>
    /// The sphere is a very common bounding volume, rivaling the Axis-Aligned Bounding Box (AABB) in popularity.
    /// 
    /// Spheres have inexpensive intersection tests.
    /// Spheres are also rotationally invariant, which means
    /// That they are trivial to transform, because all you
    /// Have to do is give them new positions.
    /// 
    /// Spheres are very memory efficient bounding volumes, as
    /// They can be defined by four components in total.
    /// </summary>
    public class Sphere
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}