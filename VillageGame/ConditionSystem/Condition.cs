using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.ConditionSystem
{
    abstract class Condition
    {
        private readonly int id;
        private readonly string conditionType;


        public string ConditionType => conditionType;
        public int ID => id;

        public Condition(int ID, string type)
        {
            id = ID;
            conditionType = type;
        }

        public abstract bool Check();

        public abstract int GetNumberOfArguments();
    }
}
