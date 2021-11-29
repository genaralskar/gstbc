using System.Collections.Generic;
using UnityEngine;

namespace genaralskar.Battle
{
    [CreateAssetMenu(menuName = "Battle/Combatant Data")]
    public class CombatantData : ScriptableObject
    {
        new public string name;
        public bool isPlayerControlled = false;
        public GameObject prefab;
        public List<CombatantType> combatantTypes;
        // public Sprite image;

        public int str = 1;
        public int dex = 1;
        public int wis = 1;
        public int con = 1;
        public int def = 1;
        public int spd = 1;

        public Gear helmet;
        public Gear armor;
        public Gear boots;
        public Gear weapon;
        public Gear offhand;
        public List<Gear> jewelry;

        public List<CombatAction> combatActions;

        public bool IsEnemyType(List<CombatantType> testTypes)
        {
            foreach(var t in testTypes)
            {
                if (IsEnemyType(t)) return true;
            }

            return false;
        }

        public bool IsEnemyType(CombatantType testType)
        {
            foreach(var ct in combatantTypes)
            {
                if (ct.IsEnemyType(testType)) return true;
            }

            return false;
        }
    }
}