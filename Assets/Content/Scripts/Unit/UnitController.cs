using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _unitRb;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Animator[] _animators;
        [SerializeField] private WindowMobileController _windowMobileController;
        [SerializeField] private Button _buttonAttack;
        [SerializeField] private PlayerUnit _currentUnit;

        public float RotationCameraSpeedMobile;
        public float MaxHealthPlayer;
        public float CurrentHealthPlayer;
        public float DamagePlayerStatic;
        public float DamagePlayerBuff;
        public bool IsMobile;
        public float MoveSpeed;
        public float RotationPayerSpeed;
        public float SpeedScroll;
        public Vector2 OffsetZoom;
        public Vector2 OffsetSpeedCamera;
        public float RotationThreshold;
        public LayerMask CollisionLayerCamera;
        public WindowInfoUnit InfoUnit;
        public float BuffSpeed;
        public bool IsTakeDamage;

        private CinemachineFramingTransposer _cinemachineTransposer;
        private bool _isPOVMode;

        public static UnitController Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                return;
            }

            Instance = this;

            var randomInt = Random.Range(1, 10000);

            if (YandexGame.playerName == "unauthorized" || YandexGame.playerName == "")
            {
                if (YandexGame.EnvironmentData.language == "ru")
                {
                    YandexGame.playerName = "Игрок" + randomInt;
                }
                else if (YandexGame.EnvironmentData.language == "en")
                {
                    YandexGame.playerName = "Player" + randomInt;
                }
                else if (YandexGame.EnvironmentData.language == "tr")
                {
                    YandexGame.playerName = "Oyuncu" + randomInt;
                }
            }

            InfoUnit.SetName(YandexGame.playerName);
            _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            IsMobile = Application.isMobilePlatform;
            if (IsMobile)
            {
                _windowMobileController.Show();
            }
            else
            {
                _windowMobileController.Hide();
            }
        }

        public void Save()
        {
            YandexGame.savesData.MaxHealthPlayer = MaxHealthPlayer;
            YandexGame.savesData.CurrentHealthPlayer = CurrentHealthPlayer;
            YandexGame.savesData.DamagePlayer = DamagePlayerStatic;

            YandexGame.SaveProgress();
        }
        private void Update()
        {
            UpdateHealth();
            Zoom();
            if (IsMobile)
            {
                if (MainUI.Instance.AutoAttack)
                {
                    _buttonAttack.gameObject.SetActive(false);
                }
                else
                {
                    _buttonAttack.gameObject.SetActive(true);
                }
                MobileRotate();
                MobileMove();
                return;
            }
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

        public void Attack()
        {
            _currentUnit.Animator.SetTrigger("IsAttack");
        }
        public void SetUnit(PlayerUnit unit)
        {
            _currentUnit = unit;
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
                _unitRb.velocity = new Vector3(0, _unitRb.velocity.y, 0);
            }
        }
        public void MobileRotate()
        {
            float horizontalInput = _windowMobileController.Rotate.x;
            float verticalInput = _windowMobileController.Rotate.y;
            float lookAxisUp = 0;
            float lookAxisRight = 0;

            if (_windowMobileController.CameraControllerPanel.pressed)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.fingerId == _windowMobileController.CameraControllerPanel.fingerId)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            lookAxisUp = -verticalInput * RotationCameraSpeedMobile;
                            lookAxisRight = horizontalInput * RotationCameraSpeedMobile;
                        }
                    }
                }
            }

            _virtualCamera.transform.Rotate(new Vector3(lookAxisUp * Time.deltaTime, lookAxisRight * Time.deltaTime, 0));
            _virtualCamera.transform.rotation = Quaternion.Euler(NormalizeAngle(_virtualCamera.transform.eulerAngles.x), NormalizeAngle(_virtualCamera.transform.eulerAngles.y), 0);

            float currentXRotation = NormalizeAngle(_virtualCamera.transform.eulerAngles.x);
            if (currentXRotation < 0)
            {
                currentXRotation = 0;
            }
            _virtualCamera.transform.rotation = Quaternion.Euler(currentXRotation, NormalizeAngle(_virtualCamera.transform.eulerAngles.y), 0);
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
        private void MobileMove()
        {
            Vector3 forward = _virtualCamera.transform.forward;
            Vector3 right = _virtualCamera.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            float horizontalInput = _windowMobileController.Move.x;
            float verticalInput = _windowMobileController.Move.y;

            Vector3 direction = (forward * verticalInput + right * horizontalInput).normalized;

            SetAnimationSpeed(direction.magnitude);

            if (direction.magnitude >= 0.1f)
            {
                Vector3 targetVelocity = direction * (MoveSpeed * BuffSpeed);

                _unitRb.velocity = new Vector3(targetVelocity.x, _unitRb.velocity.y, targetVelocity.z);

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            else
            {
                _unitRb.velocity = new Vector3(0, _unitRb.velocity.y, 0);
            }
        }

        private void Move()
        {
            Vector3 direction = transform.forward.normalized;
            _unitRb.velocity = new Vector3(direction.x * (MoveSpeed * BuffSpeed), _unitRb.velocity.y, direction.z * (MoveSpeed * BuffSpeed));
        }

        private void SetAnimationSpeed(float speed)
        {
            _currentUnit.Animator.SetFloat("Speed", speed);
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
            float scrollDelta = IsMobile ? _windowMobileController.Zoom : Input.mouseScrollDelta.y;

            float newZoom = _cinemachineTransposer.m_CameraDistance + (-scrollDelta) * SpeedScroll;

            _cinemachineTransposer.m_CameraDistance = Mathf.Clamp(newZoom, OffsetZoom.x, OffsetZoom.y);
        }
        private float NormalizeAngle(float angle)
        {
            if (angle > 180)
                angle -= 360;
            return angle;
        }
        private void UpdateHealth()
        {
            InfoUnit.SetHealth(CurrentHealthPlayer, MaxHealthPlayer);
        }
    }
}