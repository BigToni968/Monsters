using Assets.Content.Scripts.UI.Weak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.UI
{
    public class UIChestItem : MonoBehaviour
    {
        [SerializeField] private Items _prefab;

        public Items GiveGift()
        {
            return _prefab;
        }
    }
}