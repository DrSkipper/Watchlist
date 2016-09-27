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
        {
            _timedCallbacks = this.GetComponent<TimedCallbacks>();
            _menuMusic = this.MenuMusic;
            sceneBegin();
        }
    }

    void OnLevelWasLoaded(int i)
    {
        if (!this.Destroyer.MarkedForDestruction)
            sceneBegin();
    }

    void Update()
    {
        if (_isFading)
        {
            this.AudioSource.volume = Mathf.Max(0.0f, this.AudioSource.volume - this.VolumeFadeSpeed * Time.deltaTime);
        }

        if (!_allDone)
        {
            if (_menuRequest != null)
            {
                if (_menuRequest.isDone)
                {
                    _menuMusic = _menuRequest.asset as AudioClip;
                    _menuRequest = null;
                    //_gameplayMusic = this.GameplayMusic;

                    if (_playing && _attemptingPlay == this.MenuMusicName)
                    {
                        this.AudioSource.clip = _menuMusic;
                        this.AudioSource.Play();
                    }
                }
            }
            else if (_gameplayRequest != null)
            {
                if (_gameplayRequest.isDone)
                {
                    _gameplayMusic = _gameplayRequest.asset as AudioClip;
                    _gameplayRequest = null;
                    //_bossMusic = this.BossMusic;

                    if (_playing && _attemptingPlay == this.GameplayMusicName)
                    {
                        this.AudioSource.clip = _gameplayMusic;
                        this.AudioSource.Play();
                    }
                }
            }
            else if (_bossRequest != null)
            {
                if (_bossRequest.isDone)
                {
                    _bossMusic = _bossRequest.asset as AudioClip;
                    _bossRequest = null;
                    //_finalBossMusic = this.FinalBossMusic;

                    if (_playing && _attemptingPlay == this.BossMusicName)
                    {
                        this.AudioSource.clip = _bossMusic;
                        this.AudioSource.Play();
                    }
                }
            }
            else if (_finalBossRequest != null)
            {
                if (_finalBossRequest.isDone)
                {
                    _finalBossMusic = _finalBossRequest.asset as AudioClip;
                    _finalBossRequest = null;
                    //_finalMusic = this.FinalMusic;

                    if (_playing && _attemptingPlay == this.FinalBossMusicName)
                    {
                        this.AudioSource.clip = _finalBossMusic;
                        this.AudioSource.Play();
                    }
                }

            }
            else if (_finalRequest != null)
            {
                if (_finalRequest.isDone)
                {
                    _finalMusic = _finalRequest.asset as AudioClip;
                    _finalRequest = null;
                    _allDone = true;

                    if (_playing && _attemptingPlay == this.FinalMusicName)
                    {
                        this.AudioSource.clip = _finalMusic;
                        this.AudioSource.Play();
                    }
                }
            }
        }
    }

    /**
     * Private
     */
    private string _sceneName;
    private bool _isFading;
    private TimedCallbacks _timedCallbacks;
    private AudioClip _menuMusic;
    private AudioClip _gameplayMusic;
    private AudioClip _bossMusic;
    private AudioClip _finalBossMusic;
    private AudioClip _finalMusic;
    private ResourceRequest _menuRequest;
    private ResourceRequest _gameplayRequest;
    private ResourceRequest _bossRequest;
    private ResourceRequest _finalBossRequest;
    private ResourceRequest _finalRequest;
    private bool _allDone = false;
    private string _attemptingPlay = "";
    private bool _playing;
    private const string PATH_PREFIX = "Music/";

    private AudioClip MenuMusic
    {
        get
        {
            if (_menuMusic == null)
            {
                if (_menuRequest == null)
                {
                    _menuRequest = Resources.LoadAsync<AudioClip>(PATH_PREFIX + this.MenuMusicName);
                }
            }
            return _menuMusic;
        }
    }

    private AudioClip GameplayMusic
    {
        get
        {
            if (_gameplayMusic == null)
            {
                if (_gameplayRequest == null)
                {
                    _gameplayRequest = Resources.LoadAsync<AudioClip>(PATH_PREFIX + this.GameplayMusicName);
                }
            }
            return _gameplayMusic;
        }
    }

    private AudioClip BossMusic
    {
        get
        {
            if (_bossMusic == null)
            {
                if (_bossRequest == null)
                {
                    _bossRequest = Resources.LoadAsync<AudioClip>(PATH_PREFIX + this.BossMusicName);
                }
            }
            return _bossMusic;
        }
    }

    private AudioClip FinalBossMusic
    {
        get
        {
            if (_finalBossMusic == null)
            {
                if (_finalBossRequest == null)
                {
                    _finalBossRequest = Resources.LoadAsync<AudioClip>(PATH_PREFIX + this.FinalBossMusicName);
                }
            }
            return _finalBossMusic;
        }
    }

    private AudioClip FinalMusic
    {
        get
        {
            if (_finalMusic == null)
            {
                if (_finalRequest == null)
                {
                    _finalRequest = Resources.LoadAsync<AudioClip>(PATH_PREFIX + this.FinalMusicName);
                }
            }
            return _finalMusic;
        }
    }

    private void sceneBegin()
    {
        _isFading = false;
        _sceneName = SceneManager.GetActiveScene().name;
        _timedCallbacks.RemoveCallbacksForOwner(this);

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
            {
                _playing = false;
                this.AudioSource.Stop();
            }
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
                    if (_attemptingPlay != this.MenuMusicName || !this.AudioSource.isPlaying)
                    {
                        _attemptingPlay = this.MenuMusicName;
                        this.AudioSource.clip = this.MenuMusic;

                        if (!waitForPlayEvent)
                        {
                            _playing = true;
                            if (this.MenuMusic != null)
                            {
                                this.AudioSource.Play();
                            }
                        }
                        else if (!dontStop)
                        {
                            _playing = false;
                            this.AudioSource.Stop();
                        }
                    }
                }
            }
            else
            {
                _playing = false;
                this.AudioSource.Stop();
            }
        }

        _timedCallbacks.AddCallback(this, checkAsyncLoading, 1.0f);

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
            _attemptingPlay = this.GameplayMusicName;
            _playing = true;
            this.AudioSource.volume = this.GameplayVolume;

            if (this.GameplayMusic != null)
            {
                this.AudioSource.clip = this.GameplayMusic;
                this.AudioSource.Play();
            }
        }
        else if (_sceneName.Contains(this.BossRoomPrefix) || _sceneName == this.AltFinalBossRoomName)
        {
            if (_sceneName == this.FinalBossRoomName || _sceneName == this.AltFinalBossRoomName)
            {
                _attemptingPlay = this.FinalBossMusicName;
                _playing = true;
                this.AudioSource.volume = this.FinalBossVolume;

                if (this.FinalBossMusic != null)
                {
                    this.AudioSource.clip = this.FinalBossMusic;
                    this.AudioSource.Play();
                }
            }
            else
            {
                _attemptingPlay = this.BossMusicName;
                _playing = true;
                this.AudioSource.volume = this.BossRoomVolume;

                if (this.BossMusic != null)
                {
                    this.AudioSource.clip = this.BossMusic;
                    this.AudioSource.Play();
                }
            }
        }
        else if (_sceneName == this.FinalRoomName)
        {
            _attemptingPlay = this.FinalMusicName;
            _playing = true;
            this.AudioSource.volume = 1.0f;

            if (this.FinalMusic != null)
            {
                this.AudioSource.clip = this.FinalMusic;
                this.AudioSource.Play();
            }
        }
    }

    private void checkAsyncLoading()
    {
        if (_sceneName == this.GameplaySceneName)
        {
            if (_gameplayMusic == null)
                _gameplayMusic = this.GameplayMusic;
        }
        else if (_sceneName == this.FinalBossRoomName || _sceneName == this.AltFinalBossRoomName)
        {
            if (_finalBossMusic == null)
                _finalBossMusic = this.FinalBossMusic;
        }
        else if (_sceneName.Contains(this.BossRoomPrefix))
        {
            if (_bossMusic == null)
                _bossMusic = this.BossMusic;
        }
        else if (_sceneName == this.FinalRoomName)
        {
            if (_finalMusic == null)
                _finalMusic = this.FinalMusic;
        }
    }
}
