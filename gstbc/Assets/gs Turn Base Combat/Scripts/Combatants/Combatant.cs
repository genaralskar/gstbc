using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace genaralskar.Battle
{
    public class Combatant : MonoBehaviour
    {
        public CombatantData combatantData;
        public Health health;

        [Tooltip("Where an enemy will stand when attacking this combatant")]
        public Transform combatantAttackPos;

        [Tooltip("How far back to move this combatant when they are attacking")]
        public float attackOffset = -1;


        private Vector3 startPos;
        private void Start()
        {
            startPos = transform.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">Position of the target to attack</param>
        /// <returns>Position to move the combatant so they stand in front of the target</returns>
        public Vector3 GetAttackingPos(Combatant combatant)
        {
            Vector3 pos = combatant.transform.position;
            pos.x += attackOffset;
            pos += combatantAttackPos.position;
            return pos;
        }

        public Vector3 GetResetPos()
        {
            return startPos;
        }
    }
}