using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace genaralskar.Battle
{

    public class CombatantInfoDisplayUI : MonoBehaviour
    {
        private Combatant combatant;

        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI magicText;
        [SerializeField] Image magicBar;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] Image healthBar;

        public void SetCombatant(Combatant newCombatant)
        {
            combatant = newCombatant;

            if (nameText)
                nameText.text = combatant.combatantData.name;

            if (magicText)
                magicText.text = "5/5mp";

            if (healthText)
                healthText.text = $"{combatant.health.currentHealth}/{combatant.health.maxHealth}";

            if (healthBar)
                healthBar.fillAmount = combatant.health.HealthNormalized;
        }
    }
}