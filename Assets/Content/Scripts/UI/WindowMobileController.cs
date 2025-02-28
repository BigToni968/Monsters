using Assets.Content.Scripts.Others;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.UI
{
    public class WindowMobileController : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick _leftMove;
        [SerializeField] private CameraControllerPanel _rightCamera;
        [SerializeField] private float _zoom;

        public Vector2 Rotate => new(_rightCamera.LookInputVector.x, _rightCamera.LookInputVector.y);
        public Vector2 Move => new(_leftMove.Horizontal, _leftMove.Vertical);
        public float Zoom => _zoom;
        public CameraControllerPanel CameraControllerPanel => _rightCamera;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetZoom(float value)
        {
            _zoom = value;
        }
    }
}