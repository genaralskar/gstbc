using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{
    public abstract class CombatAction : ScriptableObject
    {
        public abstract void UseAction(BattleLogic battleLogic, UnityAction actionGameEndedCallback);
        public abstract string ActionName { get; }
        public abstract List<CombatantType> TargetTypes { get; }
        public abstract ActionType ActionType{ get; }
        public abstract GameObject Prefab { get; }
    }
}