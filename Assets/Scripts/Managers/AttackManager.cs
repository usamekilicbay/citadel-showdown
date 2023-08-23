using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using System.Linq;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class AttackManager
    {
        private ConfigurationManager _configurationManager;

        [Inject]
        public void Construct(ConfigurationManager configuration)
        {
            _configurationManager = configuration;
        }

        public Attack GetAttack(AttackType attackType) 
            => _configurationManager.AttackConfigs.Attacks.Single(x => x.AttackType == attackType);
    }
}
