using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI
{
    public class Items : MonoBehaviour
    {
        [SerializeField] private TypeItems _typeItems;
        public float Price;
        public float Value;
        public int Index;
    }
}