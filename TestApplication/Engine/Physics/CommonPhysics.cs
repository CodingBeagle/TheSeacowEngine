using System.Collections.Generic;

namespace TestApplication.Engine.Physics
{
    public static class CommonPhysics
    {
        private static readonly List<Particle> ActiveParticles = new List<Particle>();

        public static Particle CreateParticle(Mesh mesh)
        {
            Particle newParticle = new Particle {LinkedMesh = mesh};

            return newParticle;
        }

        public static void AddParticleToWorld(Particle particle)
        {
            ActiveParticles.Add(particle);
        }

        public static void RemoveParticleFromWorld(Particle particle)
        {
            ActiveParticles.Remove(particle);
        }

        public static void Integrate(float state)
        {
            foreach (Particle activeParticle in ActiveParticles)
            {
                activeParticle.Integrate(state);
            }
        }
    }
}