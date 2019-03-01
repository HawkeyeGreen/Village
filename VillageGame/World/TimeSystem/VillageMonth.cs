using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.TimeSystem
{
    public class VillageMonth
    {
        private int day = 1;
        private int treshold = 30;

        public void UpdateDay()
        {
            day++;
            if(day >= treshold)
            {
                VillageCalendar.GetCalendar().UpdateMonth();
            }
        }
    }
}
