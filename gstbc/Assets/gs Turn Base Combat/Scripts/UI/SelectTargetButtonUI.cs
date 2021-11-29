using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace genaralskar.Battle
{
    [RequireComponent(typeof(Button))]
    public class SelectTargetButtonUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        public UnityAction<Combatant> TargetHovered;
        public UnityAction<Combatant> TargetSelected;

        public Combatant combatant { get; private set; }
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonPressed);
        }

        public void SetCombatant(Combatant newCombatant)
        {
            combatant = newCombatant;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TargetHovered?.Invoke(combatant);
        }

        public void OnSelect(BaseEventData eventData)
        {
            TargetHovered?.Invoke(combatant);
        }

        private void OnButtonPressed()
        {
            TargetSelected?.Invoke(combatant);
        }
    }
}