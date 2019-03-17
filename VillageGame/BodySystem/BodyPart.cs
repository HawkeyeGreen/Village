using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.CombatSystem;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem
{
    class BodyPart
    {
        private Body myBody;
        private BodyPart myConnector;
        private BodyPart myConnected;

        List<BodyLayer> layers = new List<BodyLayer>();             



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
            List<LayerDamage> damages = new List<LayerDamage>();

            for(int l = 0; l < layers.Count; l++)
            {
                LayerDamage layerDamage = layers[l].ApplyAttack(attack);

                if(layerDamage.Equals(LayerDamage.Repelled))
                {
                    // Handle DamageReturn

                }


                attack.Force -= (float)layers[l].ForceReduction;
                if(attack.Force <= 0)
                {
                    break;
                }
            }

            if(damages.Count > 0)
            {
                myBody.WoundMe(damages);
            }
        }
    }
}
