using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zeus.Hermes;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.World.VillageMap
{
    public class VillageChunk : HermesLoggable
    {
        private int Length;
        private int Width;
        private int Height;
        private string DBString;

        private readonly int _ID;
        private List<Quad[,]> quads;
        private List<Quad> renderedQuads;
        private Vector3 leftBackBottom;
        private Vector3 rightFrontTop;

        public long ID => _ID;
        public string Type => "MapChunk";

        public Vector3 BaseCorner => leftBackBottom;
        public Vector3 TopCorner => rightFrontTop;
        public string DBConnectionString
        {
            get => DBString;
            set => DBString = value;
        }

        /// <summary>
        /// Initialisiert die wichtigtsen Größen dieses Chunks.
        /// Dieses Chunk ist noch NICHT einsatzbereit.
        /// Es muss nun entweder noch via SetUp, Load oder LoadFrom mit Daten befüllt werden.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="dbString"></param>
        public VillageChunk(int ID, string dbString)
        {
            _ID = ID;
            DBString = dbString;
        }

        /// <summary>
        /// Setzte alle nötigen Größen auf,
        /// um dieses Chunk zu einem leeren Chunk zu machen.
        /// Dann kann es befüllt werden.
        /// </summary>
        /// <param name="corner">Die linke, hintere Ecke dieses Chunks.</param>
        /// <param name="length">Die Länge (x-Achse) des Chunks.</param>
        /// <param name="width">Die Breite (y-Achse) des Chunks.</param>
        /// <param name="height">Die Höhe entlang der z-Achse dieses Chunks.</param>
        public void SetThisChunkUp(Vector3 corner, int length, int width, int height = 2)
        {
            Length = length;
            Width = width;
            Height = height;
            leftBackBottom = corner;
            rightFrontTop = new Vector3(corner.X + length, corner.Y + width, corner.Z + height);
            quads = new List<Quad[,]>();
            Hermes.getInstance().log(this, "Ein Chunk an Position: " + BaseCorner + " wurde erstellt mit den Dimensionen [" + length + " | " + width + " | " + height + "]");
            for (int i = 0; i < height; i += 2)
            {
                Quad[,] level = new Quad[length, width];
                for (int x = 0; x < length; x += 2)
                {
                    for (int y = 0; y < width; y += 2)
                    {
                        Quad quad = new Quad(DBString);
                        quad.SetThisQuadUp(new Vector3(corner.X + x, corner.Y + y, corner.Z + i), 2);
                        level[x, y] = quad;
                    }
                }
                quads.Add(level);
            }

        }

        /// <summary>
        /// Fügt dem Chunk dieses quad hinzu.
        /// Es wird dem richtigen Setquad zugeordnet werden.
        /// </summary>
        /// <param name="quad">Füg.Mich.EIN.</param>
        public void AddQuad(Quad quad)
        {
            Hermes.getInstance().log(this, " Ein Quad wird hinzugefügt: " + quad, 0);
            Vector3 relativPosition = quad.AbsolutePosition - BaseCorner;
            int x = Convert.ToInt32(Math.Floor(relativPosition.X / 2.0));
            int y = Convert.ToInt32(Math.Floor(relativPosition.Y / 2.0));
            int z = Convert.ToInt32(Math.Floor(relativPosition.Z / 2.0));
            quads[z][x, y].AddQuad(quad);
            quad.RelativePosition = new Vector3(x, y, z);
        }

        public Quad GetQuad(Vector3 position)
        {

            Vector3 relativePosition = position - BaseCorner;
            int x = Convert.ToInt32(relativePosition.X) / 2;
            int y = Convert.ToInt32(relativePosition.Y) / 2;
            int z = Convert.ToInt32(relativePosition.Z) / 2;
            return quads[z][x, y].GetQuad(position);
        }

        public void Save()
        {
            SaveTo(DBString);
        }

        public void Load()
        {
            LoadFrom(DBString);
        }

        public void SaveTo(string dbKey)
        {
            DBHelper.ExecuteCommandNonQuery("INSERT OR REPLACE INTO Chunks(id, x, y, z, l, w, h) VALUES(" +
                ID.ToString() + "," +
                leftBackBottom.X.ToString() + "," +
                leftBackBottom.Y.ToString() + "," +
                leftBackBottom.Z.ToString() + "," +
                Length.ToString() + "," +
                Width.ToString() + "," +
                Height.ToString() + ");"
                , dbKey);
            for (int z = 0; z < quads.Count; z+=2)
            {
                for (int x = 0; x < quads[z].GetLength(0); x+=2)
                {
                    for (int y = 0; y < quads[z].GetLength(1); y+=2)
                    {
                        quads[z][x, y].Save(_ID);
                    }
                }
            }
        }

        public void LoadFrom(string dbKey)
        {
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM Chunks WHERE id=" + _ID.ToString() + ";", dbKey).CreateDataReader();
            reader.Read();
            int x = reader.GetInt32(reader.GetOrdinal("x"));
            int y = reader.GetInt32(reader.GetOrdinal("y"));
            int z = reader.GetInt32(reader.GetOrdinal("z"));
            leftBackBottom = new Vector3(x, y, z);
            Length = reader.GetInt32(reader.GetOrdinal("l"));
            Width = reader.GetInt32(reader.GetOrdinal("w"));
            Height = reader.GetInt32(reader.GetOrdinal("h"));
            rightFrontTop = new Vector3(leftBackBottom.X + Length * 2, leftBackBottom.Y + Width * 2, leftBackBottom.Z + Height * 2);
            reader.Close();
            quads = new List<Quad[,]>();
            for (int i = 0; i < Height; i++)
            {
                Quad[,] level = new Quad[Length, Width];
                quads.Add(level);
            }
            reader = DBHelper.ExecuteQuery("SELECT id FROM Quads WHERE parentChunk=" + _ID.ToString() + ";", dbKey).CreateDataReader();
            List<int> qIDs = new List<int>();
            while (reader.Read())
            {
                qIDs.Add(reader.GetInt32(reader.GetOrdinal("id")));
            }

            foreach (int qid in qIDs)
            {
                Quad quad = new Quad(qid, DBString);
                quad.Load();
                Vector3 relativPosition = quad.AbsolutePosition - BaseCorner;
                int qx = Convert.ToInt32(Math.Floor(relativPosition.X / 2.0));
                int qy = Convert.ToInt32(Math.Floor(relativPosition.Y / 2.0));
                int qz = Convert.ToInt32(Math.Floor(relativPosition.Z / 2.0));
                quads[qz][qx, qy] = quad;
                quad.RelativePosition = relativPosition;
            }
        }

        public override string ToString()
        {
            return "MapChunk Position: " + leftBackBottom + " | Diagonal: " + (rightFrontTop - leftBackBottom).Length().ToString();
        }
    }
}
