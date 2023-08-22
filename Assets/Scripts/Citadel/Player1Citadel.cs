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
                uiCitadel.ToggleIndicators(true);
                OnDragStart();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                uiCitadel.ToggleIndicators(false);
                OnDragEnd();
            }

            if (isDragging)
            {
                UpdateDrag();
                uiCitadel.UpdateThrowForce(throwForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce);
                uiCitadel.UpdateThrowAngle(throwDirection);
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
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);

            throwDirection = dragVector.normalized;
            throwForce = Mathf.Lerp(coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce, dragDistance / coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);

            var perc = (throwForce - coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce) / (coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce - coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce) * 100f;
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
