using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRenewAmount = 20f;

    private float player1Stamina;
    private float player2Stamina;

    public static StaminaManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        player1Stamina = maxStamina;
        player2Stamina = maxStamina;
    }

    public void RenewStamina(GameManager.Turn turn)
    {
        if (turn == GameManager.Turn.Player1)
            player1Stamina += staminaRenewAmount;
        else
            player2Stamina += staminaRenewAmount;
    }

    public void ConsumeStamina(GameManager.Turn turn, AttackType attackType)
    {
        float staminaCost = GetStaminaCost(attackType);

        if (turn == GameManager.Turn.Player1)
            player1Stamina -= staminaCost;
        else
            player2Stamina += staminaCost;
    }

    public bool CanAfford(GameManager.Turn turn, AttackType attackType)
    {
        float staminaCost = GetStaminaCost(attackType);

        if (turn == GameManager.Turn.Player1)
            return player1Stamina >= staminaCost;
        else
            return player2Stamina >= staminaCost;
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

