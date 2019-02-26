using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.BodySystem
{
    class VitalSystem
    {
        private bool shutdown = false;

        private Dictionary<string, float> assignedPartFactor = new Dictionary<string, float>();
        private Dictionary<string, float> currentPartWorkingState = new Dictionary<string, float>();

        private HashSet<string> shutdownOrgans = new HashSet<string>();

        private float currentWorkingStatus = 100f;
        private float stopThreshold = -1f;

        public float StopThreshold
        {
            get => stopThreshold;
            set => stopThreshold = value;
        }

        public bool Working
        {
            get
            {
                if (shutdown)
                {
                    return false;
                }
                else
                {
                    if (currentWorkingStatus < StopThreshold)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        public void ChangePartWorkingStatus(string part, float currentWorkingCap)
        {
            currentPartWorkingState[part] = currentWorkingCap;
            RecalculateWorkingStatus();
        }

        private void RecalculateWorkingStatus()
        {
            if (!shutdown)
            {
                float workingStatus = 0f;
                foreach (string Part in currentPartWorkingState.Keys)
                {
                    workingStatus += assignedPartFactor[Part] * currentPartWorkingState[Part];
                }
            }
        }

        public void Update(Organ caller)
        {
            if (!caller.Working && shutdownOrgans.Contains(caller.Name))
            {
                shutdown = true;
            }

            if (currentPartWorkingState.ContainsKey(caller.Name))
            {
                RecalculateWorkingStatus();
                if (currentWorkingStatus < stopThreshold)
                {
                    shutdown = true;
                }
            }

        }

        public void Update(BodyPart caller)
        {

        }
    }
}
