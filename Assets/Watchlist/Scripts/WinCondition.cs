using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : VoBehavior
{
    public GameObject LevelCompletePanel;
    public float LevelCompleteScreenLength = 3.0f;
    public float ReturnToLevelSelectDelay = 1.0f;

    public void EnemySpawned(GameObject enemy)
    {
        enemy.GetComponent<Damagable>().OnDeathCallbacks.Add(this.EnemyDied);
    }

    public void EnemyDied(Damagable died)
    {
        ++enemiesDied;

        if (enemiesDied >= this.GetComponent<SpawnPositioner>().NumEnemies)
            this.EndLevel();
    }

    public void EndLevel()
    {
        this.LevelCompletePanel.GetComponent<Animator>().SetTrigger("LevelCompleteIn");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelCompleteOut, this.LevelCompleteScreenLength);
    }

    public void LevelCompleteOut()
    {
        this.LevelCompletePanel.GetComponent<Animator>().SetTrigger("LevelCompleteOut");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.ReturnToLevelSelect, this.ReturnToLevelSelectDelay);
    }

    public void ReturnToLevelSelect()
    {
        DynamicData.CompleteTile(DynamicData.MostRecentTile);
        SceneManager.LoadScene("LevelSelectScene");
    }

    /**
     * Private
     */
    private int enemiesDied;
}
