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
        private readonly Phase fluid;

        public double MaxVolume
        {
            get => maxVolume;

            set
            {
                if (currentVolume >= maxVolume)
                {
                    maxVolume = value;
                    currentVolume = maxVolume;
                }

                if (currentVolume > maxVolume)
                {
                    currentVolume = maxVolume;
                }
            }
        }

        /// <summary>
        /// Das derzeitig verbleibende Volumen an Körperflüssigkeit.
        /// </summary>
        public double CurrentVolume
        {
            get => currentVolume;

            set
            {
                if (value > maxVolume)
                {
                    currentVolume = maxVolume;
                }
                else
                {
                    currentVolume = value;
                }

                fluid.Volume = currentVolume;
            }
        }

        /// <summary>
        /// Die Stoffphase, welche die Körperflüssigkeit repräsentiert.
        /// </summary>
        public Phase Fluid => fluid;

        public BodyFluid(double startVolume, Material fluidMaterial, Temperature bodyTmp)
        {
            maxVolume = startVolume;
            currentVolume = startVolume;
            double amount = startVolume * fluidMaterial.Substance.GetDensity(fluidMaterial.Substance.GetAggregate(bodyTmp));
            fluid = new Phase(amount, fluidMaterial, bodyTmp);
        }
    }
}
