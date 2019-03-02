using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.World.TimeSystem
{
    class VillageYear
    {
        private static VillageYear moonYear;

        private string name;
        private string myCalendar;

        private LinkedList<VillageMonth> months;
        private LinkedList<VillageMonth>.Enumerator month;

        public VillageMonth CurrentMonth => month.Current;
        public string Name => name;

        /// <summary>
        /// Lädt das angegebene Jahr aus der DB.
        /// </summary>
        /// <param name="name"></param>
        public VillageYear(string name, string DB)
        {
            this.name = name;
            DataTableReader reader;

            reader = DBHelper.ExecuteQuery("SELECT * FROM Years WHERE yName='" + name + "';", DB).CreateDataReader();
            myCalendar = Convert.ToString(reader.GetString(reader.GetOrdinal("calendar")));
            reader.Close();

            reader = DBHelper.ExecuteQuery("SELECT * FROM Months WHERE yName='" + name + "';", DB).CreateDataReader();
            while (reader.Read())
            {
                months.AddLast(new VillageMonth(Convert.ToInt32(reader.GetInt64(reader.GetOrdinal("max"))), Convert.ToString(reader.GetString(reader.GetOrdinal("name"))), myCalendar));
            }
            reader.Close();

            GenerateEnumerator();
        }

        public static void LoadBaseYear(string DB) => moonYear = new VillageYear("MoonYear", DB);

        public VillageYear(string name)
        {

        }

        public bool UpdateMonth()
        {
            if (!month.MoveNext())
            {
                GenerateEnumerator();
                return true;
            }

            return false;
        }

        private void GenerateEnumerator()
        {
            month = months.GetEnumerator();
            month.MoveNext();
        }
    }
}
