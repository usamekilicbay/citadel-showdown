using System.Threading.Tasks;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private bool isDone = false;

    private void Update()
    {
        if (!isDone)
            CameraController.Instance.FollowTransform(transform, 0.01f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "AIController" || collision.gameObject.name == "Ground")
        {
            isDone = true;
            Task.Delay(1000);
            // Switch turn after a successful throw
            GameManager.Instance.SwitchTurn();
        }

        Destroy(gameObject);
    }
}
