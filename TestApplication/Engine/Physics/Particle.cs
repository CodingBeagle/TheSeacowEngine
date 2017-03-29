using System;
using GlmSharp;

namespace TestApplication.Engine.Physics
{
    /// <summary>
    /// 
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// 
        /// </summary>
        public Particle()
        {
            Damping = 0.99f;
            SetMass(2.0f);

            Acceleration = new Vector3(0.0f, -15.0f, 0.0f);
            Position = new Vector3();
            Velocity = new Vector3();
            ForceAccum = new Vector3();
        }

        /// <summary>
        /// 
        /// </summary>
        public Mesh LinkedMesh { get; set; }

        /// <summary>
        /// Holds the linear postion of the particle in world space.
        /// 
        /// The linear position is used to calculate the particle's
        /// Linear velocity.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Holds the linear velocity of the particle in world space.
        /// 
        /// Linear velocity is the velocity of the object travelling in a
        /// Straight line.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Holds the acceleration of the particle.
        /// 
        /// This value can be used to set acceleration due to gravity or
        /// Any other constant acceleration.
        /// </summary>
        public Vector3 Acceleration { get; set; }

        /// <summary>
        /// Holds the amount of damping applied to linear motion.
        /// Damping is required to remove energy added through
        /// Numerical instability in the integrator.
        /// 
        /// If you don't want the objec to look like its experiencing
        /// drag/damping, then values near but less than 1 is optimal.
        /// For example 0.995
        /// </summary>
        public float Damping { get; set; }

        /// <summary>
        /// Inverse mass of the particle.
        /// 
        /// It is more useful to hold the inverse mass because
        /// Integration is simpler and because in real-time
        /// Simulation it is more useful to have objects with
        /// Infinite mass (immovable) than zero mass.
        /// (Completely unstable in numerical simulation)
        /// </summary>
        public float InverseMass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector3 ForceAccum { get; set; }

        /// <summary>
        /// Helper function to set inverse mass through mass
        /// </summary>
        /// <param name="mass">The mass you want the particle to have</param>
        public void SetMass(float mass)
        {
            InverseMass = 1 / mass;
        }

        public float GetMass()
        {
            if (Math.Abs(InverseMass) < 0.001f)
                return float.MaxValue;

            return 1.0f / InverseMass;
        }

        /// <summary>
        /// Integrates the particle forward in time by the given amount.
        /// This function uses a Newton-Euler integration method, which is a
        /// Linear approximation of the correct integral. For this reason it
        /// May be inaccurate in some cases.
        /// 
        /// We integrate, because we want to figure out what the next position
        /// Should be in time given the current velocity.
        /// 
        /// We also want to figure velocity based on current acceleration, with
        /// Some damping added to make the physics simulation more stable.
        /// </summary>
        /// <param name="timeDuration">Time since last frame in seconds</param>
        public void Integrate(float timeDuration)
        {
            // Zero out stuff
            ForceAccum = new Vector3();

            // Update linear position
            Position.AddScaledVector(Velocity, timeDuration);

            // Update velocity
            Vector3 resulintAcc = Acceleration;
            resulintAcc.AddScaledVector(ForceAccum, InverseMass);

            Velocity.AddScaledVector(resulintAcc, timeDuration);
            Velocity *= (float)Math.Pow(Damping, timeDuration);

            // Update linked mesh
            LinkedMesh.Position = new vec3(Position.X, Position.Y, Position.Z);
        }
    }
}