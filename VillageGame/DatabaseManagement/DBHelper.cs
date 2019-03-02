using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using Zeus.Hermes;

namespace Village.VillageGame.DatabaseManagement
{
    public static class DBHelper
    {
        private static readonly int DEBUG_LEVEL = 1;

        #region DB-Keys
        public static readonly string BODY_DB_KEY = "Body";
        public static readonly string SUBSTANCES_DB_KEY = "Substances";
        #endregion

        private static Dictionary<string, SQLiteConnection> currentConnections = new Dictionary<string, SQLiteConnection>();
        private static Dictionary<string, int> useCounter = new Dictionary<string, int>();
        private static Dictionary<string, string> cachedLocation = new Dictionary<string, string>();

        /// <summary>
        /// Ein SQLite-konformer Verbindungsstring wird erzeugt.
        /// </summary>
        /// <param name="dbName">Der Name der Datenbank, für die ein String erzeugt werden soll.</param>
        /// <returns>Ein valider SQLiteConnectionString.</returns>
        public static string CreateConnectionAndDatabase(string dbName, string initialQuery, string Location = "\\Databases\\")
        {
            Hermes.GetInstance().log("DB", "Öffne Datenbankverbindung zu ... " + dbName, DEBUG_LEVEL);
            if (!currentConnections.ContainsKey(dbName))
            {
                if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + Location))
                {
                    Hermes.GetInstance().log("DB", "Ordner muss erstellt werden... " + Location, DEBUG_LEVEL);
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + Location);
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + Location + dbName + ".sqlite"))
                {
                    Hermes.GetInstance().log("DB", "Erstelle Datenbank...", DEBUG_LEVEL);
                    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + Location + dbName + ".sqlite");
                    currentConnections[dbName] = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + Location + dbName + ".sqlite" + ";Version=3;");
                    useCounter[dbName] = 0;
                    cachedLocation[dbName] = Location;
                    currentConnections[dbName].Open();
                    if(initialQuery != "")
                    {
                        ExecuteCommandNonQuery(initialQuery, dbName); // Führt alle Befehle für das initiale aufsetzen der DB aus
                    }
                }
                else
                {
                    useCounter[dbName] = 0;
                    cachedLocation[dbName] = Location;
                    currentConnections[dbName] = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + Location + dbName + ".sqlite" + ";Version=3;");
                    currentConnections[dbName].Open();
                }
                
                return dbName;
            }
            else
            {
                int i = 0;
                while (currentConnections.ContainsKey(dbName + i))
                {
                    i++;
                }
                return CreateConnectionAndDatabase(dbName + i, initialQuery, Location);
            }
        }

        /// <summary>
        /// Öffnet und speichert eine SQLiteConnection ab
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="Location"></param>
        public static void OpenConnection(string dbName, string Location)
        {
            useCounter[dbName] = 0;
            cachedLocation[dbName] = Location;
            currentConnections[dbName] = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + Location + dbName + ".sqlite" + ";Version=3;");
            currentConnections[dbName].Open();
            Hermes.GetInstance().log("DBManagement", "Folgende DB geoeffnet: " + dbName, DEBUG_LEVEL);
        }

        /// <summary>
        /// Führt den angegebenen SQLite-Befehl auf der MainDB aus.
        /// Diese Methode ist für alle Fälle gedacht, deren Command keine Rückgabe erzeugt.
        /// </summary>
        /// <param name="cmdString">Der vollständige, formatierte SQLite-Befehlsstring. ACHTUNG: ; nicht vergessen!</param>
        public static void ExecuteCommandNonQuery(string cmdString, string dbString)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(cmdString, currentConnections[dbString]);
                command.ExecuteNonQuery();
                useCounter[dbString]++;

                if(useCounter[dbString] >= 25)
                {
                    currentConnections[dbString].Close();
                    currentConnections[dbString].Dispose();
                    OpenConnection(dbString, cachedLocation[dbString]);
                }
                command.Dispose();
                Hermes.GetInstance().log("Following command was executed: " + cmdString);
            }
            catch (Exception e)
            {
                Hermes.GetInstance().log("DB","An error occured while executing this command: " + cmdString + " on the following database " + dbString, DEBUG_LEVEL);
                Hermes.GetInstance().log("DB","The error was: " + e.Message, DEBUG_LEVEL);
            }
        }

        /// <summary>
        /// Der übergebene SQLite-Befehlsstring wird auf der MainDB ausgeführt und das die Rückgabe der Query wird in einem
        /// DataSet abgelegt. Diese Methode ist für rückgabe-behaftete Befehle vorgesehen.
        /// </summary>
        /// <param name="cmdString">Dieser String ist der Befehel, der ausgeführt werden soll. Muss mit einem ';' geschlossen werden.</param>
        /// <param name="dbString">Der Connection string.</param>
        /// <returns>Dieses DataSet enthält die Rückgabe der DB.</returns>
        public static DataSet ExecuteQuery(string cmdString, string dbString)
        {
            try
            {
                SQLiteDataAdapter sqlite_adapter;  // Data Reader Object

                sqlite_adapter = new SQLiteDataAdapter(cmdString, currentConnections[dbString]);

                Hermes.GetInstance().log("Following command will be executed: " + cmdString);

                DataSet Return = new DataSet();

                int rows = sqlite_adapter.Fill(Return);
                Hermes.GetInstance().log("This amount of rows was found: " + rows.ToString());

                if (useCounter[dbString] >= 25)
                {
                    currentConnections[dbString].Close();
                    currentConnections[dbString].Dispose();
                    OpenConnection(dbString, cachedLocation[dbString]);
                }
                sqlite_adapter.Dispose();
                return Return;
            }
            catch (Exception e)
            {
                Hermes.GetInstance().log("DB", "Following error occured: " + e.Message + " on the following database " + dbString, DEBUG_LEVEL);
                return null;
            }

        }

        public static void OpenMainConnections()
        {
            Hermes.GetInstance().log("DB-System", "Alle Hauptverbindungen werden vorbreitet...", DEBUG_LEVEL);
            OpenConnection("Substances", "//Content//Main//");
            OpenConnection("Body", "//Content//Main//");
        }
    }
}
