using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.GraphicalMapRepresentation
{
    struct GraphicTile
    {
        private readonly Camera.Camera camera;
        private Dictionary<string, int[]> stateBoundIDSets;

        public GraphicTile(int tileType, Camera.Camera cam)
        {
            camera = cam;
            stateBoundIDSets = new Dictionary<string, int[]>();
        }
    }
}
