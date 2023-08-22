using UnityEngine;

namespace CitadelShowdown.Configs
{
    [CreateAssetMenu(fileName = "MovementConfigs", menuName = "Configs/Movement Configs")]
    public class MovementConfigs : ScriptableObject
    {
        [Tooltip("The maximum drag distance for projectile movement.")]
        [SerializeField] private float maxDragDistance;

        [Tooltip("The maximum throw force for projectile movement.")]
        [SerializeField] private float maxThrowForce;

        [Tooltip("The minimum throw force for projectile movement.")]
        [SerializeField] private float minThrowForce;

        // Getter property for max drag distance.
        public float MaxDragDistance => maxDragDistance;

        // Getter property for max throw force.
        public float MaxThrowForce => maxThrowForce;

        // Getter property for min throw force.
        public float MinThrowForce => minThrowForce;
    }
}
