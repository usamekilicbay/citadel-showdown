using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentTurn == GameManager.Turn.Player2)
        {
            SimulateAITurn();
        }
    }

    private void SimulateAITurn()
    {
        // Implement your AI logic here
        // Calculate throw direction, force, and attack type
        // ...

        // Apply the calculated parameters
        // rb.velocity = calculatedThrowDirection * calculatedThrowForce;

        // Switch turn
        GameManager.Instance.SwitchTurn();
    }
}
