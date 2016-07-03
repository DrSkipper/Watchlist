using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    public AudioClip MenuMusic;
    public AudioClip GameplayMusic;
    public AudioClip BossMusic;
    public AudioClip FinalBossMusic;
    public AudioClip FinalMusic;
    public string GameplaySceneName = "Gameplay";
    public string BossRoomPrefix = "BossRoom";
    public string FinalBossRoomName = "BossRoomMaster";
    public string AltFinalBossRoomName = "FinalDialogScene";
    public string FinalRoomName = "FinalRoom";
    public string CreditsName = "Credits";
    public string[] WaitForPlayMusicEventScenes;
    public string[] DontStopScenePrefixes;
    public string[] SilentScenes;
    public float MenuVolume = 1.0f;
    public float GameplayVolume = 0.5f;
    public float BossRoomVolume = 0.6f;
    public float FinalBossVolume = 0.9f;
    public float VolumeFadeSpeed = 0.3f;
    public AudioSource AudioSource;

    void Start()
    {
        sceneBegin();
    }

    void OnLevelWasLoaded(int i)
    {
        sceneBegin();
    }

    void Update()
    {
        if (_isFading)
        {
            this.AudioSource.volume = Mathf.Max(0.0f, this.AudioSource.volume - this.VolumeFadeSpeed * Time.deltaTime);
        }
    }

    /**
     * Private
     */
    private string _sceneName;
    private bool _isFading;

    private void sceneBegin()
    {
        _isFading = false;
        _sceneName = SceneManager.GetActiveScene().name;

        bool waitForPlayEvent = false;
        if (this.WaitForPlayMusicEventScenes != null)
        {
            for (int i = 0; i < this.WaitForPlayMusicEventScenes.Length; ++i)
            {
                if (this.WaitForPlayMusicEventScenes[i] == _sceneName)
                {
                    waitForPlayEvent = true;
                    break;
                }
            }
        }

        bool dontStop = false;
        if (this.DontStopScenePrefixes != null)
        {
            for (int i = 0; i < this.DontStopScenePrefixes.Length; ++i)
            {
                if (_sceneName.Contains(this.DontStopScenePrefixes[i]))
                {
                    dontStop = true;
                    break;
                }
            }
        }

        if (_sceneName == this.GameplaySceneName || _sceneName.Contains(this.BossRoomPrefix))
        {
            if (!dontStop)
                this.AudioSource.Stop();
            if (!waitForPlayEvent)
                GlobalEvents.Notifier.Listen(BeginGameplayEvent.NAME, this, gameplayBegin);
        }
        else
        {
            bool contains = false;

            if (this.SilentScenes != null)
            {
                for (int i = 0; i < this.SilentScenes.Length; ++i)
                {
                    if (this.SilentScenes[i] == _sceneName)
                    {
                        contains = true;
                        break;
                    }
                }
            }

            if (!contains)
            {
                if (_sceneName != this.CreditsName)
                {
                    this.AudioSource.volume = this.MenuVolume;
                    if (this.AudioSource.clip != this.MenuMusic || !this.AudioSource.isPlaying)
                    {
                        this.AudioSource.clip = this.MenuMusic;

                        if (!waitForPlayEvent)
                            this.AudioSource.Play();
                        else if (!dontStop)
                            this.AudioSource.Stop();
                    }
                }
            }
            else
            {
                this.AudioSource.Stop();
            }
        }

        GlobalEvents.Notifier.Listen(PlayMusicEvent.NAME, this, playMusic);
        GlobalEvents.Notifier.Listen(BeginMusicFadeEvent.NAME, this, beginMusicFade);
    }

    private void gameplayBegin(LocalEventNotifier.Event e)
    {
        play();
    }

    private void beginMusicFade(LocalEventNotifier.Event e)
    {
        _isFading = true;
    }

    private void playMusic(LocalEventNotifier.Event e)
    {
        play();
    }

    private void play()
    {
        _isFading = false;
        if (_sceneName == this.GameplaySceneName)
        {
            this.AudioSource.volume = this.GameplayVolume;
            this.AudioSource.clip = this.GameplayMusic;
            this.AudioSource.Play();
        }
        else if (_sceneName.Contains(this.BossRoomPrefix) || _sceneName == this.AltFinalBossRoomName)
        {
            if (_sceneName == this.FinalBossRoomName || _sceneName == this.AltFinalBossRoomName)
            {
                this.AudioSource.volume = this.FinalBossVolume;
                this.AudioSource.clip = this.FinalBossMusic;
                this.AudioSource.Play();
            }
            else
            {
                this.AudioSource.volume = this.BossRoomVolume;
                this.AudioSource.clip = this.BossMusic;
                this.AudioSource.Play();
            }
        }
        else if (_sceneName == this.FinalRoomName)
        {
            this.AudioSource.volume = 1.0f;
            this.AudioSource.clip = this.FinalMusic;
            this.AudioSource.Play();
        }
    }
}
