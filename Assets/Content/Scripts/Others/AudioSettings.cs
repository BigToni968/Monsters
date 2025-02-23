using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.Others
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private Slider music;
        [SerializeField] private Slider sound;

        private void OnEnable()
        {
            music.value = AudioManager.Instance.Music.volume;
            music.onValueChanged.AddListener(SettingsMusic);
            sound.value = AudioManager.Instance.Sound.volume;
            sound.onValueChanged.AddListener(SettingsSound);
        }
        private void OnDisable()
        {
            music.onValueChanged.RemoveListener(SettingsMusic);
            sound.onValueChanged.RemoveListener(SettingsSound);
        }
        private void SettingsMusic(float value)
        {
            AudioManager.Instance.Music.volume = value;
            PlayerPrefs.SetFloat("Music", value);
        }
        private void SettingsSound(float value)
        {
            AudioManager.Instance.Sound.volume = value;
            PlayerPrefs.SetFloat("Volume", value);
        }
    }
}