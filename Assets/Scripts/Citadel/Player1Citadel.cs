using Assets.Scripts.Common.Types;
using CitadelShowdown.Managers;
using CitadelShowdown.UI.Citadel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player1Citadel : CitadelBase
    {
        private InputManager _inputManager;

        [Inject]
        public void Construct(UIPlayer1Citadel uiPlayer1Citadel,
            InputManager inputManager)
        {
            uiCitadel = uiPlayer1Citadel;
            _inputManager = inputManager;
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
                UpdateDrag();
        }

        private void OnDragStart()
        {
            isDragging = true;
            _inputManager.SetDragStartPosition();
        }

        private void UpdateDrag()
        {
            var mouseCurrentPosition = _inputManager.GetMousePosition();
            var dragVector = _inputManager.GetDragVector();
            var dragPercentage = _inputManager.GetDragPercentage();

            if (dragPercentage < configurationManager.MinDragDistance)
                return;

            throwDirection = dragVector.normalized;
            Debug.Log($"Dir {throwDirection} ||| Perc: {dragPercentage}");

            trajectoryManager.UpdateTrajectoryLine(_inputManager.DragStartPosition, mouseCurrentPosition);
            trajectoryManager.UpdateTrajectory(throwDirection, dragPercentage, transform);
            uiCitadel.UpdateThrowForceText(dragPercentage);
            uiCitadel.UpdateThrowAngleText(throwDirection);
        }

        private void OnDragEnd()
        {
            trajectoryManager.HideTrajectory();
            throwForce = _inputManager.GetForceByDragPercentage();

            if (throwForce < 0f)
                return;

            isDragging = false;

            SpendEnergy((int)(throwForce * 2f)); // Adjust the stamina cost as needed

            ThrowProjectile();
            trajectoryManager.HideTrajectory();
        }
    }
}
