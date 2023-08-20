using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CitadelShowdown.Configs
{
    [CreateAssetMenu(fileName = "AttackConfigs", menuName = "Configs/Attack Configs")]
    public class AttackConfigs : ScriptableObject
    {
        [Tooltip("List of Attack objects representing attack types and their costs.")]
        [SerializeField] private List<Attack> attackTypes = new();

        // Getter property for the list of attack types and their costs.
        public List<Attack> AttackTypes => attackTypes;

        private void OnValidate()
        {
            // Get all values from the AttackType enum
            AttackType[] attackTypes = (AttackType[])Enum.GetValues(typeof(AttackType));

            // Ensure the list has the correct number of elements
            while (this.attackTypes.Count < attackTypes.Length)
            {
                this.attackTypes.Add(new Attack());
            }

            // Update attack types for each element in the list
            for (int i = 0; i < this.attackTypes.Count; i++)
            {
                this.attackTypes[i].attackType = attackTypes[i];
            }
        }
    }
}
