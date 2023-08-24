using Assets.Scripts.Common.Types;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class BattleManager
    {
        public Func<Task> OnTurnChange;

        public BattleState CurrentState { get; private set; }

        private CameraManager _cameraManager;

        [Inject]
        public void Construct(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public void StartBattle()
        {
            // Transition to the initial state, e.g., P1Select
            SwitchBattleState(BattleState.Player1);
        }

        public void EndBattle()
        {
            // Transition to the NotFighting state
            SwitchBattleState(BattleState.NotFighting);
        }

        public void SwitchBattleState(BattleState newState)
        {
            OnTurnChange();

            // Add logic here for any state-specific behavior
            CurrentState = newState;
            Debug.Log("Battle State: " + CurrentState);

            _cameraManager.SwitchCitadelCamera(CurrentState);
        }
    }
}
