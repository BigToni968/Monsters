using Assets.Content.Scripts.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI
{
    public class WindowLuckySpin : MonoBehaviour
    {
        private int _numberOfTurns;
        private int _whatWin;
        private float _speed;

        [SerializeField] private Button _buttonExit;
        [SerializeField] private Button _buttonSpin;
        [SerializeField] public TextMeshProUGUI _timerText;

        public bool IsTurn = false;
        public bool IsSpin = false;
        public float Time = 10f;
        public int TotalSpin = 0;
        public Coroutine Coroutine;

        public delegate void SpeensDelegate();
        public event SpeensDelegate OnUpdate;

        public void Update()
        {
            _buttonSpin.interactable = TotalSpin > 0;
        }
        public string Convert(int time)
        {
            int hours = (int)(time / 3600);
            int minutes = (int)((time % 3600) / 60);
            int seconds = (int)(time % 60);
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        public IEnumerator Timer(float time, TextMeshProUGUI textTimer)
        {
            _timerText.gameObject.SetActive(true);
            while (time > 0)
            {
                time -= 1;
                textTimer.SetText(Convert((int)time));
                yield return new WaitForSeconds(1f);
            }
            TotalSpin += 1;
            IsTurn = true;
            OnUpdate?.Invoke();
            Coroutine = null;
        }

        public void Spin()
        {
            if (IsTurn && !IsSpin)
            {
               // _AudioManager.SoundSpinner();
                IsSpin = true;
                StartCoroutine(TurnTheWheel());
                TotalSpin--;
                OnUpdate?.Invoke();
            }
        }

        public IEnumerator TurnTheWheel()
        {
            _timerText.gameObject.SetActive(false);
            _buttonExit.gameObject.SetActive(false);
            _numberOfTurns = Random.Range(35, 60);

            _speed = 0.01f;

            for (int i = 0; i < _numberOfTurns; i++)
            {
                transform.Rotate(0, 0, 30f);

                if (i > Mathf.RoundToInt(_numberOfTurns * 0.5f))
                {
                    _speed = 0.02f;
                }
                if (i > Mathf.RoundToInt(_numberOfTurns * 0.7f))
                {
                    _speed = 0.05f;
                }
                if (i > Mathf.RoundToInt(_numberOfTurns * 0.9f))
                {
                    _speed = 0.07f;
                }

                yield return new WaitForSeconds(_speed);
            }
            if (Mathf.RoundToInt(transform.eulerAngles.z) % 60 != 0)
            {
                transform.Rotate(0, 0, 30f);
            }
            _whatWin = Mathf.RoundToInt(transform.eulerAngles.z);
            switch (_whatWin)
            {
                case 0:
                    
                    Debug.Log("1 приз");
                    break;
                case 60:
                    
                    Debug.Log("2 приз");
                    break;
                case 120:
                    Debug.Log("3 приз");
                    break;
                case 180:
                    Debug.Log("4 приз");
                    break;
                case 240:
                    Debug.Log("5 приз");
                    break;
                case 300:
                    Debug.Log("6 приз");
                    break;
            }
            _buttonExit.gameObject.SetActive(true);
            _timerText.gameObject.SetActive(true);

            if (TotalSpin > 0)
            {
                IsTurn = true;
            }
            else
            {
                IsTurn = false;
            }
            IsSpin = false;
        }

        public void UpdateSpin(int spins)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Timer(Time, _timerText));
                Coroutine = null;
                _timerText.gameObject.SetActive(false);
            }
            TotalSpin += spins;
            MainUI.Instance.UpdateSpin();
            if (TotalSpin > 0)
            {
                IsTurn = true;
            }
            else
            {
                IsTurn = false;
            }
        }
    }
}