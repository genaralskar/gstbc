using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace genaralskar.Battle
{
    public class ActionSelectorUI : MonoBehaviour
    {
        [SerializeField] BattleLogic battleController;
        [Header("Action Selector")]
        [SerializeField] Button attackButton;
        [SerializeField] Button itemButton;
        [SerializeField] Button actionsBackButton;
        [SerializeField] Button runButton;

        [SerializeField] GameObject actionsPanel;
        [SerializeField] List<CombatActionButtonUI> actionButtons;

        [SerializeField] GameObject runPanel;

        // 0 = select type, 1 = select action, 2 = select target
        private ActionType selectedActionType;

        [Header("Target Selector")]
        [SerializeField] List<SelectTargetButtonUI> selectTargetButtons;
        [SerializeField] Button targetSelectBackButton;
        [SerializeField] CombatantInfoDisplayUI targetInfoPanel;

        private int selectionStage = 0;
        private void Awake()
        {
            if (battleController == null)
                battleController = FindObjectOfType<BattleLogic>();

            attackButton.onClick.AddListener(OnAttackButton);
            itemButton.onClick.AddListener(OnItemButton);
            runButton.onClick.AddListener(OnRunButton);
            actionsBackButton.onClick.AddListener(OnBackButton);
            targetSelectBackButton.onClick.AddListener(OnBackButton);

            foreach (var b in actionButtons)
            {
                b.ButtonPressed = OnActionSelected;
            }

            foreach (var b in selectTargetButtons)
            {
                b.TargetHovered = OnTargetHover;
                b.TargetSelected = OnTargetSelected;
            }
        }

        private void Start()
        {
            battleController.PlayerSelectActionStep += OnPlayerSelectAction;
        }


        #region Inputs

        public void ShowActionSelector()
        {
            gameObject.SetActive(true);
            DisplayTypeSelection();
        }

        public void HideActionSelector()
        {
            gameObject.SetActive(false);
        }

        private void OnPlayerSelectAction()
        {
            ShowActionSelector();
        }

        private void OnAttackButton()
        {
            GetSortedActions(ActionType.Attack);
        }

        private void OnItemButton()
        {
            GetSortedActions(ActionType.Item);
        }

        private void OnBackButton()
        {
            // if selecting attack
            if (selectionStage == 1)
                DisplayTypeSelection();
            // if selecting target
            if (selectionStage == 2)
            {
                HideTargetSelection();
                actionsPanel.gameObject.SetActive(true);
                GetSortedActions(selectedActionType);
            }
        }

        private void OnRunButton()
        {

        }

        #endregion Inputs


        #region Action Selection

        private void GetSortedActions(ActionType actionType)
        {
            selectedActionType = actionType;
            List<CombatAction> actions = new List<CombatAction>();
            // populate list with attack options.
            foreach (var action in battleController.currentCombatant.combatantData.combatActions)
            {
                if (action.ActionType == actionType)
                {
                    actions.Add(action);
                }
            }

            DisplayActions(actions);
        }

        private void DisplayActions(List<CombatAction> actions)
        {
            selectionStage = 1;

            attackButton.interactable = false;
            itemButton.interactable = false;
            runButton.interactable = false;
            actionsPanel.gameObject.SetActive(true);

            int i = 0;
            foreach (var a in actions)
            {
                actionButtons[i].PopulateButton(a, battleController);
                actionButtons[i].gameObject.SetActive(true);
                i++;
            }
            // disable the rest
            for (; i < actionButtons.Count; i++)
            {
                actionButtons[i].gameObject.SetActive(false);
            }

            if (actionButtons.Count > 0)
                EventSystem.current.SetSelectedGameObject(actionButtons[0].gameObject);
            else
                EventSystem.current.SetSelectedGameObject(actionsBackButton.gameObject);
        }

        private void DisplayTypeSelection()
        {
            selectionStage = 0;

            attackButton.interactable = true;
            itemButton.interactable = true;
            runButton.interactable = true;
            actionsPanel.gameObject.SetActive(false);

            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }

        private void OnActionSelected(CombatAction action)
        {
            battleController.SetCombatAction(action);
            // move to select target stage
            DisplayTargetSelection();
        }
        #endregion Action Selection


        #region Target Selection

        private List<Combatant> GetActionTargets()
        {
            List<Combatant> targetCombatants = new List<Combatant>();
            // for each combatant, check if they are considered and enemy to the current combatant
            Debug.Log($"Number of current combatants: {battleController.combatants.Count}");
            foreach(Combatant c in battleController.combatants)
            {
                if(battleController.currentCombatant.combatantData.IsEnemyType(c.combatantData.combatantTypes))
                {
                    targetCombatants.Add(c);
                }
            }

            // check each remaining combatant if they match the action's target type(s). If target types is null, allow everything
            if(battleController.selectedAction.TargetTypes.Count > 0)
            {
                foreach(Combatant c in targetCombatants)
                {
                    // implement per action type targeting
                    // like an attack that only tagets undead
                }
            }

            return targetCombatants;
        }

        private void DisplayTargetSelection()
        {
            // hide action display panel
            actionsPanel.gameObject.SetActive(false);

            selectionStage = 2;

            List<Combatant> targetCombatants = GetActionTargets();

            int i = 0;
            Debug.Log($"Number of target combatants: {targetCombatants.Count}");

            foreach (Combatant c in targetCombatants)
            {
                // add the combatant to the panel script
                selectTargetButtons[i].SetCombatant(c);

                // place the panels at the combatants positions
                Vector2 newPos = Camera.main.WorldToScreenPoint(c.transform.position);
                selectTargetButtons[i].transform.position = newPos;

                // enable target selecter panels for each combatant
                selectTargetButtons[i].gameObject.SetActive(true);
                i++;
            }

            // disable the rest
            for (; i < selectTargetButtons.Count; i++)
            {
                selectTargetButtons[i].gameObject.SetActive(false);
            }

            // display and move the back button
            targetSelectBackButton.gameObject.SetActive(true);
            Vector2 newPos2 = Camera.main.WorldToScreenPoint(battleController.currentCombatant.transform.position);
            targetSelectBackButton.transform.position = newPos2;

            // select the first target or back button
            // if no targets, select back button
            if (targetCombatants.Count == 0)
            {
                // display something saying you can't target that!
                Debug.LogError("No Combatants that match that action's type could be found!");
                EventSystem.current.SetSelectedGameObject(targetSelectBackButton.gameObject);
            }
            // else select the first target
            else
                EventSystem.current.SetSelectedGameObject(selectTargetButtons[0].gameObject);

        }

        private void HideTargetSelection()
        {
            foreach(var b in selectTargetButtons)
            {
                b.gameObject.SetActive(false);
            }
            targetSelectBackButton.gameObject.SetActive(false);
        }

        private void OnTargetHover(Combatant combatant)
        {
            targetInfoPanel.SetCombatant(combatant);
            // move pointer arrow over target
        }

        private void OnTargetSelected(Combatant combatant)
        {
            HideTargetSelection();
            targetInfoPanel.SetCombatant(combatant);

            // add a confirm screen in here
            HideActionSelector();

            battleController.SelectTarget(combatant);
        }

        #endregion Target Selection
    }
}