using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.TagSystem;

namespace Village.VillageGame.BodySystem
{
    class Body
    {
        List<BodyPart> parts = new List<BodyPart>();
        List<VitalSystem> vitalSystems = new List<VitalSystem>();
        List<string> vitalOrgans = new List<string>();
        List<Trauma> currentTraumata = new List<Trauma>();
        List<FluidLoss> fluidLosses = new List<FluidLoss>();

        Dictionary<string, BodyFluid> fluids = new Dictionary<string, BodyFluid>();
        

        Dictionary<string, float> PartPercentages = new Dictionary<string, float>();

        private double height = 1.8; // m
        private double width = 0.426; // m
        private double depth = 0.2; // m
        private double mass = 80; // kg

        TagSet tags = new TagSet();

        /// <summary>
        /// Aktualisiere alle Körperwerte. CombatTick!
        /// </summary>
        public void Tick()
        {
            #region FluidLoss
            for(int i = 0; i < fluidLosses.Count; i++)
            {
                FluidLoss fluidLoss = fluidLosses[i];
                double loss = fluidLoss.Tick();

                if(fluids.ContainsKey(fluidLoss.Name) && loss != 0)
                {
                    BodyFluid fluid = fluids[fluidLoss.Name];
                    fluid.CurrentVolume -= loss;
                }

                if (loss == 0)
                {
                    RemoveFluidLoss(fluidLoss);
                }
            }
            #endregion
        }

        public void RemoveFluidLoss(FluidLoss loss) => fluidLosses.Remove(loss);
    }
}
