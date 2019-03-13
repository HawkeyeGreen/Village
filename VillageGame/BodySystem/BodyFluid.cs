using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.World.ReactionSystem;

namespace Village.VillageGame.BodySystem
{
    /// <summary>
    /// Eine Körperflüssigkeit.
    /// </summary>
    struct BodyFluid
    {
        private double maxVolume;
        private double currentVolume;

        private Phase fluid;
        
        public BodyFluid(double startVolume, Material fluidMaterial, Temperature bodyTmp)
        {
            maxVolume = startVolume;
            currentVolume = startVolume;
            double amount = startVolume * fluidMaterial.Substance.GetDensity(fluidMaterial.Substance.GetAggregate(bodyTmp));
            fluid = new Phase(amount, fluidMaterial, bodyTmp);
        }
    }
}
