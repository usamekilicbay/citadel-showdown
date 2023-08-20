using CitadelShowdown.DI;
using CitadelShowdown.UI.Citadel;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player1Citadel : CitadelBase
    {
        private Vector3 dragStartScreenPosition;

        [Inject]
        public void Construct(UIPlayer1Citadell uiPlayer1Citadel)
        {
            uiCitadel = uiPlayer1Citadel;
        }

        private void Update()
        {
            if (coreLoopFacade.CurrentTurn != TurnType.Player1)
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
                UpdateUI();
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


            var perc = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce) * 100f;
            Debug.Log($"%{perc}");

            trajectoryManager.UpdateTrajectoryLine(dragStartScreenPosition, currentPosition);
            trajectoryManager.UpdateTrajectory(throwDirection, perc, transform);
        }

        private void OnDragEnd()
        {
            isDragging = false;

            SpendEnergy((int)(throwForce * 2f)); // Adjust the stamina cost as needed

            ThrowProjectile();
            trajectoryManager.HideTrajectory();
        }
    }
}
