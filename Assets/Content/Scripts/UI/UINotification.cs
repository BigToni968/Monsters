using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Content.Scripts.UI
{
    public class UINotification : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }

        public void Set(string text)
        {
            Title.SetText(text);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}