using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.BodySystem.Wounds
{
    struct Wound
    {
        private long ID;
        private WoundType type;

        private bool healed;

        public bool Healed => healed;

        private List<Subscriber> subscribers;

        public void Unsubscribe(Subscriber subscriber) => subscribers.Remove(subscriber);
        public void Subscribe(Subscriber subscriber) => subscribers.Add(subscriber);
    }
}
