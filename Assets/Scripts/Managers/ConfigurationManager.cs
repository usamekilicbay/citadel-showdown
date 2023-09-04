using CitadelShowdown.Configs;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CitadelShowdown.Managers
{
    public class ConfigurationManager : MonoBehaviour
    {
        [field: SerializeField] public GameConfigs GameConfigs { get; private set; }
        [field: SerializeField] public AttackConfigs AttackConfigs { get; private set; }
        [field: SerializeField] public MovementConfigs MovementConfigs { get; private set; }

        public float MinDragDistance 
            => MovementConfigs.MinDragDistance;
        public float MaxDragDistance
            => MovementConfigs.MaxDragDistance;
        public float MinThrowForce
            => MovementConfigs.MinThrowForce;
        public float MaxThrowForce
            => MovementConfigs.MaxThrowForce;

        #region Config Load

        private const string _folderPath = "Assets/Configs"; // Specify the complete folder path containing the ScriptableObjects

        [ContextMenu("Load Configs")]
        private void LoadConfigs()
        {
            var configurationManagerType = typeof(ConfigurationManager);

            // Get all non-public instance fields with [SerializeField] attribute
            var fields = configurationManagerType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.GetCustomAttribute<SerializeField>() != null)
                .ToArray();

            foreach (var field in fields)
            {
                Debug.LogError(field.Name);

                var guids = AssetDatabase.FindAssets($"t:{field.FieldType.Name}", new string[] { _folderPath });

                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var config = AssetDatabase.LoadAssetAtPath(assetPath, field.FieldType);

                    field.SetValue(this, config);
                }
            }
        }

        #endregion
    }
}
