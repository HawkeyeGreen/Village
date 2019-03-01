using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.TimeSystem
{
    static class VillageTime
    {
        private static int tick = 1;
        private static int currentTenMinute = 1;
        private static int currentHour = 1;

        public static void UpdateTick()
        {
            tick++;
            if(tick >= 60)
            {
                currentTenMinute++;
                tick = 0;
            }

            if(currentTenMinute >= 6)
            {
                currentHour++;
                currentTenMinute = 0;
            }

            if(currentHour >= 24)
            {

            }
        }
    }
}
