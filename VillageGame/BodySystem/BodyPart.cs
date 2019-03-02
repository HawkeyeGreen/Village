using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem
{
    class BodyPart
    {
        private Body myBody;
        List<VitalSystem> subscribers = new List<VitalSystem>();
        List<BodyLayer> layers = new List<BodyLayer>();

        Dictionary<int, List<Organ>> organs = new Dictionary<int, List<Organ>>(); // layer, organs im layer



        private double devastation;

        #region Felder
        public double Devastation => devastation;
        #endregion

        /// <summary>
        /// Laden eines PartTemplates aus der Datenbank.
        /// </summary>
        /// <param name="partID"></param>
        /// <param name="DB"></param>
        /// <param name="body"></param>
        public BodyPart(string DB, int partTID, Body body)
        {
            myBody = body;
            devastation = 0;

            DataTableReader reader;
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyPartTemplates", DB).CreateDataReader();

        }

        public static void CreateBodyPartTables(string DB)
        {

        }
    }
}
