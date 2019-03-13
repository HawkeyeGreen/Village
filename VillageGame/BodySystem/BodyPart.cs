using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.CombatSystem;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem
{
    class BodyPart
    {
        private Body myBody;
        private BodyPart myConnector;
        private BodyPart myConnected;

        List<VitalSystem> subscribers = new List<VitalSystem>();
        List<BodyLayer> layers = new List<BodyLayer>();

        Dictionary<int, List<Organ>> organs = new Dictionary<int, List<Organ>>(); // layer, organs im layer



        /// <summary>
        /// Laden eines PartTemplates aus der Datenbank.
        /// </summary>
        /// <param name="partID"></param>
        /// <param name="DB"></param>
        /// <param name="body"></param>
        public BodyPart(string DB, int partTID, Body body, BodyPart connected = null)
        {
            myBody = body;
            myConnector = connected;
            
            DataTableReader reader;
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyPartTemplates", DB).CreateDataReader();

        }

        public static void CreateBodyPartTables(string DB)
        {

        }

        public void ApplyAttack(AttackNugget attack)
        {

        }
    }
}
