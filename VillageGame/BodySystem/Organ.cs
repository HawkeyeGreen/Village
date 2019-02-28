using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem
{
    class Organ
    {
        private int ID;

        // Namen, NamenLink und BeschreibungsLink
        private readonly string myName;
        private string displayName;
        private string descrString;

        private string DBKey;

        TagSystem.TagSet tags;

        private List<VitalSystem> subscribers;
        private List<string> callMeMaybe;
        public List<string> CallMeMaybe => callMeMaybe;

        private Body body;
        private BodyPart bodyPart;

        // Wie gut funktioniert das Organ
        private double condition;

        private double stillFunctionalArea;
        private double maxHitArea;

        // Sobald die Condition kleiner als dieser Wert ist,
        // dann setzt das Organ aus.
        private double functionTH;

        #region Felder

        public string Name => myName;
        public string DisplayName_KEY => displayName;
        public string DisplayDescr_KEY => descrString;

        public string DisplayName => Localization.Local.GetInstance().GetString(DisplayName_KEY);
        public string DisplayDescription => Localization.Local.GetInstance().GetString(DisplayDescr_KEY);

        public double Condition => condition;

        public bool Working
        {
            get
            {
                if (condition < functionTH)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        /// <summary>
        /// Lädt ein Organ-Template aus der Body-DB.
        /// Dieses Template ist mit allen Daten AUßER einer ID versehen.
        /// Seine ID erhält das Organ, sobald es in eine Session-DB geschrieben wird.
        /// </summary>
        /// <param name="name">Der Name des Organ-Templates.</param>
        /// <param name="_Body">Der Körper, zu dem das Organ gehören soll.</param>
        /// <param name="_bodyPart">Das Körperteil, in dem sich das Organ befindet.</param>
        public Organ(string name, Body _Body, BodyPart _bodyPart)
        {
            subscribers = new List<VitalSystem>();
            tags = new TagSystem.TagSet();

            tags.FillTagSet(name, DBHelper.BODY_DB_KEY);
            Init(_Body, _bodyPart);
            LoadTemplate(DBHelper.ExecuteQuery("SELECT * FROM OrganTemplates WHERE name='" + name + "';", DBHelper.BODY_DB_KEY).CreateDataReader());
            LoadTemplateVitalLinks(DBHelper.ExecuteQuery("SELECT * FROM OrganVitalTemplates WHERE oName='" + name + "';", DBHelper.BODY_DB_KEY).CreateDataReader());

            // Template => Alle Arbeitswerte auf Default
            condition = 100.00;
            stillFunctionalArea = maxHitArea;
        }

        /// <summary>
        /// Lädt das Organ-Template aus 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_Body"></param>
        /// <param name="_bodyPart"></param>
        /// <param name="SourceDB"></param>
        public Organ(string name, Body _Body, BodyPart _bodyPart, string SourceDB)
        {

        }

        /// <summary>
        /// Mit diesem Konstruktor wird die Instanz des Organs mit der übergebenen ID aus
        /// der übergebenen DB wiederhergestellt.
        /// </summary>
        /// <param name="ID">Die ID des Organs.</param>
        /// <param name="DBKey">Der Key des Organs.</param>
        /// <param name="_Body"></param>
        /// <param name="_bodyPart"></param>
        public Organ(int ID, string DBKey, Body _Body, BodyPart _bodyPart)
        {
            this.ID = ID;
            this.DBKey = DBKey;
            Init(_Body, _bodyPart);
        }

        /// <summary>
        /// Initialisiert das Organ mit den Standardverbindungen zu Körper und Körperteil.
        /// </summary>
        /// <param name="_Body">Körper, zu dem das Organ gehört.</param>
        /// <param name="_bodyPart">Körpersegment, in dem es sich befindet.</param>
        private void Init(Body _Body, BodyPart _bodyPart)
        {
            body = _Body;
            bodyPart = _bodyPart;
        }

        /// <summary>
        /// Lädt ein Organ-Template aus der Verlinkung.
        /// </summary>
        /// <param name="reader">Reader, welcher auf das Ergebnis der Datenbankanfrage zeigt.</param>
        private void LoadTemplate(DataTableReader reader)
        {
            reader.Read();
            displayName = Convert.ToString(reader.GetString(reader.GetOrdinal("dName")));
            descrString = Convert.ToString(reader.GetString(reader.GetOrdinal("dDescr")));
            maxHitArea = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("maxHitArea")));
            functionTH = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("functionalTh")));
            reader.Close();
        }

        /// <summary>
        /// Erstellt die CallMeMaybe-Liste
        /// </summary>
        /// <param name="reader"></param>
        private void LoadTemplateVitalLinks(DataTableReader reader)
        {
            callMeMaybe = new List<string>();
            while (reader.Read())
            {
                callMeMaybe.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("vName"))));
            }

            reader.Close();
        }

        private void Load(DataTableReader reader)
        {
            reader.Read();

            reader.Close();
        }

        public void Subscribe(VitalSystem vital) => subscribers.Add(vital);

        private void UpdateCall()
        {
            foreach (VitalSystem system in subscribers)
            {
                system.Update(this);
            }
        }

        public void HitMe(double hitArea)
        {
            if ((hitArea >= maxHitArea) || (hitArea >= stillFunctionalArea))
            {
                condition = 0.00;
                stillFunctionalArea = 0.00;
            }
            else
            {
                stillFunctionalArea -= hitArea;
                condition = (stillFunctionalArea / maxHitArea) * 100;
            }

            UpdateCall();
        }
    }
}
