using UnityEngine;

namespace genaralskar.Battle
{
    public class Health
    {
        public int currentHealth { get; private set; }
        public int maxHealth { get; private set; }
        public float HealthNormalized => currentHealth / maxHealth;

        public void ModifyHealth(int amount)
        {
            currentHealth += amount;

            if(currentHealth <= 0)
            {
                Death();
            }
        }

        private void Death()
        {

        }
    }
}