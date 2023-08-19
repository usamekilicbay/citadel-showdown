using UnityEngine;

namespace CitadelShowdown.Managers
{
    public class StaminaManager : MonoBehaviour
    {
        public float MaxStamina = 100f;
        public float StaminaRenewAmount = 20f;

        public static StaminaManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public bool CanAfford(AttackType attackType, float currentStamina)
        {
            float staminaCost = GetStaminaCost(attackType);

            return currentStamina >= staminaCost;
        }

        private float GetStaminaCost(AttackType attackType)
        {
            // Implement your logic to determine the stamina cost based on attack type
            // For simplicity, we're using constant values here.
            return attackType switch
            {
                AttackType.RegularAttack => 10f,
                AttackType.StrongAttack => 20f,
                AttackType.WideAttack => 30f,
                _ => 0f,
            };
        }

        public enum AttackType
        {
            RegularAttack,
            StrongAttack,
            WideAttack
        }
    }
}
