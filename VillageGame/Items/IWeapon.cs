using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.World.ReactionSystem;

namespace Village.VillageGame.Items
{
    interface IWeapon
    {
        (double MaxContacArea, double MinContactArea) ContactArea { get; }
        int Sharpness { get; }
        Material Material { get; }
    }
}
