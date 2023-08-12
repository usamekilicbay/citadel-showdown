using MoreMountains.Feedbacks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Turn { Player1, Player2 }
    public static GameManager Instance { get; private set; }
    public Turn CurrentTurn { get; private set; }

    [SerializeField] MMF_Player mmfPlayerProjectile;
    public MMF_Player MMFPlayerProjectile => mmfPlayerProjectile;
    [SerializeField] MMF_Player mmfPlayer1;
    public MMF_Player MMFPlayer1 => mmfPlayer1;

    [SerializeField] MMF_Player mmfPlayer2;
    public MMF_Player MMFPlayer2 => mmfPlayer2;

    [SerializeField] private float maxPlayAreaWidth = 100f;
    public float MaxPlayAreaWidth => maxPlayAreaWidth;

    private void Awake()
    {
        Instance = this;
        CurrentTurn = Turn.Player1;
        CameraController.Instance.SwitchCitadelCamera(CurrentTurn);
    }

    public void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == Turn.Player1
            ? Turn.Player2
            : Turn.Player1;

        Debug.Log(CurrentTurn);

        CameraController.Instance.SwitchCitadelCamera(CurrentTurn);

        if (CurrentTurn == Turn.Player2)
            Player2Citadel.Instance.SimulateAITurn();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        // Handle game over logic here
    }
}
