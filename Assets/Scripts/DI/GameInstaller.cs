using CitadelShowdown.Citadel;
using CitadelShowdown.Managers;
using CitadelShowdown.ProjectileNamespace;
using CitadelShowdown.UI.Citadel;
using CitadelShowdown.UI.Dialog;
using CitadelShowdown.UI.Screen;
using System.Collections.Generic;
using System.Linq;
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
                .Bind<TrajectoryManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            //Container
            //    .Bind<AudioManager>()
            //    .FromComponentInHierarchy()
            //    .AsSingle();

            Container
                .Bind<CameraManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            //Container
            //    .Bind<PlayerStatsManager>()
            //    .To<PlayerStatsManager>()
            //    .AsSingle();

            //Container
            //    .Bind<SaveManager>()
            //    .To<SaveManager>()
            //    .AsSingle();

            //Container
            //    .Bind<ICurrencyManager>()
            //    .To<CurrencyManager>()
            //.AsSingle();

            //Container
            //    .Bind<ScoreManager>()
            //    .To<ScoreManager>()
            //    .AsSingle();

            //Container
            //    .Bind<LevelManager>()
            //    .To<LevelManager>()
            //    .AsSingle();

            //Container
            //    .Bind<ProgressManager>()
            //    .To<ProgressManager>()
            //    .AsSingle();

            Container
                .Bind<CoreLoopFacade>()
                .AsSingle();

            Container
                .Bind<UIScreenFacade>()
                .AsSingle();

            Container
                .Bind<Player1Citadel>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<Player2Citadel>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<UIPlayer1Citadell>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<UIPlayer2Citadell>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<Projectile>()
                .FromComponentInHierarchy()
                .AsSingle();

            #region UI

            Container
                .Bind<UIManagerBase>()
                .FromComponentInHierarchy()
                .AsSingle();

            var screens = FindObjectsOfType<UIScreenBase>(true);
            foreach (var screen in screens)
                Container.Bind(screen.GetType()).FromComponentsInHierarchy().AsSingle();

            Container
                .Bind<List<UIScreenBase>>()
                .FromInstance(screens.ToList())
                .AsSingle();

            var dialogs = FindObjectsOfType<UIDialogBase>(true);
            foreach (var dialog in dialogs)
                Container.Bind(dialog.GetType()).FromComponentsInHierarchy().AsSingle();

            #endregion
        }
    }
}
