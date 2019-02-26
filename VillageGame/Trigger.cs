using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame
{
    abstract class Trigger
    {
        private List<TriggerKeys> triggerMe = new List<TriggerKeys>();
        public List<TriggerKeys> TriggerOn
        {
            get => triggerMe;
            set => triggerMe = value;
        }

        public abstract void trigger();
    }

    enum TriggerKeys
    {
        OnRemove,
        OnAdd,
        OnVaporate,
        OnHit,
        OnKill,
        OnDeath,
        OnHeat,
        OnCool,
        OnDrink,
        OnEat,
        OnLight,
        OnFire,
        OnShoot,
        OnSublimate,
        OnCondense
    }
}
