using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace genaralskar.Battle
{
    [RequireComponent(typeof(Button))]
    public class CombatActionButtonUI : MonoBehaviour
    {
        public UnityAction<CombatAction> ButtonPressed;


        [SerializeField] TextMeshProUGUI text;
        [SerializeField] CombatAction action;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClicked);
        }

        public void PopulateButton(CombatAction newAction, BattleLogic controller)
        {
            action = newAction;
            text.text = action.ActionName;

            // change onclick to do something
        }

        private void ButtonClicked()
        {
            ButtonPressed?.Invoke(action);
        }
    }
}