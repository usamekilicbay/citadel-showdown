using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CitadelShowdown.Configs
{
    [CreateAssetMenu(fileName = "AttackConfigs", menuName = "Configs/Attack Configs")]
    public class AttackConfigs : ScriptableObject
    {
        [Tooltip("List of Attack objects representing attack types and their costs.")]
        [field: SerializeField]
        public List<Attack> Attacks { get; private set; } = new();

        private void OnValidate()
        {
            // Get all values from the AttackType enum
            AttackType[] attackTypes = (AttackType[])Enum.GetValues(typeof(AttackType));

            // Create a temporary list to update the attack types
            List<Attack> tempAttacks = new List<Attack>();

            // Add or update attack types in the temporary list
            foreach (AttackType type in attackTypes)
            {
                // Check if the attack type exists in the current list
                Attack existingAttack = Attacks.FirstOrDefault(a => a.AttackType == type);

                if (existingAttack == null)
                {
                    // Create a new attack for the missing type
                    tempAttacks.Add(new Attack(type: type));
                }
                else
                {
                    tempAttacks.Add(existingAttack); // Add the existing attack to the temporary list
                }
            }

            // Clear the existing list and assign the temporary list to attacks
            Attacks.Clear();
            Attacks.AddRange(tempAttacks);
        }
    }
}
