using Assets.Scripts.Common.Types;

using UnityEngine;

namespace CitadelShowdown.Citadel
{
    [System.Serializable]
    public class Attack
    {
        [SerializeField]
        [Tooltip("The type of this attack.")]
        private AttackType attackType;

        [SerializeField]
        [Tooltip("The energy cost associated with using this attack.")]
        private int energyCost;

        [SerializeField]
        [Tooltip("The amount of damage this attack deals.")]
        private int damage;

        // Getter for the attack type.
        public AttackType AttackType => attackType;

        // Getter for the energy cost.
        public int EnergyCost => energyCost;

        // Getter for the damage value.
        public int Damage => damage;

        // Constructor to initialize the fields.
        public Attack(AttackType type, int cost = default, int dmg = default)
        {
            attackType = type;
            energyCost = cost;
            damage = dmg;
        }
    }
}
