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

            // Check for triggered Traumata
            
            foreach(BodyFluid bodyFluid in fluids.Values)
            {
                List<string> traumata = bodyFluid.GetTraumata();

                foreach(string trauma in traumata)
                {
                    List<Trauma> findings = LookUpTrauma(trauma);
                    bool addable;

                    if(findings.Count == 0)
                    {
                        addable = true;
                    }
                    else
                    {
                        addable = true;
                        // Gucke, ob die gefundenen Traumata einzigartig sind oder bereits durch den Verlust der gegebenen Körperflüssigkeit verursacht wurden
                        foreach(Trauma foundTrauma in findings)
                        {
                            if(foundTrauma.Unique || foundTrauma.Cause == "FluidLoss Of " + bodyFluid.Fluid.Substance.Name)
                            {
                                addable = false;
                                foundTrauma.Refresh(); // Grund IMMER noch aktiv!
                                break;
                            }
                        }                        
                    }

                    if (addable)
                    {
                        // Add Trauma
                    }
                }

            }

            #endregion
        }

        public void RemoveFluidLoss(FluidLoss loss) => fluidLosses.Remove(loss);

        public void Death()
        {

        }

        private List<Trauma> LookUpTrauma(in string name)
        {
            List<Trauma> findings = new List<Trauma>();
            foreach(Trauma trauma in currentTraumata)
            {
                if(trauma.Name == name)
                {
                    findings.Add(trauma);
                }
            }
            return findings;
        }
    }

}
