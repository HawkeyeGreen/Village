using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.BodySystem.Wounds
{
    class Trauma
    {
        private int id;
        private string causedBy;
        private bool unique;
        private string name;

        public string Cause => causedBy;
        public string Name => name;
        public bool Unique => unique;
        public int ID => id;

        public void Refresh()
        {

        }
    }
}
