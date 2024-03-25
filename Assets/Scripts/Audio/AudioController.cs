using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace CGD.Audio
{
    public class AudioController : Singleton<AudioController>
    {
        [Header("Scene References")]
        [SerializeField] private AudioMixer mixer;
        [Space(2)]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Collections")]
        [SerializeField] private UICollection ui;
        [SerializeField] private MusicCollection music;
        [SerializeField] private GameplayCollection gameplay;


        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
        {
            if (scene.buildIndex == 1)
                PlayMenuMusic();
            else
                StopMusic();
        }




        #region Get Functions
        public AudioClip GetClip(UI name) => ui.GetClip(name.ToString());
        public AudioClip GetClip(Music name) => music.GetClip(name.ToString());
        public AudioClip GetClip(Gameplay name) => gameplay.GetClip(name.ToString());
        #endregion

        #region Music
        private void PlayMenuMusic() 
        {
            musicSource.loop = true;
            musicSource.clip = GetClip(Music.Menu);
            musicSource.Play();
        }
        private void StopMusic() => musicSource.Stop();
        #endregion

        #region UI
        public void PlayUI(UI ui) => sfxSource.PlayOneShot(GetClip(ui));
        #endregion
    }
}