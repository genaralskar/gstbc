using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{

    public class BattleLogic : MonoBehaviour
    {
        public UnityAction PlayerSelectActionStep;

        private UnityAction ActionSelectedCallback;
        private UnityAction ActionGameEndedCallback;

        [Header("CRUMPLE ZONE")]
        public List<CombatantData> testcombats;

        [Header("Positions")]
        public Transform[] playersPositions;
        public Transform[] enemyPositions;

        [Header("Combatants")]
        public CombatantData playerCombatant;

        [Header("UI")]
        [SerializeField] GameObject pointerContainer;

        public int currentCombatantIndex { get; private set; }

        public Combatant currentCombatant { get; private set; }
        public Combatant combatantTarget { get; private set; }
        public CombatAction selectedAction { get; private set; }

        private List<Combatant> spawnedEnemies = new List<Combatant>();
        private List<Combatant> spawnedParties = new List<Combatant>();
        public List<Combatant> combatants { get; private set; }

        private Dictionary<CombatAction, GameObject> combatActionPrefabs = new Dictionary<CombatAction, GameObject>();

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Start()
        {
            StartBattle(new BattleData(testcombats));
        }

        public void StartBattle(BattleData data)
        {
            combatants = new List<Combatant>();

            // sort by speed
            data.combatants.Sort((c1, c2) => c2.spd.CompareTo(c1.spd));


            // spawn combatant prefabs, assign them proper datas
            foreach (var cb in data.combatants)
            {
                GameObject newObj = Instantiate(cb.prefab);
                Combatant cobj = newObj.GetComponent<Combatant>();
                combatants.Add(cobj);

                if (cb.isPlayerControlled)
                {
                    spawnedParties.Add(cobj);
                    cobj.transform.position = playersPositions[spawnedParties.Count - 1].position;
                }
                else
                {
                    spawnedEnemies.Add(cobj);
                    cobj.transform.position = enemyPositions[spawnedEnemies.Count - 1].position;
                }
            }


            // spawn combat action prefabs            
            foreach (var cb in data.combatants)
            {
                foreach (var action in cb.combatActions)
                {
                    // if we've already spawned this action prefab
                    if (action.Prefab == null || combatActionPrefabs.ContainsKey(action)) continue;
                    GameObject obj = Instantiate(action.Prefab);
                    combatActionPrefabs.Add(action, obj);
                }
            }

            currentCombatantIndex = 0;
            SelectNewCombatant(combatants[0]);

            CombatantTurn();
        }

        private void CombatantTurn()
        {
            if (currentCombatant.combatantData.isPlayerControlled)
            {
                PlayerSelectAction();
            }
            else
            {
                EnemySelectAction();
            }
        }

        private void SelectNewCombatant(Combatant newCombatant)
        {
            currentCombatant = newCombatant;
        }

        private void NextCombatant()
        {
            currentCombatantIndex = (currentCombatantIndex + 1) % (combatants.Count);
            Debug.Log($"index: {currentCombatantIndex}");
            SelectNewCombatant(combatants[currentCombatantIndex]);
            CombatantTurn();
        }

        private void PlayerSelectAction()
        {
            // display marker on current combatant
            // show ui (send action call probably) allowing the user to select their action (attack, item, run)
            PlayerSelectActionStep?.Invoke();
        }
        private void PlayerSelectTarget()
        {
            // display an arrow over the currently selected target
            // allow the user to change which target is selected
        }

        public void SelectTarget(Combatant newTarget)
        {
            combatantTarget = newTarget;
            Debug.Log($"Combatant {currentCombatant.name} targets {combatantTarget.name} with {selectedAction.name}!");
            // Start the minigame!
            // activate prefab?
            selectedAction.UseAction(this, ActionGameEndedCallback = () => OnBattleGameEnded());
        }

        private void EnemySelectAction()
        {
            // randomly select from their current actions
            SetCombatAction(currentCombatant.combatantData.combatActions[0]);
            // randomly select a target to attack

            // maybe refer to their combat data to allow for custom logic per enemy
            List<Combatant> targetCombatants = new List<Combatant>();
            // for each combatant, check if they are considered and enemy to the current combatant
            Debug.Log($"Number of current combatants: {combatants.Count}");
            foreach (Combatant c in combatants)
            {
                if (currentCombatant.combatantData.IsEnemyType(c.combatantData.combatantTypes))
                {
                    targetCombatants.Add(c);
                }
            }

            // check each remaining combatant if they match the action's target type(s). If target types is null, allow everything
            if (selectedAction.TargetTypes.Count > 0)
            {
                foreach (Combatant c in targetCombatants)
                {
                    // implement per action type targeting
                    // like an attack that only tagets undead
                }
            }

            SelectTarget(targetCombatants[Random.Range(0, targetCombatants.Count)]);
        }

        private void OnBattleGameEnded()
        {
            // select next combatant, rince and repeat
            Debug.Log("Game Over!");
            NextCombatant();
        }

        public void SetCombatAction(CombatAction newAction)
        {
            selectedAction = newAction;
        }

        public GameObject GetActionPrefab(CombatAction action)
        {
            GameObject obj = combatActionPrefabs[action];
            Debug.Log($"Getting Prefab for action {action}, obj is {obj}");
            return combatActionPrefabs[action];
        }

    }

    public enum BattleState
    {
        SelectAction, BattleGame, DamageCalc, PlayerWon, EnemyWon
    }

    public class BattleData
    {
        // enemies
        public List<CombatantData> combatants = new List<CombatantData>();

        // get player data

        public BattleData(List<CombatantData> combatants)
        {
            this.combatants = combatants;
        }
    }

}
