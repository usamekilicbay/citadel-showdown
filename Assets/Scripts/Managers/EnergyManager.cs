using Assets.Scripts.Common.Types;
using CitadelShowdown.DI;
using System.Linq;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class EnergyManager
    {
        private CoreLoopFacade _coreLoopFacade;

        [Inject]
        public virtual void Construct(CoreLoopFacade coreLoopFacade)
        {
            _coreLoopFacade = coreLoopFacade;
        }

        public bool CanAfford(AttackType attackType, float currentEnergy)
        {
            var staminaCost = GetEnergyCost(attackType);

            return currentEnergy >= staminaCost;
        }

        private float GetEnergyCost(AttackType attackType)
        {
            // Implement your logic to determine the stamina cost based on attack type
            // For simplicity, we're using constant values here.
            return _coreLoopFacade.ConfigurationManager
                .GameConfigs.AttackConfigs.AttackTypes.Single(x => x.attackType == attackType).energyCost;
        }
    }
}
