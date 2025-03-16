using Assets.Content.Scripts.TimeBuff;
using Assets.Content.Scripts.UI.Weak;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowItem : MonoBehaviour
    {
        [field: SerializeField] public List<Items> Items { get; private set; } = new List<Items>();

        [SerializeField] private Transform _contentItems;
        [SerializeField] private Items[] _items;
        [SerializeField] private Items[] _itemsForAds;
        [SerializeField] private TextMeshProUGUI _textUsedItem;
        [SerializeField] private float _scaleAmount = 1.2f;
        [SerializeField] private float _scaleDuration = 0.2f;

        private List<TypeItems> _itemsUseList = new List<TypeItems>();
        private Items _item;
        private Coroutine _coroutine;
        private Vector3 originalScale;
        private bool _isAds = false;
        private GoldTimeBuff _goldTimeBuff;
        private ExpTimeBuff _expTimeBuff;
        private DamageTimeBuff _damageTimeBuff;

        private void Start()
        {
            Load();
            originalScale = _textUsedItem.transform.localScale;
            foreach (var item in TimerBuff.ItemsTime)
            {
                switch (item.Key)
                {
                    case TypeItems.Gold:
                        _goldTimeBuff = UnitController.Instance.gameObject.AddComponent<GoldTimeBuff>();
                        _goldTimeBuff.SetValue(1);
                        _goldTimeBuff.Duration = item.Value;
                        _goldTimeBuff.Type = TypeItems.Gold;
                        MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue = item.Value;
                        _goldTimeBuff.OnTime += x => MainUI.Instance.SliderGoldBuff.value = x;
                        _goldTimeBuff.Removed += () =>
                        {
                            if (MainUI.Instance.SliderGoldBuff != null)
                            {
                                MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue;
                                MainUI.Instance.SliderGoldBuff.gameObject.SetActive(false);
                            }

                            _itemsUseList.Remove(TypeItems.Gold);
                        };
                        _itemsUseList.Add(TypeItems.Gold);
                        _goldTimeBuff.Play();
                        MainUI.Instance.SliderGoldBuff.gameObject.SetActive(true);
                        _isAds = false;
                        _item = null;
                        break;
                    case TypeItems.Exp:
                        _expTimeBuff = UnitController.Instance.gameObject.AddComponent<ExpTimeBuff>();
                        _expTimeBuff.SetValue(1);
                        _expTimeBuff.Duration = item.Value;
                        _expTimeBuff.Type = TypeItems.Exp;
                        MainUI.Instance.SliderExpBuff.value = MainUI.Instance.SliderExpBuff.maxValue = item.Value;
                        _expTimeBuff.OnTime += x => MainUI.Instance.SliderExpBuff.value = x;
                        _expTimeBuff.Removed += () =>
                        {
                            if (MainUI.Instance.SliderExpBuff != null)
                            {
                                MainUI.Instance.SliderExpBuff.value = MainUI.Instance.SliderExpBuff.maxValue;
                                MainUI.Instance.SliderExpBuff.gameObject.SetActive(false);
                            }

                            _itemsUseList.Remove(TypeItems.Exp);
                        };
                        _itemsUseList.Add(TypeItems.Exp);
                        _expTimeBuff.Play();
                        MainUI.Instance.SliderExpBuff.gameObject.SetActive(true);
                        _isAds = false;
                        _item = null;
                        break;
                    case TypeItems.Damage:
                        _damageTimeBuff = UnitController.Instance.gameObject.AddComponent<DamageTimeBuff>();
                        _damageTimeBuff.SetValue(1);
                        _damageTimeBuff.Duration = item.Value;
                        _damageTimeBuff.Type = TypeItems.Damage;
                        MainUI.Instance.SliderDamageBuff.value = MainUI.Instance.SliderDamageBuff.maxValue = item.Value;
                        _damageTimeBuff.OnTime += x => MainUI.Instance.SliderDamageBuff.value = x;
                        _damageTimeBuff.Removed += () =>
                        {
                            if (MainUI.Instance.SliderDamageBuff != null)
                            {
                                MainUI.Instance.SliderDamageBuff.value = MainUI.Instance.SliderDamageBuff.maxValue;
                                MainUI.Instance.SliderDamageBuff.gameObject.SetActive(false);
                            }
                            _itemsUseList.Remove(TypeItems.Damage);
                        };
                        _itemsUseList.Add(TypeItems.Damage);
                        _damageTimeBuff.Play();
                        MainUI.Instance.SliderDamageBuff.gameObject.SetActive(true);
                        _isAds = false;
                        _item = null;
                        break;
                }                
            }
            TimerBuff.ItemsTime.Clear();
        }

        private void Save(int index)
        {
            YandexGame.savesData.Items.Add(_items[index].Index);
            YandexGame.SaveProgress();
        }
        public void Load()
        {
            YandexGame.LoadProgress();
            for (int i = 0; i < YandexGame.savesData.Items.Count; i++)
            {
                LoadChest(_items[YandexGame.savesData.Items[i]]);
            }
        }

        private void LoadChest(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            items.OnSelect += Selected;
            Items.Add(items);
        }

        public void AddItem(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            items.OnSelect += Selected;
            Items.Add(items);
            Save(items.Index);
        }
        public void RemoveItem(Items item)
        {
            Items.Remove(item);
            if (YandexGame.savesData.Items.Contains(item.Index))
            {
                YandexGame.savesData.Items.Remove(item.Index);
                YandexGame.SaveProgress();
            }
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        public void Selected(Items item)
        {
            _item?.DeSelected();
            _item = item;
        }

        public void Use()
        {
            if (_item == null)
                return;
            if (!_isAds && _itemsUseList.Contains(_item._typeItems))
            {
                _coroutine = StartCoroutine(ScaleAndDeactivate());
                return;
            }

            switch (_item._typeItems)
            {
                case TypeItems.Gold:
                    _goldTimeBuff = UnitController.Instance.gameObject.AddComponent<GoldTimeBuff>();
                    _goldTimeBuff.SetValue(_item.Value);
                    _goldTimeBuff.Duration = _item.Time;
                    _goldTimeBuff.Type = _item._typeItems;
                    MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue = _item.Time;
                    _goldTimeBuff.OnTime += x => MainUI.Instance.SliderGoldBuff.value = x;
                    _goldTimeBuff.Removed += () =>
                    {
                        if (MainUI.Instance.SliderGoldBuff != null)
                        {
                            MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue;
                            MainUI.Instance.SliderGoldBuff.gameObject.SetActive(false);
                        }

                        _itemsUseList.Remove(TypeItems.Gold);
                    };
                    _itemsUseList.Add(_item._typeItems);
                    _goldTimeBuff.Play();
                    MainUI.Instance.GoldBuffImage.sprite = _item.Image.sprite;
                    MainUI.Instance.SliderGoldBuff.gameObject.SetActive(true);
                    if (!_isAds)
                    {
                        RemoveItem(_item);
                    }
                    _isAds = false;
                    _item = null;
                    break;
                case TypeItems.Exp:
                    _expTimeBuff = UnitController.Instance.gameObject.AddComponent<ExpTimeBuff>();
                    _expTimeBuff.SetValue(_item.Value);
                    _expTimeBuff.Duration = _item.Time;
                    _expTimeBuff.Type = _item._typeItems;
                    MainUI.Instance.SliderExpBuff.value = MainUI.Instance.SliderExpBuff.maxValue = _item.Time;
                    _expTimeBuff.OnTime += x => MainUI.Instance.SliderExpBuff.value = x;
                    _expTimeBuff.Removed += () =>
                    {
                        if (MainUI.Instance != null)
                        {
                            MainUI.Instance.SliderExpBuff.value = MainUI.Instance.SliderExpBuff.maxValue;
                            MainUI.Instance.SliderExpBuff.gameObject.SetActive(false);
                        }

                        _itemsUseList.Remove(TypeItems.Exp);
                    };
                    _itemsUseList.Add(_item._typeItems);
                    _expTimeBuff.Play();
                    MainUI.Instance.ExpBuffImage.sprite = _item.Image.sprite;
                    MainUI.Instance.SliderExpBuff.gameObject.SetActive(true);
                    if (!_isAds)
                    {
                        RemoveItem(_item);
                    }
                    _isAds = false;
                    _item = null;
                    break;
                case TypeItems.Damage:
                    _damageTimeBuff = UnitController.Instance.gameObject.AddComponent<DamageTimeBuff>();
                    _damageTimeBuff.SetValue(_item.Value);
                    _damageTimeBuff.Duration = _item.Time;
                    _damageTimeBuff.Type = _item._typeItems;
                    MainUI.Instance.SliderDamageBuff.value = MainUI.Instance.SliderDamageBuff.maxValue = _item.Time;
                    _damageTimeBuff.OnTime += x => MainUI.Instance.SliderDamageBuff.value = x;
                    _damageTimeBuff.Removed += () =>
                    {
                        if (MainUI.Instance != null)
                        {
                            MainUI.Instance.SliderDamageBuff.value = MainUI.Instance.SliderDamageBuff.maxValue;
                            MainUI.Instance.SliderDamageBuff.gameObject.SetActive(false);
                        }
                        _itemsUseList.Remove(TypeItems.Damage);
                    };
                    _itemsUseList.Add(_item._typeItems);
                    _damageTimeBuff.Play();
                    MainUI.Instance.DamageBuffImage.sprite = _item.Image.sprite;
                    MainUI.Instance.SliderDamageBuff.gameObject.SetActive(true);
                    if (!_isAds)
                    {
                        RemoveItem(_item);
                    }
                    _isAds = false;
                    _item = null;
                    break;
            }
        }

        public void UseAds(int index)
        {
            _isAds = true;
            _item = _itemsForAds[index];
            if (index == 0)
            {
                if (_expTimeBuff != null)
                {                   
                    _expTimeBuff.Removed += () =>
                    {
                        MainUI.Instance.SliderExpBuff.value = MainUI.Instance.SliderExpBuff.maxValue;
                        MainUI.Instance.SliderExpBuff.gameObject.SetActive(false);
                        _itemsUseList.Remove(TypeItems.Exp);
                    };
                    Destroy(_expTimeBuff);
                }
            }
            if (index == 1)
            {
                if (_damageTimeBuff != null)
                {
                    _damageTimeBuff.Removed += () =>
                    {
                        MainUI.Instance.SliderDamageBuff.value = MainUI.Instance.SliderDamageBuff.maxValue;
                        MainUI.Instance.SliderDamageBuff.gameObject.SetActive(false);
                        _itemsUseList.Remove(TypeItems.Damage);
                    };
                    Destroy(_damageTimeBuff);
                }
            }
            Use();
        }

        private IEnumerator ScaleAndDeactivate()
        {
            _textUsedItem.gameObject.SetActive(true);

            Vector3 targetScale = originalScale * _scaleAmount;
            yield return StartCoroutine(ScaleTo(targetScale, _scaleDuration));

            _textUsedItem.gameObject.SetActive(false);

            _textUsedItem.transform.localScale = originalScale;
            _coroutine = null;
        }

        private IEnumerator ScaleTo(Vector3 target, float duration)
        {
            Vector3 initialScale = _textUsedItem.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _textUsedItem.transform.localScale = Vector3.Lerp(initialScale, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _textUsedItem.transform.localScale = target;
        }
    }
    public enum TypeItems
    {
        Exp,
        Damage,
        Gold,
    }
}