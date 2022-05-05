using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace MissionMultipliers
{
    public class MissionMultiplierSettings : UnityModManager.ModSettings
    {
        public float MissionPayMultiplier = 1.0f;
        public float MissionRepMultiplier = 1.0f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }    
}
