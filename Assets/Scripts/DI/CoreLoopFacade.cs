using CitadelShowdown.Managers;
using Zenject;

namespace CitadelShowdown.DI
{
    public class CoreLoopFacade
    {
        public GameManager GameManager { get; private set; }
        public ConfigurationManager ConfigurationManager { get; private set; }
        public AttackManager AttackManager { get; private set; }

        [Inject]
        public void Construct(GameManager gameManager,
            ConfigurationManager configurationManager,
            AttackManager attackManager)
        {
            GameManager = gameManager;
            ConfigurationManager = configurationManager;
            AttackManager = attackManager;
        }

        public TurnType CurrentTurn
            => GameManager.CurrentTurn;

        public void SwitchTurn()
        {
            GameManager.SwitchTurn();
        }
    }
}
