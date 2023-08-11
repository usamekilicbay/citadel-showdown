using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Turn { Player1, Player2 }
    public static GameManager Instance { get; private set; }
    public Turn CurrentTurn { get; private set; }

    private void Awake()
    {
        Instance = this;
        CurrentTurn = Turn.Player1;
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Turn.Player1) ? Turn.Player2 : Turn.Player1;
    }
}
