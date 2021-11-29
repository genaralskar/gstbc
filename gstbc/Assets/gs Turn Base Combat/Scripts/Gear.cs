using System.Collections.Generic;
using UnityEngine;

namespace genaralskar.Battle
{
    [CreateAssetMenu(menuName = "Battle/Gear")]
    public class Gear : ScriptableObject
    {
        public GearType gearType;
        public List<GearStat> gearStats;

        [System.Serializable]
        public struct GearStat
        {
            public CombatantStats stat;
            public int amount;
        }
    }

}
