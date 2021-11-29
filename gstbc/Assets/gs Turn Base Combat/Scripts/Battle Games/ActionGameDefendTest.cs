using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{

    public class ActionGameDefendTest : BattleGame
    {
        [SerializeField] ParticleSystem particle;
        public override void StartGame(UnityAction gameFinishedCallback, BattleLogic controller)
        {
            StartCoroutine(Game(gameFinishedCallback, controller));
        }

        private IEnumerator Game(UnityAction gameFinishedCallback, BattleLogic controller)
        {
            controller.currentCombatant.transform.position = Vector3.zero;

            yield return new WaitForSeconds(1f);
            particle.Play();
            yield return new WaitForSeconds(2.5f);
            controller.currentCombatant.transform.position = controller.currentCombatant.GetResetPos();
            yield return new WaitForSeconds(0.5f);

            gameFinishedCallback?.Invoke();
        }
    }
}
