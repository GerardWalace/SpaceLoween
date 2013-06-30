using System;
using System.Collections.Generic;
using System.Linq;
using SpaceTest.SpFrwk;

namespace SpaceTest
{
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
            : this(null as SpItem) { }

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
            return String.Format("{0} : {1}", Name, base.ToString());
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello mono !");
        }
    }
}
