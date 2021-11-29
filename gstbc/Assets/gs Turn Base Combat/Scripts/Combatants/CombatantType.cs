using System.Collections.Generic;
using UnityEngine;

namespace genaralskar.Battle
{
    [CreateAssetMenu(menuName = "Battle/Combatant Type")]
    public class CombatantType : ScriptableObject
    {
        [Tooltip("List of CombatantTypes that this CombatantType would consider an enemy." +
            "For example, a Player would consider a Monster an enemy. A Guard would consider a Monster an enemy as well")]
        [SerializeField] List<CombatantType> enemyTypes;

        public bool IsEnemyType(CombatantType testType)
        {
            return enemyTypes.Contains(testType);
        }

        public bool IsEnemyType(List<CombatantType> testTypes)
        {
            foreach(var t in testTypes)
            {
                if (IsEnemyType(t)) return true;
            }

            return false;
        }
    }
}