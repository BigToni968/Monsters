using Assets.Content.Scripts.Statbuff;
using Assets.Content.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI.Weak
{
    public class UiWeakInventory : MonoBehaviour
    {

        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private List<UiWeakItem> _dressImage;
        [SerializeField] private List<InventoryWeakItemType> _itemTypes;

        private int _useItem = 0;


        private void OnEnable()
        {
            for (int i = 0; i < _windowInventory.ItemsEquipment.Count; i++)
            {
                _windowInventory.ItemsEquipment[i].Index = i;
                _windowInventory.ItemsEquipment[i].OnSelect += Dress;
            }


            for (int i = 0; i < _dressImage.Count; i++)
            {
                _dressImage[i].OnSelect += Undress;
            }
        }

        public void Dress(int id, InventoryWeakItemType type)
        {
            switch (type)
            {
                case InventoryWeakItemType.Health:
                    OnDress(type, id);
                    HealthStatBuff healthStatBuff = UnitController.Instance.gameObject.AddComponent<HealthStatBuff>();
                    healthStatBuff.SetValue(_windowInventory.ItemsEquipment[id].Value);
                    healthStatBuff.Active();
                    break;
                case InventoryWeakItemType.Damage:
                    OnDress(type, id);
                    DamageStatBuff DamageStatBuff = UnitController.Instance.gameObject.AddComponent<DamageStatBuff>();
                    DamageStatBuff.SetValue(_windowInventory.ItemsEquipment[id].Value);
                    DamageStatBuff.Active();
                    break;
                case InventoryWeakItemType.Speed:
                    OnDress(type, id);
                    SpeedStatBuff speedStatBuff = UnitController.Instance.gameObject.AddComponent<SpeedStatBuff>();
                    speedStatBuff.SetValue(_windowInventory.ItemsEquipment[id].Value);
                    speedStatBuff.Active();
                    break;
                case InventoryWeakItemType.Exp:
                    OnDress(type, id);
                    ExpStatsBuff expStatsBuff = UnitController.Instance.gameObject.AddComponent<ExpStatsBuff>();
                    expStatsBuff.SetValue(_windowInventory.ItemsEquipment[id].Value);
                    expStatsBuff.Active();
                    break;
                case InventoryWeakItemType.Gold:
                    OnDress(type, id);
                    GoldStatBuff goldStatBuff = UnitController.Instance.gameObject.AddComponent<GoldStatBuff>();
                    goldStatBuff.SetValue(_windowInventory.ItemsEquipment[id].Value);
                    goldStatBuff.Active();
                    break;

            }
        }

        public void Undress(int id, InventoryWeakItemType type)
        {
            switch (type)
            {
                case InventoryWeakItemType.Health:
                    OnUndress(id, type);
                    UnitController.Instance.gameObject.TryGetComponent(out HealthStatBuff health);
                    Destroy(health);
                    break;
                case InventoryWeakItemType.Damage:
                    OnUndress(id, type);
                    UnitController.Instance.gameObject.TryGetComponent(out DamageStatBuff damage);
                    Destroy(damage);
                    break;
                case InventoryWeakItemType.Speed:
                    OnUndress(id, type);
                    UnitController.Instance.gameObject.TryGetComponent(out SpeedStatBuff speed);
                    Destroy(speed);
                    break;
                case InventoryWeakItemType.Exp:
                    OnUndress(id, type);
                    UnitController.Instance.gameObject.TryGetComponent(out ExpStatsBuff exp);
                    Destroy(exp);
                    break;
                case InventoryWeakItemType.Gold:
                    OnUndress(id, type);
                    UnitController.Instance.gameObject.TryGetComponent(out GoldStatBuff gold);
                    Destroy(gold);
                    break;
            }
        }

        private void OnDress(InventoryWeakItemType type, int id)
        {
            if (_useItem < _dressImage.Count)
            {
                for (int i = 0; i < _dressImage.Count; i++)
                {
                    if (!_dressImage[i].IsBusy)
                    {
                        _itemTypes.Add(type);
                        _useItem++;
                        _windowInventory.ItemsEquipment[id].gameObject.SetActive(false);
                        _dressImage[i].Image.sprite = _windowInventory.ItemsEquipment[id].Image.sprite;
                        _dressImage[i].gameObject.SetActive(true);
                        _dressImage[i].IsBusy = true;
                        _dressImage[i].IndexItems = id;
                        _dressImage[i].Index = i;
                        _dressImage[i].Type = type;
                        Save();
                        break;
                    }
                }
            }
        }

        private void OnUndress(int id, InventoryWeakItemType type)
        {
            if (_useItem > 0)
            {
                if (_dressImage[id].IsBusy)
                {
                    _itemTypes.Remove(type);
                    _useItem--;
                    _windowInventory.ItemsEquipment[_dressImage[id].IndexItems].gameObject.SetActive(true);
                    _dressImage[id].gameObject.SetActive(false);
                    _dressImage[id].IsBusy = false;
                    Save();
                }
            }
        }

        private void LoadDress(int k, int i, InventoryWeakItemType type)
        {
            _useItem++;
            _windowInventory.ItemsEquipment[k].gameObject.SetActive(false);
            _dressImage[i].Image.sprite = _windowInventory.ItemsEquipment[k].Image.sprite;
            _dressImage[i].gameObject.SetActive(true);
            _dressImage[i].IsBusy = true;
            _dressImage[i].IndexItems = k;
            _dressImage[i].Index = i;
            _dressImage[i].Type = type;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _windowInventory.ItemsEquipment.Count; i++)
            {
                _windowInventory.ItemsEquipment[i].OnSelect -= Dress;
            }

            for (int i = 0; i < _dressImage.Count; i++)
            {
                _dressImage[i].OnSelect -= Undress;
            }
        }

        private void Save()
        {
            YandexGame.savesData.ItemTypes = _itemTypes;
            YandexGame.SaveProgress();
        }

        public void Load()
        {
            if (YandexGame.savesData.ItemTypes.Count > 0 || YandexGame.savesData.ItemsEquipment.Count > 0)
            {
                YandexGame.LoadProgress();
                _itemTypes = YandexGame.savesData.ItemTypes;

                for (int k = 0; k < _windowInventory.ItemsEquipment.Count; k++)
                {
                    for (int i = 0; i < _itemTypes.Count; i++)
                    {
                        if (_itemTypes[i] == _windowInventory.ItemsEquipment[k].Type)
                        {
                            if (_dressImage[i].IsBusy == true)
                            {
                                continue;
                            }
                            switch (_itemTypes[i])
                            {
                                case InventoryWeakItemType.Health:
                                    LoadDress(k, i, InventoryWeakItemType.Health);
                                    HealthStatBuff healthStatBuff = UnitController.Instance.gameObject.AddComponent<HealthStatBuff>();
                                    healthStatBuff.SetValue(_windowInventory.ItemsEquipment[k].Value);
                                    healthStatBuff.Active();
                                    break;
                                case InventoryWeakItemType.Damage:
                                    LoadDress(k, i, InventoryWeakItemType.Damage);
                                    DamageStatBuff DamageStatBuff = UnitController.Instance.gameObject.AddComponent<DamageStatBuff>();
                                    DamageStatBuff.SetValue(_windowInventory.ItemsEquipment[k].Value);
                                    DamageStatBuff.Active();
                                    break;
                                case InventoryWeakItemType.Speed:
                                    LoadDress(k, i, InventoryWeakItemType.Speed);
                                    SpeedStatBuff speedStatBuff = UnitController.Instance.gameObject.AddComponent<SpeedStatBuff>();
                                    speedStatBuff.SetValue(_windowInventory.ItemsEquipment[k].Value);
                                    speedStatBuff.Active();
                                    break;
                                case InventoryWeakItemType.Exp:
                                    LoadDress(k, i, InventoryWeakItemType.Exp);
                                    ExpStatsBuff expStatsBuff = UnitController.Instance.gameObject.AddComponent<ExpStatsBuff>();
                                    expStatsBuff.SetValue(_windowInventory.ItemsEquipment[k].Value);
                                    expStatsBuff.Active();
                                    break;
                                case InventoryWeakItemType.Gold:
                                    LoadDress(k, i, InventoryWeakItemType.Gold);
                                    GoldStatBuff goldStatBuff = UnitController.Instance.gameObject.AddComponent<GoldStatBuff>();
                                    goldStatBuff.SetValue(_windowInventory.ItemsEquipment[k].Value);
                                    goldStatBuff.Active();
                                    break;
                            }
                            break;
                        }
                    }

                }
            }
        }
    }

    public enum InventoryWeakItemType
    {
        Health,
        Damage,
        Exp,
        Speed,
        Gold
    }
}