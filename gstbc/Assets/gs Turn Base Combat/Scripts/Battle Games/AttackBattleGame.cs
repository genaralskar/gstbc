using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace genaralskar.Battle
{

    // Single target. The player moves in front of the target. A bar fills up with an indicator to press "Fire1".
    // When the user presses "Fire1" while the bar is displayed, it will damage the selected target based on how full the bar is.
    // The bar will fill up once then go back down. When it hits the bottom the attack automatically finished and deals minimum damage.

    public class AttackBattleGame : BattleGame
    {
        public float damageMod = 1;
        [SerializeField] GameObject container;
        public float fillSpeed = 1;
        public Image fillbar;

        public override void StartGame(UnityAction gameFinishedCallback, BattleLogic controller)
        {
            // spawn fill meter. Damage = % of fill from meter
            StartCoroutine(PlayGame(gameFinishedCallback, controller));
        }

        private IEnumerator PlayGame(UnityAction gameFinishedCallback, BattleLogic controller)
        {
            // display prefab stuff
            fillbar.fillAmount = 0;
            container.gameObject.SetActive(true);

            // move combatant in front of target
            Vector3 movePos = controller.currentCombatant.GetAttackingPos(controller.combatantTarget);
            controller.currentCombatant.transform.position = movePos;

            // show fillbar
            fillbar.gameObject.SetActive(true);

            // wait a beat
            yield return new WaitForSeconds(.3f);

            // start fillbar
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float fill = 0;
            float timer = 0;
            while (timer < 2 & !Input.GetButton("Fire1"))
            {
                fill = Mathf.PingPong(timer, 1);
                timer += Time.deltaTime * fillSpeed;
                fillbar.fillAmount = fill;

                // exit conditions
                //if (Input.GetButtonDown("Fire1"))
                //{
                //    Debug.Log("Input!");
                //    break;
                //}

                yield return wait;
            }
            container.gameObject.SetActive(false);

            // damage calc mod based on fill
            int damage = (int)(controller.currentCombatant.combatantData.str * fill * damageMod) + controller.currentCombatant.combatantData.str;
            Debug.Log($"Damage: {damage}");

            // wait for animation or something
            yield return new WaitForSeconds(1f);

            // move player back
            controller.currentCombatant.transform.position = controller.currentCombatant.GetResetPos();

            // wait a beat
            yield return new WaitForSeconds(1f);

            // hide prefab stuff

            gameFinishedCallback?.Invoke();

        }
    }

}