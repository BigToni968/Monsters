using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI
{
    public class WindowInfoUnit : MonoBehaviour
    {
        [SerializeField] private Transform _windowInfo;
        [SerializeField] private Transform _health;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Slider _healthSlider;


        private void Update()
        {
            _windowInfo.LookAt(Camera.main.transform);
        }

        public void SetName(string name)
        {
            _nameText.SetText(name);
        }

        public void SetHealth(float currentHealth, float maxHealth)
        {
            _healthText.SetText($"{currentHealth}/{maxHealth}");
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHealth;
        }

        public void ShowHealth()
        {
            _health.gameObject.SetActive(true);
        }

        public void HideHealth()
        {
            _health.gameObject.SetActive(false);
        }
        
        public void ShowCanvas()
        {
            _windowInfo.gameObject.SetActive(true);
        }

        public void HideCanvas()
        {
            _windowInfo.gameObject.SetActive(false);
        }
    }
}