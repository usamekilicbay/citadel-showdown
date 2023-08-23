
using CitadelShowdown.Managers;
using CitadelShowdown.UI.Screen;
using Zenject;

namespace CitadelShowdown.DI
{
    public class UIScreenFacade
    {
        public UIManagerBase UIManager { get; private set; }
        //public UIHomeScreen HomeScreen { get; private set; }
        public UIGameScreen GameScreen { get; private set; }
        public UIResultScreen ResultScreen { get; private set; }
        public GameManager GameManager { get; private set; }

        [Inject]
        public void Construct(UIManagerBase uiManager,
            //UIHomeScreen homeScreen,
            UIGameScreen gameScreen,
            UIResultScreen resultScreen,
            GameManager gameManager)
        {
            UIManager = uiManager;
            //HomeScreen = homeScreen;
            GameScreen = gameScreen;
            ResultScreen = resultScreen;
            GameManager = gameManager;
        }

        public void ShowScreen(UIScreenBase targetScreen)
        {
            UIManager.ShowScreen(targetScreen);
        }

        public void ShowGameScreen()
        {
            UIManager.ShowScreen(GameScreen);
        }
        
        public void ShowResultScreen()
        {
            UIManager.ShowScreen(ResultScreen);
        }
    }
}
