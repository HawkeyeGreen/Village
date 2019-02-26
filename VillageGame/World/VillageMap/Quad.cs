using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using Village.VillageGame.World.ReactionSystem;
using Village.VillageGame.DatabaseManagement;
using Zeus.Hermes;
using System.Data;

namespace Village.VillageGame.World.VillageMap
{
    /// <summary>
    /// Ein Teil der Welt.
    /// </summary>
    public class Quad : HermesLoggable
    {
        private string DBString;
        private static int counter = 0;
        private int length;
        private int _ID;

        private List<Trigger> triggers;

        // Die Substanzen innerhalb des Raumes dieses Quads
        private Phase phase;

        // Die Position im Welt-Koordinatensystem
        private Vector3 absolutePosition;

        // Die Position im Parentquad
        private Vector3 relativePosition;

        /// <summary>
        /// Die Position der linken hinteren Ecke 
        /// </summary>
        public Vector3 AbsolutePosition
        {
            get => absolutePosition;
            set => absolutePosition = value;
        }

        /// <summary>
        /// Die Position dieses Quads relativ zur Position des ParentQuads.
        /// </summary>
        public Vector3 RelativePosition
        {
            get => relativePosition;
            set => relativePosition = value;
        }

        /// <summary>
        /// Die Phase, die sich innerhalb dieses Quads befindet
        /// </summary>
        public Phase Phase
        {
            get => phase;
            set => phase = value;
        }

        public int Length
        {
            get => length;
            set => length = value;
        }

        public long ID => _ID;

        public string Type => "Quad";

        public string DBConnectionString
        {
            get => DBString;
            set => DBString = value;
        }

        /// <summary>
        /// Die Quads, welche diesem Quad untergeordnet sind.
        /// Null, wenn es sich um ein primitive Quad handelt.
        /// </summary>
        private Quad[,,] subQuads;

        /// <summary>
        /// Das Quad, zu dem dieses gehört.
        /// Null, wenn das Quad die derzeit oberste Wurzel ist.
        /// </summary>
        private Quad parentQuad;

        /// <summary>
        /// Initialisiert dieses Quad mit seinen wichtigsten Feldern.
        /// Dieses Quad ist NOCH NICHT nutzbar!
        /// Es muss noch noch mit weiteren Feldern versorgt werden via:
        /// SeThisQuadUp, Load oder LoadFrom.
        /// </summary>
        /// <param name="ID">Die ID-Kennung dieses Quads.</param>
        /// <param name="dbKey">Die MainDB dieses Quads.</param>
        public Quad(int ID, string dbKey)
        {
            _ID = ID;
            DBString = dbKey;
        }

        /// <summary>
        /// Als ID wird hier der globale Quad-Counter genutzt.
        /// </summary>
        /// <param name="dbKey">Aktuelle Hauptdatenbank dieses Quads.</param>
        public Quad(string dbKey)
        {
            DBString = dbKey;
            _ID = counter;
            counter++;
            Hermes.getInstance().log(this, "A Quad (No: " + counter + ") was generated. ");
        }


