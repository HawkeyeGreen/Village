using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Village.VillageGame.DatabaseManagement;


namespace Village.VillageGame.TagSystem
{
    /// <summary>
    /// Ein TagSet verwaltet die Tags eines smth. Ein smth kann stets nur eine Variante eines Tags aufweisen.
    /// Deswegen ein Set und keine Liste o.ä.
    /// KeyType referiert auf die Möglichkeit Tags sowohl an string-based verwaltete Smth als auch an
    /// ID-based verwaltete Smths zu hängen.
    /// </summary>
    class TagSet
    {
        private Dictionary<string, Tag> tags = new Dictionary<string, Tag>();

        /// <summary>
        /// Fügt den Tag diesem Set zu.
        /// Enthält das set einen Tag mit diesem Namen,
        /// so wird der enthaltene Tag ersetzt.
        /// </summary>
        /// <param name="tag">Dieser Tag soll hinzugefügt werden.</param>
        public void AddTag(Tag tag)
        {
            tags[tag.Name] = tag;
        }

        /// <summary>
        /// Erstellt einen Tag mit diesem Namen.
        /// </summary>
        /// <remarks>Das Add des Tags wird auf null gesetzt. Die Value auf -1.</remarks>
        /// <param name="name">Der Name des Tags.</param>
        public void AddTag(string name)
        {
            AddTag(new Tag(name));
        }

        /// <summary>
        /// Erstellt einen Tag mit diesem Namen und dieser Value.
        /// </summary>
        /// <remarks>Das Add des Tags wird auf null gesetzt.</remarks>
        /// <param name="name">Der Name des Tags.</param>
        /// <param name="value">Der Tag erhält diese Zahl.</param>
        public void AddTag(string name, int value)
        {
            AddTag(new Tag(name, value));
        }

        /// <summary>
        /// Erstellt einen Tag mit diesem Namen und hängt dieses Add dazu.
        /// </summary>
        /// <remarks>Die Value wird auf -1 gesetzt.</remarks>
        /// <param name="name">Der Name des Tags.</param>
        /// <param name="add">Der textliche Zusatz dieses Tags.</param>
        public void AddTag(string name, string add)
        {
            AddTag(new Tag(name, add));
        }

        /// <summary>
        /// Erstellt einen Tag mit diesem Namen und dieser Value.
        /// Zusätzlich ein Add angehängt.
        /// </summary>
        /// <param name="name">Der Name des Tags.</param>
        /// <param name="add">Der textliche Zusatz dieses Tags.</param>
        /// <param name="value">Der Tag erhält diese Zahl.</param>
        public void AddTag(string name, string add, int value)
        {
            AddTag(new Tag(name, add, value));
        }

        /// <summary>
        /// Überprüft, ob in diesem TagSet der angegebene Tag vorkommt.
        /// </summary>
        /// <param name="name">Der Name des Tags.</param>
        /// <returns></returns>
        public bool ContainsTag(string name)
        {
            return tags.ContainsKey(name);
        }

        /// <summary>
        /// Ruft den Add des angebenen Tags ab.
        /// Wenn der Tag nicht vorkommt, wird null zurückgegeben.
        /// </summary>
        /// <param name="name">Der Tag Name.</param>
        /// <returns>Den Add-Zusatz des Tags oder null.</returns>
        public string GetAdd(string name)
        {
            if (!ContainsTag(name)) { return null; }
            return tags[name].Add;
        }

        /// <summary>
        /// Ruft die Value des Tags ab. Gibt -1 zurück, wenn der Tag nicht gefunden wurde.
        /// </summary>
        /// <param name="name">Der Tag Name.</param>
        /// <returns>Value des Tags oder -1.</returns>
        public int GetValue(string name)
        {
            if (!ContainsTag(name)) { return -1; }
            return tags[name].Value;
        }

        /// <summary>
        /// Ruft sowohl Add als auch Value des angegebenen Tags ab und gibt sie als named-Tuple zurück.
        /// </summary>
        /// <param name="name">Name des Tags.</param>
        /// <returns>null und -1, wenn der Tag nicht gefunden wurde.</returns>
        public (string Add, int Value) GetAddAndValue(string name)
        {
            if (!ContainsTag(name)) { return (null, -1); }
            return (GetAdd(name), GetValue(name));
        }

        public void FillTagSet(string key, string DB)
        {
            tags = new Dictionary<string, Tag>();
            Fill(DBHelper.ExecuteQuery("SELECT [name], [add], [value] FROM Tags WHERE sKey='" + key + "';", DB).CreateDataReader());
        }

        public void FillTagSet(int key, string DB)
        {
            tags = new Dictionary<string, Tag>();
            Fill(DBHelper.ExecuteQuery("SELECT [name], [add], [value] FROM Tags WHERE iKey=" + key + ";", DB).CreateDataReader());
        }

        public void FillTagSet(string sKey, int iKey, string DB)
        {
            tags = new Dictionary<string, Tag>();
            Fill(DBHelper.ExecuteQuery("SELECT [name], [add], [value] FROM Tags WHERE iKey=" + iKey + " AND sKey='" + sKey + "';", DB).CreateDataReader());
        }

        private void Fill(DataTableReader reader)
        {
            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("name"));
                int value = Convert.ToInt32(reader.GetInt64(reader.GetOrdinal("value")));
                string add = reader.GetString(reader.GetOrdinal("add")); ;
                if (add == "NULL")
                {
                    add = null;
                }
                if (add == null && value == -1)
                {
                    AddTag(name);
                }
                else if (add == null && value != -1)
                {
                    AddTag(name, value);
                }
                else if (add != null && value == -1)
                {
                    AddTag(name, add);
                }
                else
                {
                    AddTag(name, add, value);
                }
            }
            reader.Close();
        }

        public void DumpTagSet(string key, string DB)
        {
            foreach(Tag tag in tags.Values)
            {
                string add = tag.Add;
                if(add == null)
                {
                    add = "NULL";
                }
                DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO TAGS([sKey], [name], [add], [value]) VALUES('" + key + "','" + tag.Name + "','" + add + "'," + tag.Value + ");", DB);
            }
        }

        public void DumpTagSet(int key, string DB)
        {
            foreach (Tag tag in tags.Values)
            {
                string add = tag.Add;
                if (add == null)
                {
                    add = "NULL";
                }
                DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO TAGS([iKey], [name], [add], [value]) VALUES(" + key + ",'" + tag.Name + "','" + add + "'," + tag.Value + ");", DB);
            }
        }

        public void DumpTagSet(string sKey, int iKey, string DB)
        {
            foreach (Tag tag in tags.Values)
            {
                string add = tag.Add;
                if (add == null)
                {
                    add = "NULL";
                }
                DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO TAGS([sKey], [iKey], [name], [add], [value]) VALUES('" + sKey +"'," + iKey + ",'" + tag.Name + "','" + add + "'," + tag.Value + ");", DB);
            }
        }

        public override string ToString()
        {
            string result = "[TAGSET: ";

            foreach (Tag tag in tags.Values)
            {
                result += tag + "  ";
            }

            result += "]";
            return result;
        }
    }
}
