using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{
    public abstract class BattleGame : MonoBehaviour
    {
        public abstract void StartGame(UnityAction gameFinishedCallback, BattleLogic controller);
    }
}
