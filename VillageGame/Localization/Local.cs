using System.Collections.Generic;
using System.Data;
using Village.VillageGame.DatabaseManagement;
using Zeus.Hermes;

namespace Village.VillageGame.Localization
{
    public class Local : HermesLoggable
    {
        private static readonly string[] languageStrings = new string[]
        {
            "ENG",
            "DE"
        };

        private static int debugLvl = 4;
        private static Local Instance;
        private static Language language = Language.DE;
        private Dictionary<string, string> localStrings = new Dictionary<string, string>();
        public long ID => 1337;
        public string Type => "LocalizationSystem";

        private Local(Language lang)
        {
            language = lang;
            DBHelper.OpenConnection(DBHelper.LANGUAGE_DB_KEY, "//Content//Main//");
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM " + language + ";", "Lang").CreateDataReader();

            while (reader.Read())
            {
                localStrings[reader.GetString(reader.GetOrdinal("LKey"))] = reader.GetString(reader.GetOrdinal("LStr"));
                Hermes.GetInstance().log(this, "Folgender lang-Str wurde geladen: " + reader.GetString(reader.GetOrdinal("LStr")) + " | Gespeichert unter: " + reader.GetString(reader.GetOrdinal("LKey")), debugLvl);
            }
            reader.Close();
        }

        public static Local GetInstance()
        {
            if(Instance == null)
            {
                Instance = new Local(language);
            }
            return Instance;
        }

        public void ChangeLanguage(Language newLange)
        {
            if(language != newLange)
            {
                Hermes.GetInstance().log("Localization", "Sprache wurde auf " + newLange + " umgestellt.", debugLvl);
                language = newLange;
                DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM " + language + ";", "Lang").CreateDataReader();

                while (reader.Read())
                {
                    localStrings[reader.GetString(reader.GetOrdinal("LKey"))] = reader.GetString(reader.GetOrdinal("LStr"));
                    Hermes.GetInstance().log("Localization", "Folgender lang-Str wurde geladen: " + reader.GetString(reader.GetOrdinal("LStr")) + " | Gespeichert unter: " + reader.GetString(reader.GetOrdinal("LKey")), debugLvl);
                }
                reader.Close();
            }
            else
            {
                Hermes.GetInstance().log("Localization", "Es wurde versucht die Sprache umzustellen, aber die Sprache war bereits eingestellt. \n Alte Sprache: " + language + " \n Neue Sprache: " + newLange, debugLvl);
            }

        }

        public string GetString(string key)
        {
            if(localStrings.ContainsKey(key))
            {
                return localStrings[key];
            }
            else
            {
                return "STR:" + key + " was not found.";
            }
        }
    }

    public enum Language
    {
        ENG,
        DE
    }
}
