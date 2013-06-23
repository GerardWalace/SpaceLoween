using System;

namespace SpaceTest.SpFrwk
{
    [Serializable]
    public class SpVector
    {
        /// <summary>
        /// Coord X
        /// </summary>
        public Double X { get; set; }
        /// <summary>
        /// Coord Y
        /// </summary>
        public Double Y { get; set; }
        /// <summary>
        /// Coord Z
        /// </summary>
        public Double Z { get; set; }
        /// <summary>
        /// Length (pwd2)
        /// </summary>
        public Double Length2 { get { return X * X + Y * Y + Z * Z; } }
        /// <summary>
        /// Length (sqrt)
        /// </summary>
        public Double Length { get { return Math.Sqrt(Length2); } }

        #region Constructors

        public SpVector(
            Double x = default(Double),
            Double y = default(Double),
            Double z = default(Double))
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SpVector(SpVector copy)
            : this(
            copy != null ? copy.X : default(Double),
            copy != null ? copy.Y : default(Double),
            copy != null ? copy.Z : default(Double)) { }

        public SpVector()
            : this(null) { }


        #endregion //Constructors

        #region Operators

        public override bool Equals(object obj)
        {
            if (obj == this) // Start with a ReferenceEquals Test
                return true;
            else if (obj is SpVector)
                return Equals(obj as SpVector);
            else
                return false;
        }

        protected bool Equals(SpVector obj)
        {
            if (obj == null)
                return false;
            else
                return this.X.Equals(obj.X)
                    && this.Y.Equals(obj.Y)
                    && this.Z.Equals(obj.Z);
        }

        public static SpVector operator *(Double coef, SpVector v)
        {
            return v * coef;
        }

        public static SpVector operator *(SpVector v, Double coef)
        {
            if (v == null)
                return null;
            else
                return new SpVector(
                    v.X * coef,
                    v.Y * coef,
                    v.Z * coef);
        }

        public static SpVector operator /(Double coef, SpVector v)
        {
            return v / coef;
        }

        public static SpVector operator /(SpVector v, Double div)
        {
            if (v == null)
                return null;
            else
                return new SpVector(
                    v.X / div,
                    v.Y / div,
                    v.Z / div);
        }

        public static SpVector operator -(SpVector v)
        {
            return v * -1;
        }

        public static SpVector operator +(SpVector v, SpVector v2)
        {
            if (v == null || v2 == null)
                return v ?? v2 ?? null;
            else
                return new SpVector(
                    v.X + v2.X,
                    v.Y + v2.Y,
                    v.Z + v2.Z);
        }

        public static SpVector operator -(SpVector v, SpVector v2)
        {
            return v + (-v2);
        }

        #endregion //Operators

        #region Functions
        public SpVector Normalize()
        {
            return this / Length;
        }
        #endregion //Functions

        #region ToString
        public override string ToString()
        {
            return String.Format("{0:G3} ({1:G1}/{2:G1}/{3:G1})", Length, X, Y, Z);
        }
        #endregion //ToString
    }
}
