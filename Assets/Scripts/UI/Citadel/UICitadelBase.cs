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

        public void UpdateHealth(float health)
        {
            healthText.SetText("Health: " + health);
        }

        public void UpdateStamina(float stamina)
        {
            staminaText.SetText("Stamina: " + stamina);
        }

        public void UpdateThrowForce(float throwForce, float minThrowForce, float maxThrowForce)
        {
            float throwForcePercentage = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce) * 100f;
            throwForceText.SetText($"{throwForcePercentage:F0} % \n Force");
        }

        public void UpdateThrowAngle(Vector2 throwDirection)
        {
            float throwAngle = Mathf.Rad2Deg * Mathf.Atan2(throwDirection.y, throwDirection.x);
            throwAngleText.SetText($"{throwAngle:F1} ° \n Angle");
        }
    }
}
