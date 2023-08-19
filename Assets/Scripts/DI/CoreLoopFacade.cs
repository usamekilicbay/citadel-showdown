using CitadelShowdown.Managers;
using Zenject;

namespace CitadelShowdown.DI
{
    public class CoreLoopFacade
    {
        public GameManager GameManager { get; private set; }
        public ConfigurationManager ConfigurationManager { get; private set; }

        [Inject]
        public void Construct(GameManager gameManager,
            ConfigurationManager configurationManager)
        {
            GameManager = gameManager;
            ConfigurationManager = configurationManager;
        }

        public TurnType GetCurrentTurn()
            => GameManager.CurrentTurn;
    }
}
