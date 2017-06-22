using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : VoBehavior
{
    public GameObject LevelCompletePanel;
    public float LevelCompleteScreenLength = 3.0f;
    public float ReturnToLevelSelectDelay = 1.0f;
    public string Destination = "LevelSelectScene";
    public CompletionEffect OnComplete = CompletionEffect.CompleteTile;

    [System.Serializable]
    public enum CompletionEffect
    {
        CompleteTile,
        WipeProgressData
    }

    public void EnemySpawned(GameObject enemy)
    {
        Damagable d = enemy.GetComponent<Damagable>();
        if (d != null)
            d.OnDeathCallbacks.Add(this.EnemyDied);
        else
            enemy.GetComponent<MiniBossBehavior>().DeathCallback = this.EnemyDied;
    }

    public void EnemyDied(Damagable died)
    {
        ++enemiesDied;

        if (enemiesDied >= this.GetComponent<SpawnPositioner>().NumEnemies)
            this.EndLevel();
    }

    public void EndLevel()
    {
        PauseController.EnablePausing(false);
        this.LevelCompletePanel.GetComponent<Animator>().SetTrigger("LevelCompleteIn");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelCompleteOut, this.LevelCompleteScreenLength);
    }

    public void LevelCompleteOut()
    {
        GlobalEvents.Notifier.SendEvent(new BeginMusicFadeEvent());
        this.LevelCompletePanel.GetComponent<Animator>().SetTrigger("LevelCompleteOut");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.ReturnToLevelSelect, this.ReturnToLevelSelectDelay);
    }

    public void ReturnToLevelSelect()
    {
        GlobalEvents.Notifier.SendEvent(new LevelCompleteEvent());
        if (this.OnComplete == CompletionEffect.CompleteTile)
            ProgressData.CompleteTile(ProgressData.MostRecentTile);
        else if (this.OnComplete == CompletionEffect.WipeProgressData)
            ProgressData.WipeData();
        ProgressData.SaveToDisk();
        SceneManager.LoadScene(this.Destination);
    }

    /**
     * Private
     */
    private int enemiesDied;
}
