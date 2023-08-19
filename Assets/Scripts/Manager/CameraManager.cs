using Cinemachine;
using UnityEngine;
using static CitadelShowdown.Managers.GameManager;

namespace CitadelShowdown.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float zoomOutSize = 8f;
        [SerializeField] private float zoomInSize = 5f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float followSpeed = 5f;

        [SerializeField] CinemachineVirtualCamera projectileCamera;
        [SerializeField] CinemachineVirtualCamera player1CitadelCamera;
        [SerializeField] CinemachineVirtualCamera player2CitadelCamera;

        private static CameraManager instance;
        public static CameraManager Instance => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            projectileCamera = GameObject.Find("Projectile Camera").GetComponent<CinemachineVirtualCamera>();
            player1CitadelCamera = GameObject.Find("Player 1 Citadel Camera").GetComponent<CinemachineVirtualCamera>();
            player2CitadelCamera = GameObject.Find("Player 2 Citadel Camera").GetComponent<CinemachineVirtualCamera>();
        }

        public void SwitchToProjectileCamera(Transform target)
        {
            projectileCamera.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            projectileCamera.Follow = target;
            //projectileCamera.LookAt = target;

            player1CitadelCamera.gameObject.SetActive(false);
            player2CitadelCamera.gameObject.SetActive(false);
            projectileCamera.gameObject.SetActive(true);
        }

        public void SwitchCitadelCamera(TurnType turn)
        {
            projectileCamera.gameObject.SetActive(false);
            player1CitadelCamera.gameObject.SetActive(turn == TurnType.Player1);
            player2CitadelCamera.gameObject.SetActive(turn == TurnType.Player2);
        }
    }
}