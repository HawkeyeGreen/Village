using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.TimeSystem
{
    public class VillageCalendar
    {
        private static VillageCalendar currentCalendar;
        private LinkedList<VillageMonth> months;
        private LinkedList<VillageMonth>.Enumerator enumerator;

        private VillageCalendar(string DB)
        {
            enumerator = months.GetEnumerator();
        }

        private VillageCalendar()
        {
            months = new LinkedList<VillageMonth>();


            enumerator = months.GetEnumerator();
        }

        public static void Load(string DB)
        {
            currentCalendar = new VillageCalendar(DB);
        }

        public static void GenerateCalendar()
        {
            currentCalendar = new VillageCalendar();
        }

        public static VillageCalendar GetCalendar() => currentCalendar;

        public VillageMonth GetCurrentMonth()
        {
            return enumerator.Current;
        }

        public void UpdateMonth()
        {

        }
    }
}
