using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AIController")
        {
            // Switch turn after a successful throw
            GameManager.Instance.SwitchTurn();
        }
    }
}
