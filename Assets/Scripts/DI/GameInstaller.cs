using CitadelShowdown.Managers;
using CitadelShowdown.UI.Dialog;
using CitadelShowdown.UI.Screen;
using Zenject;

namespace CitadelShowdown.DI
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<GameManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<ConfigurationManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<AudioManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<CameraManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<PlayerStatsManager>()
                .To<PlayerStatsManager>()
                .AsSingle();

            Container
                .Bind<SaveManager>()
                .To<SaveManager>()
                .AsSingle();

            Container
                .Bind<ICurrencyManager>()
                .To<CurrencyManager>()
            .AsSingle();

            Container
                .Bind<ScoreManager>()
                .To<ScoreManager>()
                .AsSingle();

            Container
                .Bind<LevelManager>()
                .To<LevelManager>()
                .AsSingle();

            Container
                .Bind<ProgressManager>()
                .To<ProgressManager>()
                .AsSingle();

            Container
                .Bind<CoreLoopFacade>()
                .AsSingle();

            #region UI

            Container
                .Bind<UIManagerBase>()
                .FromComponentInHierarchy()
                .AsSingle();

            var screens = FindObjectsOfType<UIScreenBase>(true);
            foreach (var screen in screens)
                Container.Bind(screen.GetType()).FromComponentsInHierarchy().AsSingle();

            var dialogs = FindObjectsOfType<UIDialogBase>(true);
            foreach (var dialog in dialogs)
                Container.Bind(dialog.GetType()).FromComponentsInHierarchy().AsSingle();

            #endregion
        }
    }
}