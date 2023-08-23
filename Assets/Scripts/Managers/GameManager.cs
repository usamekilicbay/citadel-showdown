using CitadelShowdown.Citadel;
using CitadelShowdown.DI;
using CitadelShowdown.UI.Screen;
using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] MMF_Player mmfPlayerProjectile;
        public MMF_Player MMFPlayerProjectile => mmfPlayerProjectile;
        [SerializeField] MMF_Player mmfPlayer1;
        public MMF_Player MMFPlayer1 => mmfPlayer1;

        [SerializeField] MMF_Player mmfPlayer2;
        public MMF_Player MMFPlayer2 => mmfPlayer2;

        [SerializeField] private float maxPlayAreaWidth = 100f;
        public float MaxPlayAreaWidth => maxPlayAreaWidth;

        public TurnType CurrentTurn { get; private set; }

        private UIManagerBase _uiManager;
        private CameraManager _cameraManager;
        //private ICurrencyManager _currencyManager;
        //private LevelManager _levelManager;
        //private ScoreManager _scoreManager;
        //private ProgressManager _progressManager;
        private UIScreenFacade _screenFacade;
        private Player1Citadel _player1Citadel;
        private Player2Citadel _player2Citadel;

        [Inject]
        public void Construct(UIManagerBase uiManager,
            CameraManager cameraManager,
            //ICurrencyManager currencyManager,
            //ScoreManager scoreManager,
            //LevelManager levelManager,
            //ProgressManager progressManager,
            UIScreenFacade screenFacade,
            Player1Citadel player1Citadel,
            Player2Citadel player2Citadel)
        {
            _uiManager = uiManager;
            _cameraManager = cameraManager;
            //_currencyManager = currencyManager;
            //_levelManager = levelManager;
            //_scoreManager = scoreManager;
            //_progressManager = progressManager;
            _screenFacade = screenFacade;
            _player1Citadel = player1Citadel;
            _player2Citadel = player2Citadel;
        }

        private void Start()
        {
            _cameraManager.SwitchCitadelCamera(CurrentTurn);
        }

        //TODO: Development purpose
        private void Update()
        {
            if (Input.GetMouseButton(2))
                Time.timeScale = 0.3f;
            else if (Input.GetMouseButtonUp(2))
                Time.timeScale = 1f;

            if (Input.GetKeyDown(KeyCode.Space))
                _screenFacade.ShowScreen(_screenFacade.GameScreen);
        }

        public void SwitchTurn()
        {
            CurrentTurn = CurrentTurn == TurnType.Player1
                ? TurnType.Player2
                : TurnType.Player1;

            _cameraManager.SwitchCitadelCamera(CurrentTurn);

            if (CurrentTurn == TurnType.Player2)
                _player2Citadel.SimulateAITurn();
        }

        public void Renew()
        {
            _player1Citadel.Renew();
            _player2Citadel.Renew();
        }


        public void StartRun()
        {
            _player1Citadel.Renew();
            _player2Citadel.Renew();
            _screenFacade.ShowGameScreen();
            //_progressManager.Renew();
            //_scoreManager.Renew();
        }

        public async Task CompleteRun(bool isSuccessful = true)
        {
            await Task.Delay(2000);

            _screenFacade.ShowResultScreen();


            //if (isSuccessful)
            //    await _progressManager.Complete(isSuccessful);

            //await _levelManager.Complete(isSuccessful);
            //await _scoreManager.Complete(isSuccessful);
        }
    }
}
