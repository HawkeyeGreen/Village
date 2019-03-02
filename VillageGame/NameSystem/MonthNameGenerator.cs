using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;
using Village.VillageGame.Helper;

namespace Village.VillageGame.NameSystem
{
    public class MonthNameGenerator : NameGenerator
    {
        List<string> names = new List<string>();

        public MonthNameGenerator(string theme, string sourceDB)
        {
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT name FROM ThemedNames WHERE theme='" + theme + "';", sourceDB).CreateDataReader();
            while (reader.Read())
            {
                names.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("name"))));
            }
        }

        public override string GetName()
        {
            return names[RandomHelper.random.Next(0, names.Count)];
        }
    }
}
