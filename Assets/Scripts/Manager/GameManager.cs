using CitadelShowdown.Citadel;
using CitadelShowdown.UI.Screen;
using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class GameManager : MonoBehaviour
    {
        public TurnType CurrentTurn { get; private set; }

        [SerializeField] MMF_Player mmfPlayerProjectile;
        public MMF_Player MMFPlayerProjectile => mmfPlayerProjectile;
        [SerializeField] MMF_Player mmfPlayer1;
        public MMF_Player MMFPlayer1 => mmfPlayer1;

        [SerializeField] MMF_Player mmfPlayer2;
        public MMF_Player MMFPlayer2 => mmfPlayer2;

        [SerializeField] private float maxPlayAreaWidth = 100f;
        public float MaxPlayAreaWidth => maxPlayAreaWidth;

        public Player1Citadel player1Citadel;
        public Player2Citadel player2Citadel;


        private UIManagerBase _uiManager;
        private ICurrencyManager _currencyManager;
        private LevelManager _levelManager;
        private ScoreManager _scoreManager;
        private ProgressManager _progressManager;
        private UIHomeScreen _uiHomeScreen;
        private UIGameScreen _uiGameScreen;
        private UIResultScreen _uiResultScreen;

        [Inject]
        public void Construct(UIManagerBase uiManager,
            ICurrencyManager currencyManager,
            ScoreManager scoreManager,
            LevelManager levelManager,
            ProgressManager progressManager,
            UIHomeScreen uiHomeScreen,
            UIGameScreen uiGameScreen,
            UIResultScreen uiResultScreen)
        {
            _uiManager = uiManager;
            _currencyManager = currencyManager;
            _levelManager = levelManager;
            _scoreManager = scoreManager;
            _progressManager = progressManager;
            _uiHomeScreen = uiHomeScreen;
            _uiGameScreen = uiGameScreen;
            _uiResultScreen = uiResultScreen;
        }

        private void Start()
        {
            CameraManager.Instance.SwitchCitadelCamera(CurrentTurn);

            player1Citadel = FindAnyObjectByType<Player1Citadel>();
            player2Citadel = FindAnyObjectByType<Player2Citadel>();
        }


        //TODO: Development purpose
        private void Update()
        {
            if (Input.GetMouseButton(2))
                Time.timeScale = 0.3f;
            else if (Input.GetMouseButtonUp(2))
                Time.timeScale = 1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _uiManager.ShowScreen(_uiGameScreen);
            }
        }

        public void SwitchTurn()
        {
            CurrentTurn = CurrentTurn == TurnType.Player1
                ? TurnType.Player2
                : TurnType.Player1;

            CameraManager.Instance.SwitchCitadelCamera(CurrentTurn);

            if (CurrentTurn == TurnType.Player2)
                player2Citadel.SimulateAITurn();
        }

        public void Renew()
        {
            player1Citadel.Renew();
            player2Citadel.Renew();
        }


        public void StartRun()
        {
            _progressManager.Renew();
            _scoreManager.Renew();
        }

        public async Task CompleteRun(bool isSuccessful = true)
        {
            await Task.Delay(2000);

            _uiManager.ShowScreen(_uiResultScreen);

            if (isSuccessful)
                await _progressManager.Complete(isSuccessful);

            await _levelManager.Complete(isSuccessful);
            await _scoreManager.Complete(isSuccessful);
        }
    }
}