        public void SetThisQuadUp(Vector3 position, int lngth)
        {
            subQuads = null;
            parentQuad = null;
            absolutePosition = position;
            length = lngth;
            if(length == 2)
            {
                for(int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Quad subQuad = new Quad(DBString);
                            subQuad.SetThisQuadUp(new Vector3(absolutePosition.X + x, absolutePosition.Y + y, absolutePosition.Z + z),1);
                            AddQuad(subQuad);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fügt das Quad an der angebenen Stelle hinzu.
        /// Eventuell vorhandene Quads werden überschrieben!
        /// </summary>
        /// <param name="quad">Das neue Quad</param>
        /// <param name="position">Die Position des Quads in der Welt.</param>
        public void AddQuad(Quad quad, Vector3 position)
        {
            if (subQuads == null)
            {
                subQuads = new Quad[2, 2, 2];
            }
            subQuads[Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(position.Z)] = quad;
            quad.parentQuad = this;
            quad.relativePosition = position;
        }

        public void AddQuad(Quad q)
        {
            Vector3 rel = q.absolutePosition - absolutePosition;
            int x = Convert.ToInt32(rel.X);
            int y = Convert.ToInt32(rel.Y);
            int z = Convert.ToInt32(rel.Z);
            AddQuad(q, new Vector3(x, y, z));
        }

        public Quad GetSubQuad(Vector3 relativPosition)
        {
            return subQuads[Convert.ToInt32(relativPosition.X), Convert.ToInt32(relativPosition.Y), Convert.ToInt32(relativPosition.Z)];
        }

        public Quad GetQuad(Vector3 absPosition)
        {
            if (length == 1)
            {
                if (AbsolutePosition == absPosition)
                {
                    Hermes.getInstance().log(this, "I am this Quad!" + this.ToString());
                    return this;
                }
                else
                {
                    Hermes.getInstance().log(this, "Nach mir nur die Flut... " + this.ToString());
                    return null;
                }
            }
            else
            {
                Hermes.getInstance().log(this, "I am NOT this Quad!" + this.ToString());
                foreach (Quad q in subQuads)
                {
                    Quad quad = q.GetQuad(absPosition);
                    if (quad != null)
                    {
                        return quad;
                    }

                }
                return null;
            }
        }

        public override string ToString()
        {
            return "Quad: " + AbsolutePosition.ToString() + "; Length: " + length;
        }

        /// <summary>
        /// Speichert das Quad in seine momentane MainDB.
        /// </summary>
        public void Save(int parentId)
        {
            SaveTo(DBString, parentId);
        }

        /// <summary>
        /// Lädt das Quad mit dieser ID aus seiner MainDB.
        /// </summary>
        public void Load()
        {
            LoadFrom(DBString);
        }

        /// <summary>
        /// Speichert dieses Quad in die angegebene DB.
        /// </summary>
        /// <param name="DBKey">Der Schlüssel zur DB.</param>
        public void SaveTo(string DBKey, int parentID)
        {
            if (length == 1)
            {
                DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO Quads(id, x, y, z, l, [parentQuad]) VALUES(" +
                            ID.ToString() + "," +
                            AbsolutePosition.X.ToString() + "," +
                            AbsolutePosition.Y.ToString() + "," +
                            AbsolutePosition.Z.ToString() + "," +
                            Length.ToString() + "," +
                            parentID.ToString() + ");"
                            , DBKey);
            }
            else if (length == 2)
            {
                DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO Quads(id, x, y, z, l, [parentChunk]) VALUES(" +
                            ID.ToString() + "," +
                            AbsolutePosition.X.ToString() + "," +
                            AbsolutePosition.Y.ToString() + "," +
                            AbsolutePosition.Z.ToString() + "," +
                            Length.ToString() + "," +
                            parentID.ToString() + ");"
                            , DBKey);
                if (subQuads != null)
                {
                    for (int x = 0; x < subQuads.GetLength(0); x++)
                    {
                        for (int y = 0; y < subQuads.GetLength(1); y++)
                        {
                            for (int z = 0; z < subQuads.GetLength(2); z++)
                            {
                                if(subQuads[x, y, z] != null)
                                {
                                    subQuads[x, y, z].Save(_ID);
                                }
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Lädt das Quad mit dieser ID von der angebenen DB.
        /// </summary>
        /// <param name="DBKey">Der Schlüssel zur DB.</param>
        public void LoadFrom(string DBKey)
        {
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM Quads WHERE id=" + _ID.ToString() + ";", DBKey).CreateDataReader();
            reader.Read();
            absolutePosition = new Vector3(reader.GetInt32(reader.GetOrdinal("x")), reader.GetInt32(reader.GetOrdinal("y")), reader.GetInt32(reader.GetOrdinal("z")));
            length = reader.GetInt32(reader.GetOrdinal("l"));
            if(length == 2)
            {
                List<int> qIDs = new List<int>();
                while (reader.Read())
                {
                    qIDs.Add(reader.GetInt32(reader.GetOrdinal("id")));
                }

                foreach (int qid in qIDs)
                {
                    Quad quad = new Quad(qid, DBString);
                    quad.Load();
                    AddQuad(quad);
                }
            }
        }

        public virtual List<Tuple<Substance, int>> RemovePhase(int amount)
        {
            OnRemove();
            throw new NotImplementedException();
        }

        public virtual void OnRemove()
        {
            foreach (Trigger trigger in triggers)
            {
                if (trigger.TriggerOn.Contains(TriggerKeys.OnRemove))
                {
                    trigger.trigger();
                }
            }
        }
    }
}
