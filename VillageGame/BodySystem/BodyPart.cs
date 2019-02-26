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
        List<VitalSystem> subscribers = new List<VitalSystem>();
        

        private double devastation;

        #region Felder
        public double Devastation => devastation;
        #endregion
    }
}
