using Assets.Content.Scripts.UI.Weak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.UI
{
    public class UIChestItemEquip : MonoBehaviour
    {
        [SerializeField] private UiWeakItem _prefab;

        public UiWeakItem GiveGift()
        {
            return _prefab;
        }
    }
}