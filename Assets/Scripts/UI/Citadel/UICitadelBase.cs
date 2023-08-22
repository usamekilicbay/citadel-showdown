using UnityEngine;
using TMPro;

namespace CitadelShowdown.UI.Citadel
{
    public abstract class UICitadelBase : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI staminaText;
        [SerializeField] private TextMeshProUGUI throwForceText;
        [SerializeField] private TextMeshProUGUI throwAngleText;
        [SerializeField] private GameObject throwIndicator;

        public void UpdateHealthText(float health)
        {
            healthText.SetText("Health: " + health);
        }

        public void UpdateEnergyText(float stamina)
        {
            staminaText.SetText("Energy: " + stamina);
        }

        public void ToggleIndicators(bool isHidden)
        {
            throwIndicator.SetActive(isHidden);
        }

        public void UpdateThrowForce(float throwForce, float minThrowForce, float maxThrowForce)
        {
            float throwForcePercentage = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce) * 100f;
            throwForceText.SetText($"{throwForcePercentage:F0}%");
        }

        public void UpdateThrowAngle(Vector2 throwDirection)
        {
            float throwAngle = Mathf.Rad2Deg * Mathf.Atan2(throwDirection.y, throwDirection.x);
            throwAngleText.SetText($"{throwAngle:F1}°");
        }
    }
}
