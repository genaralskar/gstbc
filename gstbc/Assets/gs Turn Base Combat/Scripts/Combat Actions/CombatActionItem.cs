using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{
    [CreateAssetMenu(menuName = "Battle/Item Action")]
    public class CombatActionItem : CombatAction
    {
        public bool targetAll = false;

        [SerializeField] string actionName;
        public override string ActionName => actionName;

        [SerializeField] ActionType actionType;
        public override ActionType ActionType => actionType;

        [SerializeField] List<CombatantType> targetTypes;
        public override List<CombatantType> TargetTypes => targetTypes;

        [SerializeField] GameObject prefab;
        public override GameObject Prefab => prefab;

        public override void UseAction(BattleLogic battleLogic, UnityAction battleGameEndedCallback)
        {
            // get str values and stuff
        }
    }
}