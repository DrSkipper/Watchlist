using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    public AudioClip MenuMusic;
    public AudioClip GameplayMusic;
    public AudioClip BossMusic;
    public AudioClip FinalBossMusic;
    public string GameplaySceneName = "Gameplay";
    public string BossRoomPrefix = "BossRoom";
    public string FinalBossRoomName = "BossRoomMaster";
    public string[] SilentScenes;
    public float MenuVolume = 1.0f;
    public float GameplayVolume = 0.5f;
    public float BossRoomVolume = 0.6f;
    public AudioSource AudioSource;

    void Start()
    {
        sceneBegin();
    }

    void OnLevelWasLoaded(int i)
    {
        sceneBegin();
    }

    private string _sceneName;

    private void sceneBegin()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        if (_sceneName == this.GameplaySceneName || _sceneName.Contains(this.BossRoomPrefix))
        {
            //this.AudioSource.Stop();
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
                this.AudioSource.volume = this.MenuVolume;
                if (this.AudioSource.clip != this.MenuMusic || !this.AudioSource.isPlaying)
                {
                    this.AudioSource.clip = this.MenuMusic;
                    this.AudioSource.Play();
                }
            }
            else
            {
                this.AudioSource.Stop();
            }
        }
    }

    private void gameplayBegin(LocalEventNotifier.Event e)
    {
        if (_sceneName == this.GameplaySceneName)
        {
            this.AudioSource.volume = this.GameplayVolume;
            this.AudioSource.clip = this.GameplayMusic;
            this.AudioSource.Play();
        }
        else if (_sceneName.Contains(this.BossRoomPrefix))
        {
            this.AudioSource.volume = this.BossRoomVolume;
            if (_sceneName == this.FinalBossRoomName)
            {
                this.AudioSource.clip = this.FinalBossMusic;
                this.AudioSource.Play();
            }
            else
            {
                this.AudioSource.clip = this.BossMusic;
                this.AudioSource.Play();
            }
        }
    }
}
