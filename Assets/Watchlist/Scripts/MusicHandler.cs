using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    public string MenuMusicName;
    public string GameplayMusicName;
    public string BossMusicName;
    public string FinalBossMusicName;
    public string FinalMusicName;
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
    public DontDestroyOnLoad Destroyer;

    void Start()
    {
        if (!this.Destroyer.MarkedForDestruction)
            sceneBegin();
    }

    void OnLevelWasLoaded(int i)
    {
        if (!this.Destroyer.MarkedForDestruction)
        {
            if (_timedCallbacks == null)
                _timedCallbacks = this.GetComponent<TimedCallbacks>();
            sceneBegin();
        }
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
    private AudioClip _menuMusic;
    private AudioClip _gameplayMusic;
    private AudioClip _bossMusic;
    private AudioClip _finalBossMusic;
    private AudioClip _finalMusic;
    private const string PATH_PREFIX = "Music/";

    private AudioClip MenuMusic
    {
        get
        {
            if (_menuMusic == null)
                _menuMusic = Resources.Load<AudioClip>(PATH_PREFIX + this.MenuMusicName);
            return _menuMusic;
        }
    }

    private AudioClip GameplayMusic
    {
        get
        {
            if (_gameplayMusic == null)
                _gameplayMusic = Resources.Load<AudioClip>(PATH_PREFIX + this.GameplayMusicName);
            return _gameplayMusic;
        }
    }

    private AudioClip BossMusic
    {
        get
        {
            if (_bossMusic == null)
                _bossMusic = Resources.Load<AudioClip>(PATH_PREFIX + this.BossMusicName);
            return _bossMusic;
        }
    }

    private AudioClip FinalBossMusic
    {
        get
        {
            if (_finalBossMusic == null)
                _finalBossMusic = Resources.Load<AudioClip>(PATH_PREFIX + this.FinalBossMusicName);
            return _finalBossMusic;
        }
    }

    private AudioClip FinalMusic
    {
        get
        {
            if (_finalMusic == null)
                _finalMusic = Resources.Load<AudioClip>(PATH_PREFIX + this.FinalMusicName);
            return _finalMusic;
        }
    }

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
