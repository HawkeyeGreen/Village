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

        Dictionary<string, float> PartPercentages = new Dictionary<string, float>();

        private double height = 1.8; // m
        private double width = 0.426; // m
        private double depth = 0.2; // m
        private double mass = 80; // kg

        TagSet tags = new TagSet();

        public void Tick()
        {

        }
    }
}
