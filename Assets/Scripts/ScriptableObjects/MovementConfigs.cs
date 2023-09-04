using UnityEngine;

namespace CitadelShowdown.Configs
{
    [CreateAssetMenu(fileName = "MovementConfigs", menuName = "Configs/Movement Configs")]
    public class MovementConfigs : ScriptableObject
    {
        [field: SerializeField]
        [Tooltip("The minimum drag distance for throwing projectile.")]
        public float MinDragDistance { get; private set; }

        [field: SerializeField]
        [Tooltip("The maximum drag distance for throwing projectile.")]
        public float MaxDragDistance { get; private set; }

        [field: SerializeField]
        [Tooltip("The minimum throw force for projectile movement.")]
        public float MinThrowForce { get; private set; }

        [field: SerializeField]
        [Tooltip("The maximum throw force for projectile movement.")]
        public float MaxThrowForce { get; private set; }
    }
}
