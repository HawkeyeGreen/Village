using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.World.TimeSystem
{
    public class VillageCalendar
    {
        private static Dictionary<string, VillageCalendar> loadedCalendars = new Dictionary<string, VillageCalendar>();

        private VillageYear year;
        private int currentYear;

        private VillageCalendar(string name)
        {

        }

        public static VillageCalendar GetCalendar(string name)
        {
            if (!loadedCalendars.ContainsKey(name))
            {
                loadedCalendars[name] = new VillageCalendar(name);
            }
            return loadedCalendars[name];
        }

        public void UpdateMonth()
        {

        }
    }
}
