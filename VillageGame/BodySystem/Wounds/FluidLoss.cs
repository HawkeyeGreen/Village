using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.BodySystem.Wounds
{
    /// <summary>
    /// Der Verlust einer Körperflüssigkeit
    /// </summary>
    struct FluidLoss : Subscriber
    {
        private Body body; // Der Körper, zu dem ich gehöre
        private Wound cause; // Zu welcher Wunde gehöre ich?
        private string name; // Welche Körperflüssigkeit?

        private double initialLoss; // Wie viel verlore der Körper zu beginn?
        private double currentLoss; // Wie viel verliert der Köper gerade?
        private double lossLoss; // Um wie viel verringert sich der Loss pro Tick?
        private double dryUpThreshold; // Ab wann fällt der Loss auf 0?

        public string Name => name;
        public double InitialLoss => initialLoss;
        public double CurrentLoss => currentLoss;
        public double LossLoss => lossLoss;
        public double DryUpThreshold => dryUpThreshold;

        public void Update()
        {
            if(cause.Healed)
            {
                // Entferne FluidLoss
            }
        }
    }
}
