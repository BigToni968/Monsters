using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Others
{
    public class AudioManager : MonoBehaviour
    {
        [field: SerializeField] public static AudioManager Instance { get; private set; }

        public AudioSource Music;
        public AudioSource Sound;

        public AudioClip[] MusicClips;
        public AudioClip[] KickClips;
        public AudioClip NotificationClip;
        public AudioClip Spinner;
        public AudioClip ClipButton;
        public AudioClip ClipOpenUnit;

        private List<AudioClip> availableTracks;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            float musicValue = Music.volume;

            if (PlayerPrefs.HasKey("Music"))
            {
                musicValue = PlayerPrefs.GetFloat("Music");
            }
            PlayerPrefs.SetFloat("Music", musicValue);
            Music.volume = musicValue;

            float soundValue = Sound.volume;
            if (PlayerPrefs.HasKey("Volume"))
            {
                soundValue = PlayerPrefs.GetFloat("Volume");
            }
            PlayerPrefs.SetFloat("Volume", soundValue);
            Sound.volume = soundValue;

            StartPlaying();
        }

        public void ButtonSound()
        {
            Sound.PlayOneShot(ClipButton);
        }
        public void SoundChest()
        {
            Sound.PlayOneShot(Spinner);
        }
        void StartPlaying()
        {
            availableTracks = new List<AudioClip>(MusicClips);
            PlayNextTrack();
        }

        void PlayNextTrack()
        {
            if (availableTracks.Count == 0)
            {
                availableTracks = new List<AudioClip>(MusicClips);
                ShuffleTracks(availableTracks);
            }

            int randomIndex = Random.Range(0, availableTracks.Count);
            AudioClip nextTrack = availableTracks[randomIndex];

            availableTracks.RemoveAt(randomIndex);

            Music.clip = nextTrack;
            Music.Play();

            StartCoroutine(WaitForTrackToEnd(Music.clip.length));
        }

        System.Collections.IEnumerator WaitForTrackToEnd(float trackLength)
        {
            yield return new WaitForSeconds(trackLength - 3f);
            PlayNextTrack();
        }

        void ShuffleTracks(List<AudioClip> tracksToShuffle)
        {
            for (int i = 0; i < tracksToShuffle.Count; i++)
            {
                AudioClip temp = tracksToShuffle[i];
                int randomIndex = Random.Range(i, tracksToShuffle.Count);
                tracksToShuffle[i] = tracksToShuffle[randomIndex];
                tracksToShuffle[randomIndex] = temp;
            }
        }
    }
}