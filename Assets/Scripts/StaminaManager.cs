using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRenewAmount = 20f;

    private Dictionary<GameManager.Turn, Dictionary<AttackType, float>> playerStamina;

    public static StaminaManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        InitializeStamina();
    }

    private void InitializeStamina()
    {
        playerStamina = new Dictionary<GameManager.Turn, Dictionary<AttackType, float>>();

        foreach (GameManager.Turn turn in System.Enum.GetValues(typeof(GameManager.Turn)))
        {
            playerStamina[turn] = new Dictionary<AttackType, float>();

            foreach (AttackType attackType in System.Enum.GetValues(typeof(AttackType)))
            {
                playerStamina[turn][attackType] = maxStamina;
            }
        }
    }

    public void RenewStamina(GameManager.Turn turn)
    {
        foreach (AttackType attackType in System.Enum.GetValues(typeof(AttackType)))
        {
            playerStamina[turn][attackType] = Mathf.Clamp(playerStamina[turn][attackType] + staminaRenewAmount, 0f, maxStamina);
        }
    }

    public void ConsumeStamina(GameManager.Turn turn, AttackType attackType)
    {
        float staminaCost = GetStaminaCost(attackType);
        playerStamina[turn][attackType] = Mathf.Clamp(playerStamina[turn][attackType] - staminaCost, 0f, maxStamina);
    }

    public float GetStamina(GameManager.Turn turn, AttackType attackType)
    {
        return playerStamina[turn][attackType];
    }

    private float GetStaminaCost(AttackType attackType)
    {
        // Implement your logic to determine the stamina cost based on attack type
        // For simplicity, we're using constant values here.
        switch (attackType)
        {
            case AttackType.Attack1:
                return 10f;
            case AttackType.Attack2:
                return 20f;
            case AttackType.Attack3:
                return 30f;
            default:
                return 0f;
        }
    }

    public enum AttackType
    {
        None,
        Attack1,
        Attack2,
        Attack3
    }
}

