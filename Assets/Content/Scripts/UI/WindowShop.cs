using Assets.Content.Scripts.UI.Weak;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Content.Scripts.UI
{
    public class WindowShop : MonoBehaviour
    {
        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private WindowChest _windowChest;
        [SerializeField] private WindowItem _windowItem;
        [SerializeField] private List<UIChest> _chests;
        [SerializeField] private List<Items> _items;
        [SerializeField] private List<UiWeakItem> _uiWeakItems;
        [SerializeField] private TextMeshProUGUI _textNoMoney;

        [SerializeField] private float _scaleAmount = 1.2f;
        [SerializeField] private float _scaleDuration = 0.2f;

        private Vector3 originalScale;
        private Coroutine _coroutine;

        private void Start()
        {
            originalScale = _textNoMoney.transform.localScale;
        }
        public void BuyChest(int index)
        {
            if (MainUI.Instance.Money >= _chests[index].Price)
            {
                MainUI.Instance.ChangeMoney(-_chests[index].Price);
                _windowChest.AddChest(_chests[index]);
            }
            else
            {
                if (_coroutine == null)
                {
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }

        public void BuyItem(int index)
        {
            if (MainUI.Instance.Money >= _items[index].Price)
            {
                MainUI.Instance.ChangeMoney(-_items[index].Price);
                _windowItem.AddItem(_items[index]);
            }
            else
            {
                if (_coroutine == null)
                {
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }

        public void BuyEquip(int index)
        {
            if (MainUI.Instance.Money >= _uiWeakItems[index].Price)
            {
                MainUI.Instance.ChangeMoney(-_uiWeakItems[index].Price);
                _windowInventory.AddItemEquipment(_uiWeakItems[index]);
            }
            else
            {
                if (_coroutine == null)
                {
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }

        private IEnumerator ScaleAndDeactivate()
        {
            _textNoMoney.gameObject.SetActive(true);

            Vector3 targetScale = originalScale * _scaleAmount;
            yield return StartCoroutine(ScaleTo(targetScale, _scaleDuration));

            _textNoMoney.gameObject.SetActive(false);

            _textNoMoney.transform.localScale = originalScale;
            _coroutine = null;
        }

        private IEnumerator ScaleTo(Vector3 target, float duration)
        {
            Vector3 initialScale = _textNoMoney.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _textNoMoney.transform.localScale = Vector3.Lerp(initialScale, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _textNoMoney.transform.localScale = target; 
        }
    }
}