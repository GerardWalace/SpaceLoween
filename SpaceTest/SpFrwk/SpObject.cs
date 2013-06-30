using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceTest.SpFrwk
{
    [Serializable]
    public class SpObject
    {
        /// <summary>
        /// Masse
        /// </summary>
        public Double M { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public SpVector P { get; set; }
        /// <summary>
        /// Speed
        /// </summary>
        public SpVector S { get; set; }
        /// <summary>
        /// Acceleration
        /// </summary>
        public SpVector A { get; set; }

        public List<SpVector> Forces { get; private set; }

        #region Constructors

        public SpObject(
            Double m = default(Double),
            SpVector p = default(SpVector),
            SpVector s = default(SpVector),
            SpVector a = default(SpVector),
            IEnumerable<SpVector> fs = null)
        {
            Forces = new List<SpVector>(fs ?? new List<SpVector>());
            M = m;
            P = p ?? new SpVector();
            S = s ?? new SpVector();
            A = a ?? new SpVector();
        }

        public SpObject(SpObject copy)
            : this(
            copy != null ? copy.M : default(Double),
            copy != null ? copy.P : default(SpVector),
            copy != null ? copy.S : default(SpVector),
            copy != null ? copy.A : default(SpVector),
            copy != null ? from f in copy.Forces select new SpVector(f) : null) { }

        public SpObject()
            : this(null) { }

        #endregion //Constructors

        #region Operators

        public override bool Equals(object obj)
        {
            if (obj == this) // Start with a ReferenceEquals Test
                return true;
            else if (obj is SpObject)
                return Equals(obj as SpObject);
            else
                return false;
        }

        protected bool Equals(SpObject obj)
        {
            if (obj == null)
                return false;
            else
                return this.M.Equals(obj.M)
                    && this.P.Equals(obj.P)
                    && this.S.Equals(obj.S)
                    && this.A.Equals(obj.A)
                    && this.Forces.SequenceEqual(obj.Forces);
        }

        #endregion //Operators

        #region Functions
        public virtual void Run() { }
        public virtual void Move()
        {
            // On calcule l'accelerations du tour selon la loi "somme des forces = M * a"
            CalulateAcceleration();
            // On applique le mouvement
            this.S += this.A;
            this.P += this.S;
        }
        public virtual void CalulateAcceleration()
        {
            // On calcule l'accelerations du tour selon la loi "somme des forces = M * a"
            SpVector sumF = new SpVector();
            foreach (var f in Forces)
                sumF += f;

            this.A = sumF / M;
        }
        #endregion // Functions

        #region ToString
        public override string ToString()
        {
            return String.Format("Pos={0}", P);
        }
        #endregion //ToString
    }
}
