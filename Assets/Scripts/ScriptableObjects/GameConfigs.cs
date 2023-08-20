using UnityEngine;

namespace CitadelShowdown.Configs
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Configs/Game Configs")]
    public class GameConfigs : ScriptableObject
    {
        [Tooltip("The maximum health for the player.")]
        [SerializeField] private float maxHealth;

        [Tooltip("The maximum stamina for the player.")]
        [SerializeField] private float maxEnergy;

        [Tooltip("The rate at which stamina renews (per second).")]
        [SerializeField] private float staminaRenewRate;

        [Tooltip("Attack configurations for different attack types.")]
        [SerializeField] private AttackConfigs attackConfigs;

        // Getter property for max health.
        public float MaxHealth => maxHealth;

        // Getter property for max stamina.
        public float MaxEnergy => maxEnergy;

        // Getter property for stamina renew rate.
        public float EnergyRenewRate => staminaRenewRate;

        // Getter property for attack configurations.
        public AttackConfigs AttackConfigs => attackConfigs;
    }
}
