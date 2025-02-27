using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.Others
{
    public class OpenChestEquip : MonoBehaviour
    {
        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private WindowChest _windowChest;
        [SerializeField] private UIChestItemEquip[] _commonItems;
        [SerializeField] private UIChestItemEquip[] _rareItems;
        [SerializeField] private UIChestItemEquip[] _epicItems;
        [SerializeField] private RectTransform _panelMove;
        [SerializeField] private Button _button;
        [SerializeField] private Button _buttonExit;
        [SerializeField] private RectTransform _line;
        [SerializeField] private float _minSpeed = 1.0f;
        [SerializeField] private float _maxSpeed = 5.0f;
        [SerializeField] private float _stopDuration = 2.0f;
        [SerializeField] private LayerMask _layerMask;


        private List<UIChestItemEquip> _items = new List<UIChestItemEquip>();

        private UIChest _chest;

        private Vector2 _initialPanelPosition;

        private void Start()
        {

            _initialPanelPosition = _panelMove.anchoredPosition;
        }
        public void OpenCase()
        {
            _windowChest.RemoveChest(_chest);
            StartCoroutine(OpenCaseCoroutine());
            _button.interactable = false;
            _buttonExit.interactable = false;
        }

        private IEnumerator OpenCaseCoroutine()
        {

            float speed = Random.Range(_minSpeed, _maxSpeed);
            float panelWidth = _panelMove.rect.width;
            float targetPosition = _panelMove.anchoredPosition.x - panelWidth;

            while (_panelMove.anchoredPosition.x > targetPosition)
            {
                _panelMove.anchoredPosition = new Vector2(_panelMove.anchoredPosition.x - speed * Time.deltaTime, _panelMove.anchoredPosition.y);
                yield return null;
            }

            yield return new WaitForSeconds(_stopDuration);

            RewardPlayer();
            _buttonExit.interactable = true;
        }

        public void SetupItems(UIChest chest)
        {
            _panelMove.anchoredPosition = _initialPanelPosition;
            _chest = chest;
            _button.interactable = true;
            for (int i = 0; i < _items.Count; i++)
            {
                Destroy(_items[i].gameObject);
            }
            _items.Clear();

            int itemCount = 15;
            for (int i = 0; i < itemCount; i++)
            {
                UIChestItemEquip randomItem = GetRandomItem();
                UIChestItemEquip itemInstance = Instantiate(randomItem, _panelMove);
                _items.Add(itemInstance);
            }
        }

        private UIChestItemEquip GetRandomItem()
        {
            float roll = Random.Range(0f, 1f);
            if (roll < 0.75f)
            {
                return _commonItems[Random.Range(0, _commonItems.Length)];
            }
            else if (roll < 0.9f)
            {
                return _rareItems[Random.Range(0, _rareItems.Length)];
            }
            else
            {
                return _epicItems[Random.Range(0, _epicItems.Length)];
            }
        }

        private void RewardPlayer()
        {
            Debug.Log("Позиция линии: " + _line.position);
            Collider[] colliders = Physics.OverlapSphere(_line.position, 2f, _layerMask);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Debug.Log("Объект найден: " + colliders[i].name);

                    if (colliders[i].TryGetComponent<UIChestItemEquip>(out UIChestItemEquip item))
                    {
                        _windowInventory.AddItemEquipment(item.GiveGift());
                    }
                    else
                    {
                        Debug.Log("Компонент UIChestItemEquip не найден.");
                    }
                }

            }
            else
            {
                Debug.Log("Ничего не найдено под линией.");
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_line.position, 2f);
        }
    }
}