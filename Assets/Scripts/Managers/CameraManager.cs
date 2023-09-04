using Assets.Scripts.Common.Types;
using Cinemachine;
using UnityEngine;

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

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            projectileCamera = GameObject.Find("Projectile Camera").GetComponent<CinemachineVirtualCamera>();
            player1CitadelCamera = GameObject.Find("Player 1 Citadel Camera").GetComponent<CinemachineVirtualCamera>();
            player2CitadelCamera = GameObject.Find("Player 2 Citadel Camera").GetComponent<CinemachineVirtualCamera>();
        }

        public void SwitchToProjectileCamera()
        {
            player1CitadelCamera.gameObject.SetActive(false);
            player2CitadelCamera.gameObject.SetActive(false);
            projectileCamera.gameObject.SetActive(true);
        }

        public void SwitchCitadelCamera(BattleState battleState)
        {
            projectileCamera.gameObject.SetActive(false);
            player1CitadelCamera.gameObject.SetActive(battleState == BattleState.Player1);
            player2CitadelCamera.gameObject.SetActive(battleState == BattleState.Player2);
        }
    }
}
