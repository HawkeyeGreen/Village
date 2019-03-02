using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Zeus.Hermes;
using System.Data;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.World.VillageMap
{
    /// <summary>
    /// Die Darstellung und Verwaltung der lokalen Map.
    /// </summary>
    public class VillageMap : HermesLoggable
    {
        private string dbString;
        private readonly string mapName;

        private Vector3 chunkDimensions;

        // Die ChunkMap enthält alle chunks und ihre IDs
        private List<List<List<int>>> chunkMap = new List<List<List<int>>>();

        // Diese Chunks werden immer simuliert, weil sich dort wichtige, signifikante Objekte aufhalten
        // Dennoch garantiert dies kein Rendering :P
        private List<int> activatedChunks = new List<int>();

        // Diese Chunks werden gerendert
        private List<VillageChunk> renderedChunks = new List<VillageChunk>();

        /// <summary>
        /// Der Name dieser Karte.
        /// </summary>
        public string Name => mapName;

        public string DBConnectionString
        {
            get => dbString;
            set => dbString = value;
        }

        public long ID => 1337;
        public string Type => "VillageMap";

        /// <summary>
        /// Lädt die Map oder initialisiert sie grundsätzlich.
        /// Wenn eine neue Map erstellt wird,
        /// dann muss sie über die entsprechenden Initial-Methoden noch weiter mit Werten
        /// befüllt werden, bevor sie voll einsatzfähig ist.
        /// </summary>
        /// <param name="dbFileName">Der Name der Datenbank (ohne Dateiendung!)</param>
        /// <param name="location">Der Speicherort relativ zum BasisOrdner des Spiels.</param>
        public VillageMap(string dbFileName, string location)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + location + dbFileName + ".sqlite"))
            {
                DBHelper.OpenConnection(dbFileName, location);
                dbString = dbFileName;
                Hermes.GetInstance().log(this, "Map-Database wurde gefunden.", 3);
                DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM Chunks", dbString).CreateDataReader();
                while (reader.Read())
                {
                    chunkDimensions = new Vector3(reader.GetInt32(reader.GetOrdinal("l")), reader.GetInt32(reader.GetOrdinal("w")), reader.GetInt32(reader.GetOrdinal("h")));
                    int cID = reader.GetInt32(reader.GetOrdinal("id"));
                    Vector3 position = new Vector3(reader.GetInt32(reader.GetOrdinal("x")), reader.GetInt32(reader.GetOrdinal("y")), reader.GetInt32(reader.GetOrdinal("z")));
                }
            }
            else
            {
                Hermes.GetInstance().log(this, "Map-Database wurde nicht gefunden. Lege an...", 3);
                string initialCMD =
                    "CREATE TABLE [Chunks] (" +
                    "[id] int not null primary key unique," +
                    "[x] int not null," +
                    "[y] int not null," +
                    "[z] int not null," +
                    "[l] int not null," +
                    "[w] int not null," +
                    "[h] int not null);" +
                    "CREATE TABLE [Quads] (" +
                    "[id] int not null primary key unique," +
                    "[x] int not null," +
                    "[y] int not null," +
                    "[z] int not null," +
                    "[l] int not null," +
                    "[parentQuad] int," +
                    "[parentChunk] int" +
                    ");";

                dbString = DBHelper.CreateConnectionAndDatabase(dbFileName, initialCMD, location);
                Hermes.GetInstance().log(this, "Map-Database wurde angelegt. Folgender dbString wurde zugewiesen: " + dbString, 3);
            }
        }

        /// <summary>
        /// Erzeugt die Boundaries der Map und bestimmt die Maße von Chunks innerhalb der Map.
        /// Alle Werte müssen durch zwei teilbar sein.
        /// </summary>
        /// <param name="x">Die Länge der X-Achse.</param>
        /// <param name="y">Die Länge der Y-Achse.</param>
        /// <param name="z">Die Länge der Z-Achse.</param>
        public void InitialMapDimensions(int x, int y, int z)
        {

            if (x % 2 != 0 || y % 2 != 0 || z % 2 != 0)
            {
                throw new ArgumentException();
            }

            int chunk_x = defineChunkDimension(x);
            int chunk_y = defineChunkDimension(y);
            int chunk_z = defineChunkDimension(z);

            int l = x / chunk_x;
            Hermes.GetInstance().log(this, l.ToString() + " Chunks in X-Richtung.", 4);
            int w = y / chunk_y;
            Hermes.GetInstance().log(this, w.ToString() + " Chunks in Y-Richtung.", 4);
            int h = z / chunk_z;
            Hermes.GetInstance().log(this, h.ToString() + " Chunks in Z-Richtung.", 4);

            int c = 0;

            for (int cx = 0; cx < chunk_x; cx++)
            {
                chunkMap.Add(new List<List<int>>());
                for (int cy = 0; cy < chunk_y; cy++)
                {
                    chunkMap[cx].Add(new List<int>());
                    for (int cz = 0; cz < chunk_z; cz++)
                    {
                        VillageChunk chunk = new VillageChunk(c, dbString);
                        chunk.SetThisChunkUp(new Vector3(cx * l, cy * w, cz * h), l, w, h);
                        chunk.Save();
                        chunkMap[cx][cy].Add(Convert.ToInt32(chunk.ID));
                        c++;
                    }
                }
            }

            chunkDimensions = new Vector3(l, w, h);
        }

        private int defineChunkDimension(int l)
        {
            if (l < 32)
            {
                if (l % 4 == 0)
                {
                    return 4;
                }

                if (l % 3 == 0)
                {
                    return 3;
                }

            }

            if (l > 32)
            {
                if (l % 8 == 0)
                {
                    return 8;
                }

                if (l % 4 == 0)
                {
                    return 4;
                }

                if (l % 3 == 0)
                {
                    return 3;
                }
            }

            return 2;
        }

        public void AddQuad(Quad quad)
        {
            VillageChunk chunk = GetChunkAtPosition(quad.AbsolutePosition);
            chunk.AddQuad(quad);
            chunk.Save();
        }

        public VillageChunk GetChunkAtPosition(Vector3 position)
        {
            int x = Convert.ToInt32(Math.Floor(position.X / chunkDimensions.X));
            int y = Convert.ToInt32(Math.Floor(position.Y / chunkDimensions.Y));
            int z = Convert.ToInt32(Math.Floor(position.Z / chunkDimensions.Z));
            VillageChunk chunk = new VillageChunk(chunkMap[x][y][z], dbString);
            chunk.Load();
            return chunk;
        }
    }
}
