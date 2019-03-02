using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.World.ReactionSystem;

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
        /// Laden einer Part-Instanz
        /// </summary>
        /// <param name="partID"></param>
        /// <param name="DB"></param>
        /// <param name="body"></param>
        public BodyPart(int partID, string DB, Body body)
        {
            myBody = body;
        }
    }
}
