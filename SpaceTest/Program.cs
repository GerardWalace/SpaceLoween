using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SpaceTest
{
    public static class AlmostTests
    {
        public static bool AlmostEquals(this Double v, Double v2, Double approx)
        {
            return Math.Abs(v2 - v) <= Math.Abs(approx);
        }

        public static bool AlmostEquals(this SpVector v, SpVector v2, SpVector approx)
        {
            return (v2 - v).Length2 <= approx.Length2;
        }

        public static Double AlmostRatio(this Double v, Double v2)
        {
            return Math.Abs((v2 - v) / v);
        }

        public static Double AlmostRatio(this SpVector v, SpVector v2)
        {
            return (v2 - v).Length2 / v.Length2;
        }
    }

    public static class SpConst
    {
        /// <summary>
        /// Constante Gravitationnelle = 6.67384 * 10^-11 m3 kg-1 s-2
        /// => 6.67384 * 10^-20 km3 kg-1 s-2
        /// </summary>
        public const Double G = 6.67384E-20;
    }

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
    //[Serializable]
    //public class SpForce : SpVector
    //{
    //    /// <summary>
    //    /// Newton Force
    //    /// </summary>
    //    public Double F { get; set; }

    //    #region Constructors

    //    public SpForce(SpVector vector = default(SpVector), Double force = default(Double))
    //        : base(vector ?? new SpVector())
    //    {
    //        F = force;
    //    }

    //    public SpForce(SpForce copy)
    //        : this(
    //        copy as SpVector,
    //        copy != null ? copy.F : default(Double)) { }

    //    public SpForce()
    //        : this(null) { }

    //    #endregion //Constructors

    //    #region Operators

    //    public override bool Equals(object obj)
    //    {
    //        if (obj == this) // Start with a ReferenceEquals Test
    //            return true;
    //        else if (obj is SpForce)
    //            return Equals(obj as SpForce);
    //        else
    //            return false;
    //    }

    //    protected bool Equals(SpForce obj)
    //    {
    //        if (obj == null)
    //            return false;
    //        else
    //            return this.F.Equals(obj.F)
    //                && this.Equals(obj as SpVector);
    //    }

    //    public static SpForce operator -(SpForce f)
    //    {
    //        return new SpForce(-(f as SpVector), f.F);
    //    }

    //    #endregion //Operators
    //}
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
    [Serializable]
    public class SpItem : SpObject
    {
        /// <summary>
        /// Name (Id)
        /// </summary>
        public String Name { get; set; }

        #region Constructors

        public SpItem(String name = default(String), SpObject coord = default(SpObject))
            : base(coord ?? new SpObject())
        {
            Name = name ?? String.Empty;
        }

        public SpItem(SpItem copy)
            : this(
            copy != null ? copy.Name : default(String),
            copy as SpObject) { }

        public SpItem()
            : this(null) { }

        #endregion //Constructors

        #region Operators

        public override bool Equals(object obj)
        {
            if (obj == this) // Start with a ReferenceEquals Test
                return true;
            else if (obj is SpItem)
                return Equals(obj as SpItem);
            else
                return false;
        }

        protected bool Equals(SpItem obj)
        {
            if (obj == null)
                return false;
            else
                return this.Name.Equals(obj.Name)
                    && this.Equals(obj as SpObject);
        }

        #endregion //Operators

        #region ToString
        public override string ToString()
        {
            return String.Format("{0} : {1}", Name, P);
        }
        #endregion //ToString
    }


    [Serializable]
    public class SpUnivers : SpItem
    {
        #region Constructors

        public SpUnivers(SpItem item = default(SpItem))
            : base(item ?? new SpItem())
        {
        }

        public SpUnivers(SpUnivers copy)
            : this(
            copy as SpItem) { }

        public SpUnivers()
            : this(null) { }

        #endregion //Constructors

        #region Operators

        public override bool Equals(object obj)
        {
            if (obj == this) // Start with a ReferenceEquals Test
                return true;
            else if (obj is SpUnivers)
                return Equals(obj as SpUnivers);
            else
                return false;
        }

        protected bool Equals(SpUnivers obj)
        {
            throw new NotImplementedException("TOTEST");
            if (obj == null)
                return false;
            else
                return this.Equals(obj as SpItem)
                    && m_Items.Count == obj.m_Items.Count
                    && m_Items.SequenceEqual(obj.m_Items);
        }

        #endregion //Operators

        #region Members
        private List<SpObject> m_Items = new List<SpObject>();
        #endregion //Members


        #region Functions
        public void AddItem(SpObject obj)
        {
            if (m_Items.Contains(obj) == false)
            {
                this.S += obj.S * (this.M != 0.0 ? obj.M / this.M : 1.0);
                this.P += obj.P * (this.M != 0.0 ? obj.M / this.M : 1.0);
                this.M += obj.M;
                m_Items.Add(obj);
            }
        }

        public override void Run()
        {
            // On remet a zero les forces
            m_Items.ForEach(i => i.Forces.Clear());
            // On calcule les forces de gravitations que chacun des objets exercent les uns sur les autres
            for (int i = 0; i < m_Items.Count; i++)
                for (int j = i + 1; j < m_Items.Count; j++)
                    AddRespectiveGravitationalForces(m_Items[i], m_Items[j]);
            // On lance les mouvements internes
            m_Items.ForEach(i => i.Run());
            // On applique le mouvement
            m_Items.ForEach(i => i.Move());
        }

        private void AddRespectiveGravitationalForces(SpObject o1, SpObject o2)
        {
            SpVector fo1o2 = (o2.P - o1.P).Normalize() * SpConst.G * o1.M * o2.M / (o2.P - o1.P).Length2;

            SpVector fo2o1 = -fo1o2;
            o1.Forces.Add(fo1o2);
            o2.Forces.Add(fo2o1);
        }
        #endregion //Functions
    }

    public class SpaceSerializer<T> where T : class, new()
    {
        private XmlSerializer m_serializer = new XmlSerializer(typeof(T));

        public void Serialize(Stream stream, T o)
        {
            m_serializer.Serialize(stream, o);
        }

        public T Deserialize(Stream stream)
        {
            return m_serializer.Deserialize(stream) as T;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
