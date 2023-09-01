using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using CitadelShowdown.DI;
using MoreMountains.Feedbacks;
using System.Threading;
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

        private UIManagerBase _uiManager;
        private CameraManager _cameraManager;
        //private ICurrencyManager _currencyManager;
        //private LevelManager _levelManager;
        //private ScoreManager _scoreManager;
        //private ProgressManager _progressManager;
        private UIScreenFacade _screenFacade;
        private Player1Citadel _player1Citadel;
        private Player2Citadel _player2Citadel;
        private BattleManager _battleManager;

        [Inject]
        public void Construct(UIManagerBase uiManager,
            CameraManager cameraManager,
            //ICurrencyManager currencyManager,
            //ScoreManager scoreManager,
            //LevelManager levelManager,
            //ProgressManager progressManager,
            UIScreenFacade screenFacade,
            Player1Citadel player1Citadel,
            Player2Citadel player2Citadel,
            BattleManager battleManager)
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
            _battleManager = battleManager;
        }

        private void Start()
        {
            StartRun();
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
            _battleManager.StartBattle();
            //_progressManager.Renew();
            //_scoreManager.Renew();
        } 

        public async Task CompleteRun(bool isSuccessful = true, CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);

            _battleManager.EndBattle();

            _screenFacade.ShowResultScreen();

            //if (isSuccessful)
            //    await _progressManager.Complete(isSuccessful);

            //await _levelManager.Complete(isSuccessful);
            //await _scoreManager.Complete(isSuccessful);
        }
    }
}
