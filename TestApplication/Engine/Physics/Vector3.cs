using System;

namespace TestApplication.Engine.Physics
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Inver
        /// </summary>
        public void Invert()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// To normalize a vector essentially means converting a vector
        /// To a Unit Vector, that is, a vector of magnitude 1.
        /// 
        /// Many algorithms and calculations depend on normalized
        /// Vectors.
        /// </summary>
        public void Normalize()
        {
            float magnitude = Magnitude();

            if (magnitude > 1.0f)
            {
                Vector3 normalizedVector = this * (1.0f / magnitude);

                X = normalizedVector.X;
                Y = normalizedVector.Y;
                Z = normalizedVector.Z;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scale"></param>
        public void AddScaledVector(Vector3 vector, float scale)
        {
            X += vector.X * scale;
            Y += vector.Y * scale;
            Z += vector.Z * scale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        public void ComponentProductUpdate(Vector3 vector)
        {
            X *= vector.X;
            Y *= vector.Y;
            Z *= vector.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector3 ComponentProduct(Vector3 vector)
        {
            return new Vector3(X * vector.X, Y * vector.Y, Z * vector.Z);
        }

        /// <summary>
        /// The "Cross Product" is also known as "Vector Product".
        /// 
        /// The cross product is *NOT* commutative!
        /// 
        /// The geometrical interpretation of the cross product depends on the
        /// Vectors given!
        /// 
        /// For a pair of vectors, A being normalized and B NOT being normalized,
        /// The magnitude of the resulting vector represents the components of B
        /// That is *NOT* in the direction of A.
        /// 
        /// However, the cross product is mostly important because of its direction,
        /// Not magnitude.
        /// 
        /// In three dimensions, the vector product will point in a direction that
        /// Is at right angles to both of its operands. This is convinient for
        /// Several algorithms.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector3 CrossProduct(Vector3 vector)
        {
            return new Vector3(
                Y * vector.Z - Z * vector.Y,
                Z * vector.X - X * vector.Z,
                X * vector.Y - Y * vector.X
                );
        }

        /// <summary>
        /// Cross product update
        /// </summary>
        /// <param name="vector"></param>
        public void CrossProductUpdate(Vector3 vector)
        {
            Vector3 crossProduct = CrossProduct(vector);

            X = crossProduct.X;
            Y = crossProduct.Y;
            Z = crossProduct.Z;
        }

        /// <summary>
        /// The "Scalar Product" is by far the most common product of two vectors when dealing with
        /// Computer physics and computer graphics.
        /// 
        /// The scalar product is also called the "dot product" or "inner product".
        /// 
        /// The scalar product does not result in a vector, like many other vector operations!
        /// Instead, it results in a scalar value, hence its name!
        /// 
        /// The scalar product has some important use cases! For example, you can use it
        /// To figure how information of two vectors relative to each other. The scalar product
        /// In and by itself can be used to calculate the magnitude of one vector in the direction
        /// Of another, that is, how much one vector points towards another!
        /// 
        /// Usually, one perform the scalar product with two normalized vectors. If one vector is
        /// Not normalized, the scalar product will be multiplied by this length!
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float ScalarProduct(Vector3 vector)
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"( {X}, {Y}, {Z} )";
        }

        #region Operator Overloads
        /// <summary>
        /// For vectors, multiplication of a vector by a scalar is defined as multiplying each component
        /// Of the vector with the scalar value.
        /// 
        /// Geometrically, multiplication of a vector by a scalar scales the length of the vector,
        /// While leaving the direction the same.
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 vectorA, float scalar)
        {
            return new Vector3(vectorA.X * scalar, vectorA.Y * scalar, vectorA.Z * scalar);
        }

        /// <summary>
        /// Vector addition is simply adding each corresponding component of each vector
        /// Together. I.E, Vector1.X + Vector2.X, Vector1.Y + Vector2.Y, etc...
        /// 
        /// Geometrically, adding two vectors together is the equivalent of placing them
        /// End to end. The resulting vector is the one going from the origin of the first
        /// Vector to the end of the second.
        /// 
        /// Geometrically,subtracting two vectors is the equivalent of placing
        /// The vectors so that their origins touch each other. The resulting
        /// Vector (in for example A + B), is the vector going from the end of B
        /// To the end of A in this arrangment.
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 vectorA, Vector3 vectorB)
        {
            return new Vector3(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z);
        }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 vectorA, Vector3 vectorB)
        {
            return new Vector3(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y, vectorA.Z - vectorB.Z);
        }

        public float this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                    case 2:
                        return this.Z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (key)
                {
                    case 0:
                        this.X = value;
                        break;
                    case 1:
                        this.Y = value;
                        break;
                    case 2:
                        this.Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion
    }
}