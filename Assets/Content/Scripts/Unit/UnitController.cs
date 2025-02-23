using Assets.Content.Scripts.UI;
using Cinemachine;
using UnityEngine;

namespace Assets.Content.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _unitRb;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Animator[] _animators;

        public float MaxHealthPlayer;
        public float CurrentHealthPlayer;
        public float DamagePlayer;
        public bool IsMobile;
        public float MoveSpeed;
        public float RotationPayerSpeed;
        public float SpeedScroll;
        public Vector2 OffsetZoom;
        public Vector2 OffsetSpeedCamera;
        public float RotationThreshold;
        public LayerMask CollisionLayerCamera;
        public WindowInfoUnit InfoUnit;

        private CinemachineFramingTransposer _cinemachineTransposer;
        private bool _isPOVMode;

        public static UnitController Instance { get; private set; } = null;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                return;
            }

            Instance = this;

            InfoUnit.SetName(name);
            _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            UpdateHealth();
            Zoom();
            HandleController();
            if (Input.GetMouseButton(1))
            {
                if (!_isPOVMode)
                {
                    _isPOVMode = true;
                    SwitchCamera();
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (_isPOVMode)
                {
                    _isPOVMode = false;
                    SwitchCamera();
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        private void HandleController()
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            SetAnimationSpeed(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (verticalInput > 0)
            {
                Rotation(_virtualCamera.transform.eulerAngles.y);
            }

            if (verticalInput < 0)
            {
                Rotation(_virtualCamera.transform.eulerAngles.y - 180f);
            }

            if (horizontalInput < 0)
            {
                Rotation(_virtualCamera.transform.eulerAngles.y - 90f);
            }

            if (horizontalInput > 0)
            {
                Rotation(_virtualCamera.transform.eulerAngles.y + 90f);
            }

            if (verticalInput == 0 && horizontalInput == 0)
            {
                _unitRb.velocity = new Vector3(_unitRb.velocity.x, _unitRb.velocity.y, 0);
            }
        }

        private void Rotation(float angle)
        {
            float currentAngle = transform.eulerAngles.y;
            float newAngle = Mathf.LerpAngle(currentAngle, angle, RotationPayerSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);

            if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, angle)) < RotationThreshold)
            {
                Move();
            }
        }



        private void Move()
        {
            Vector3 direction = transform.forward.normalized;
            _unitRb.velocity = new Vector3(direction.x * MoveSpeed, _unitRb.velocity.y, direction.z * MoveSpeed);
        }

        private void SetAnimationSpeed(float speed)
        {
            foreach (var animator in _animators)
            {
                animator.SetFloat("Speed", speed);
            }
        }

        private void SwitchCamera()
        {
            if (_isPOVMode)
            {
                var pov = _virtualCamera.AddCinemachineComponent<CinemachinePOV>();
                pov.m_VerticalAxis.Value = _virtualCamera.transform.eulerAngles.x;
                pov.m_HorizontalAxis.Value = _virtualCamera.transform.eulerAngles.y;
                pov.m_VerticalAxis.m_MinValue = 0f;
                pov.m_VerticalAxis.m_MaxSpeed = OffsetSpeedCamera.y;
                pov.m_HorizontalAxis.m_MaxSpeed = OffsetSpeedCamera.x;
            }
            else
            {
                _virtualCamera.DestroyCinemachineComponent<CinemachinePOV>();
            }
        }

        private void Zoom()
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            float newZoom = _cinemachineTransposer.m_CameraDistance + -scrollDelta * SpeedScroll;

            _cinemachineTransposer.m_CameraDistance = Mathf.Clamp(newZoom, OffsetZoom.x, OffsetZoom.y);
        }

        private void UpdateHealth()
        {
            InfoUnit.SetHealth(CurrentHealthPlayer, MaxHealthPlayer);
        }
    }
}