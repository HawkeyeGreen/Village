using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zeus.Hermes;
using Village.VillageGame.World.VillageMap;

namespace Village.VillageGame.World.Generator
{
    public class PrimitiveGenerator
    {
        private readonly int lenght;
        private readonly int width;
        private readonly int height;

        private VillageMap.VillageMap map;
        public VillageMap.VillageMap Map => map;

        public PrimitiveGenerator(int l, int w, int h)
        {
            lenght = l;
            width = w;
            height = h;
        }

        public void GeneratePrimitiveMap()
        {
            map = new VillageMap.VillageMap("Primitive", "//Content//Main//");
            map.InitialMapDimensions(lenght, width, height);
        }
    }
}
