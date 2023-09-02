using Assets.Scripts.Common.Types;
using CitadelShowdown.UI.Citadel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player1Citadel : CitadelBase
    {
        private Vector3 _dragStartScreenPosition;

        [Inject]
        public void Construct(UIPlayer1Citadel uiPlayer1Citadel)
        {
            uiCitadel = uiPlayer1Citadel;
        }

        private void Update()
        {
            if (actionType != ActionType.Attack 
                || coreLoopFacade.BattleState == BattleState.NotFighting)
                return;

            HandlePlayerInput();
        }

        public async override Task UpdateTurn(CancellationToken cancellationToken = default)
        {
            if (coreLoopFacade.BattleState != BattleState.Player1)
                return;

            //collider.enabled = coreLoopFacade.BattleState == BattleState.Player2;

            await base.UpdateTurn();
        }

        public async override Task SetAttackType(AttackType attackType, CancellationToken cancellationToken = default)
        {
            await Task.Delay(500);

            await base.SetAttackType(attackType);
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
                uiCitadel.UpdateThrowForceText(throwForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce);
                uiCitadel.UpdateThrowAngleText(throwDirection);
            }
        }

        private void OnDragStart()
        {
            isDragging = true;
            _dragStartScreenPosition = Input.mousePosition;
            _dragStartScreenPosition.z = 10; // Depth of the Camera
            _dragStartScreenPosition = Camera.main.ScreenToWorldPoint(_dragStartScreenPosition);
        }

        private void UpdateDrag()
        {
            Debug.Log("dRGGA");

            Vector3 currentPosition = Input.mousePosition;
            currentPosition.z = 10; // Depth of the Camera
            currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);

            // Check if the drag distance exceeds the threshold
            Vector3 dragVector = _dragStartScreenPosition - currentPosition;
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);

            throwDirection = dragVector.normalized;
            throwForce = Mathf.Lerp(coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce, dragDistance / coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);

            var perc = (throwForce - coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce) / (coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce - coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce) * 100f;
            //Debug.Log($"%{perc}");

            trajectoryManager.UpdateTrajectoryLine(_dragStartScreenPosition, currentPosition);
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
