using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CitadelShowdown.UI.Citadel
{
    public abstract class UICitadelBase : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private GameObject attackButtonParent;
        [SerializeField] private Button regularAttackChooseButton;
        [SerializeField] private Button strongAttackChooseButton;
        [SerializeField] private Button wideAttackChooseButton;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI staminaText;
        [SerializeField] private TextMeshProUGUI throwForceText;
        [SerializeField] private TextMeshProUGUI throwAngleText;
        [SerializeField] private GameObject throwIndicator;

        protected CitadelBase citadel;

        private void Awake()
        {
            regularAttackChooseButton.onClick
                .AddListener(() => citadel.SetAttackType(AttackType.RegularAttack));
            strongAttackChooseButton.onClick
                .AddListener(() => citadel.SetAttackType(AttackType.StrongAttack));
            wideAttackChooseButton.onClick
                .AddListener(() => citadel.SetAttackType(AttackType.WideAttack));
        }

        public void UpdateHealthText(float health)
        {
            healthText.SetText("Health: " + health);
        }

        public void UpdateEnergyText(float stamina)
        {
            staminaText.SetText("Energy: " + stamina);
        }

        public void ToggleAttackSelectionButtons(bool toggle)
        {
            attackButtonParent.SetActive(toggle);
        }

        public void ToggleIndicators(bool isHidden)
        {
            throwIndicator.SetActive(isHidden);
        }

        public void UpdateThrowForceText(float throwForce)
        {
            throwForceText.SetText($"{throwForce:F0}%");
        }

        public void UpdateThrowAngleText(Vector2 throwDirection)
        {
            float throwAngle = Mathf.Rad2Deg * Mathf.Atan2(throwDirection.y, throwDirection.x);
            throwAngleText.SetText($"{throwAngle:F1}°");
        }
    }
}
