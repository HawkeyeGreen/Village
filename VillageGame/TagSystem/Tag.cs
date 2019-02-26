using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.TagSystem
{
    /// <summary>
    /// A tag bound to smth. Gives the smth a specific trait.
    /// </summary>
    struct Tag
    {
        private int value;
        private string add;
        private string name;

        /// <summary>
        /// The name of the tag tagName[v,a].
        /// Is never <code>null</code>.
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// A numeric value bound to the Tag.
        /// -1 bedeutet unbelegt.
        /// </summary>
        public int Value
        {
            get => value;
            set => this.value = value;
        }

        /// <summary>
        /// Ein textlicher Zusatz für den Tag.
        /// </summary>
        public string Add
        {
            get => add;
            set => add = value;
        }

        /// <summary>
        /// Der volle Konstruktor für einen Tag.
        /// Belegt alle Felder mit einem Wert.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="v"></param>
        public Tag(string n, string a, int v)
        {
            name = n;
            add = a;
            value = v;
        }

        /// <summary>
        /// Initialisiert einen Tag, welcher ausschließlich einen numerischen Zusatz aufweist.
        /// </summary>
        /// <param name="n">Der Name des Tags.</param>
        /// <param name="v">Der numerische Wert.</param>
        public Tag(string n, int v)
        {
            name = n;
            value = v;
            add = null;
        }

        /// <summary>
        /// Belegt einen Tag mit Name und Addition.
        /// </summary>
        /// <param name="n">Name des Tags.</param>
        /// <param name="a">String-Addition.</param>
        public Tag(string n, string a)
        {
            name = n;
            add = a;
            value = -1;
        }

        /// <summary>
        /// Value und Add werden auf null-Werte gesetzt.
        /// </summary>
        /// <param name="n">Name des Tags.</param>
        public Tag(string n)
        {
            name = n;
            value = -1;
            add = null;
        }

        public override string ToString()
        {
            string result = "{TAG:name:" + name;
            if(add != null)
            {
                result += "|add:" + add;
            }
            if(value != -1)
            {
                result += "|value:" + value;
            }

            result += "}";

            return result;
        }
    }
}
