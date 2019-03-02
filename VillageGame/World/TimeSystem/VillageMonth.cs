using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.TimeSystem
{
    public class VillageMonth
    {
        private readonly string myCalendar;
        private readonly string name;
        private readonly int treshold;

        private int day;

        public string Name => name;
        public int Day => day;
        public int Treshold => treshold;

        /// <summary>
        /// Initialisiert einen Monat mit der gegebenen Tagesgrenze.
        /// </summary>
        /// <param name="treshold">Der letzte Tag des Monats</param>
        /// <param name="name">Der Name des Monats</param>
        public VillageMonth(int treshold, string name, string calendar)
        {
            day = 1;
            this.treshold = treshold;
            this.name = name;
            myCalendar = calendar;
        }

        public void UpdateDay()
        {
            day++;
            if(day > treshold)
            {
                VillageCalendar.GetCalendar(myCalendar).UpdateMonth();
                day = 1;
            }
        }
    }
}
