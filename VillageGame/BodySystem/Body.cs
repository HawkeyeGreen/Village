using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.TagSystem;

namespace Village.VillageGame.BodySystem
{
    class Body
    {
        List<BodyPart> parts = new List<BodyPart>();
        List<VitalSystem> vitalSystems = new List<VitalSystem>();
        List<string> vitalOrgans = new List<string>();
        List<Trauma> currentTraumata = new List<Trauma>();

        TagSet tags = new TagSet();


    }
}
