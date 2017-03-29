using System.Reflection;
using TestApplication.Engine.Physics;

namespace TestApplication.Engine.Collision
{
    public static class IntersectionDetector
    {
        public static Vector3 ClosestPointBetweenPointAndAabb(Vector3 point, Aabb aabb)
        {
            Vector3 calculatedVector = new Vector3();

            // For each coordinate axis, if the point coordinate value is
            // Outside box, clamp it to the boc, else keep it as is
            for (int i = 0; i < 3; i++)
            {
                float v = point[i];

                if (v < aabb.Min[i])
                    v = aabb.Min[i];

                if (v > aabb.Max[i])
                    v = aabb.Max[i];

                calculatedVector[i] = v;
            }

            return calculatedVector;
        }

        public static bool TestSphereAgainstAabb(Sphere sphere, Aabb aabb)
        {
            // Find point P on AABB closest to sphere cneter
            Vector3 closestPointOnAabb = ClosestPointBetweenPointAndAabb(sphere.Center, aabb);

            // Sphere and AABB intersect if the (squared) distance from sphere
            // Center to point p is less than the (squared) sphere radius
            Vector3 v = closestPointOnAabb - sphere.Center;

            return v.ScalarProduct(v) <= sphere.Radius * sphere.Radius;
        }
    }
}