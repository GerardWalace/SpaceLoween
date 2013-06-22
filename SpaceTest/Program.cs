using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SpaceTest
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

        #endregion //Operators
    }
    [Serializable]
    public class SpForce : SpVector
    {
        /// <summary>
        /// Newton Force
        /// </summary>
        public Double F { get; set; }

        #region Constructors

        public SpForce(SpVector vector = default(SpVector), Double force = default(Double))
            : base(vector ?? new SpVector())
        {
            F = force;
        }

        public SpForce(SpForce copy)
            : this(
            copy as SpVector,
            copy != null ? copy.F : default(Double)) { }

        public SpForce()
            : this(null) { }

        #endregion //Constructors

        #region Operators

        public override bool Equals(object obj)
        {
            if (obj == this) // Start with a ReferenceEquals Test
                return true;
            else if (obj is SpForce)
                return Equals(obj as SpForce);
            else
                return false;
        }

        protected bool Equals(SpForce obj)
        {
            if (obj == null)
                return false;
            else
                return this.F.Equals(obj.F)
                    && this.Equals(obj as SpVector);
        }

        #endregion //Operators
    }
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

        public List<SpForce> Forces { get; private set; }

        #region Constructors

        public SpObject(
            Double m = default(Double),
            SpVector p = default(SpVector),
            SpVector s = default(SpVector),
            SpVector a = default(SpVector),
            IEnumerable<SpForce> fs = null)
        {
            Forces = new List<SpForce>(fs ?? new List<SpForce>());
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
            copy != null ? from f in copy.Forces select new SpForce(f) : null) { }

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
