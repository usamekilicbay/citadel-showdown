using Assets.Scripts.Common.Types;
using CitadelShowdown.Managers;
using Zenject;

namespace CitadelShowdown.DI
{
    public class CoreLoopFacade
    {
        public GameManager GameManager { get; private set; }
        public ConfigurationManager ConfigurationManager { get; private set; }
        public AttackManager AttackManager { get; private set; }
        public BattleManager BattleManager { get; private set; }

        [Inject]
        public void Construct(GameManager gameManager,
            ConfigurationManager configurationManager,
            AttackManager attackManager,
            BattleManager battleManager)
        {
            GameManager = gameManager;
            ConfigurationManager = configurationManager;
            AttackManager = attackManager;
            BattleManager = battleManager;
        }

        public BattleState BattleState
            => BattleManager.CurrentState;

        public void SwitchTurn()
        {
            var state = BattleState == BattleState.Player1
                ? BattleState.Player2
                : BattleState.Player1;

            BattleManager.SwitchBattleState(state);
        }
    }
}
