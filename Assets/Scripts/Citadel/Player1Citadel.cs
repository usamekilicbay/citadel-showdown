using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using CitadelShowdown.UI.Citadel;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player1Citadel : CitadelBase
    {
        private Vector3 dragStartScreenPosition;

        private CoreLoopFacade _loopFacade;

        public override void Construct(CoreLoopFacade coreLoopFacade)
        {
            base.Construct(coreLoopFacade);
        }

        protected override void Start()
        {
            base.Start();
            uiCitadel = FindAnyObjectByType<UIPlayer1Citadel>();
        }

        private void Update()
        {
            if (coreLoopFacade.GameManager.CurrentTurn != TurnType.Player1)
            {
                return;
            }

            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnDragStart();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnDragEnd();
            }

            if (isDragging)
            {
                UpdateDrag();
                UpdateTrajectory();
                UpdateUI();
            }
            else
            {
                HideTrajectory();
            }
        }

        private void OnDragStart()
        {
            isDragging = true;
            dragStartScreenPosition = Input.mousePosition;
            dragStartScreenPosition.z = 10; // Depth of the Camera
            dragStartScreenPosition = Camera.main.ScreenToWorldPoint(dragStartScreenPosition);
            SpawnProjectile();
        }

        private void UpdateDrag()
        {
            Vector3 currentPosition = Input.mousePosition;
            currentPosition.z = 10; // Depth of the Camera
            currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);

            // Check if the drag distance exceeds the threshold
            Vector3 dragVector = dragStartScreenPosition - currentPosition;
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);

            throwDirection = dragVector.normalized;
            throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, dragDistance / maxDragDistance);

            UpdateTrajectoryLine(dragStartScreenPosition, currentPosition);
        }

        private void OnDragEnd()
        {
            isDragging = false;

            SpendStamina((int)(throwForce * 2f)); // Adjust the stamina cost as needed

            trajectoryLineRenderer.enabled = false;

            ThrowProjectile();
        }
    }
}
