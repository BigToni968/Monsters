using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Content.Scripts.Others
{
    public class PortalTrigger : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        [SerializeField] private Transform _canvas;

        private void Update()
        {
            if (_canvas.gameObject.activeSelf)
            {
                _canvas.LookAt(Camera.main.transform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<UnitController>(out UnitController controller))
            {
                SceneManager.LoadScene(_sceneIndex);
            }
        }
    }
}