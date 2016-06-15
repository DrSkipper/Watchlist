using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
public class LevelInfoPanelHandler : MonoBehaviour
{
    public GameObject LevelInfoRootObject;
    public PlayerShopHandler[] Shops;
    public float BeginGameplayDelay = 0.4f;
    public LevelGenManager LevelGenManager;
    public SpawnPositioner SpawnPositioner;
    public Text LevelLayoutText;
    public string LevelLayoutCAString = "OPEN SPACES";
    public string LevelLayoutOtherString = "NARROW CORRIDORS";
    public Image EnemyImage1;
    public Image EnemyImage2;
    public Texture2D EnemySprites;

    void Awake()
    {
        this.LevelGenManager.AddUpdateDelegate(levelGenUpdate);
    }

    public void Update()
    {
        if (!_readied)
        {
            _readied = true;
            for (int i = 0; i < this.Shops.Length; ++i)
            {
                if (this.Shops[i] != null && !this.Shops[i].HasReadied)
                {
                    _readied = false;
                    break;
                }
            }

            if (_readied)
            {
                PauseController.EnablePausing(true);
                Destroy(this.LevelInfoRootObject);
                this.GetComponent<TimedCallbacks>().AddCallback(this, beginGameplay, this.BeginGameplayDelay);
            }
        }

        if (!_haveUpdatedEnemyInfo && this.SpawnPositioner.SpawnersPlaced)
        {
            _haveUpdatedEnemyInfo = true;

            int mostCommon = -1;
            int secondMostCommon = -1;

            Dictionary<int, int> enemyCounts = this.SpawnPositioner.GetEnemyCounts();
            foreach (int enemyId in enemyCounts.Keys)
            {
                if (mostCommon == -1 || enemyCounts[enemyId] > enemyCounts[mostCommon])
                {
                    secondMostCommon = mostCommon;
                    mostCommon = enemyId;
                }
                else if (secondMostCommon == -1 || enemyCounts[enemyId] > enemyCounts[secondMostCommon])
                {
                    secondMostCommon = enemyId;
                }
            }
            Dictionary<string, Sprite> sprites = this.EnemySprites.GetSprites();
            if (mostCommon != -1)
                this.EnemyImage1.sprite = sprites[StaticData.EnemyData.EnemyTypes[mostCommon].SpriteName];
            else
                this.EnemyImage1.enabled = false;
            if (secondMostCommon != -1)
                this.EnemyImage2.sprite = sprites[StaticData.EnemyData.EnemyTypes[secondMostCommon].SpriteName];
            else
                this.EnemyImage2.enabled = false;
        }
    }

    private bool _readied;
    private bool _haveUpdatedLevelInfo;
    private bool _haveUpdatedEnemyInfo;
    private LevelGenOutput _output;

    private void levelGenUpdate()
    {
        if (this.LevelGenManager.Finished)
        {
            LevelGenOutput output = this.LevelGenManager.GetOutput();

            if (output.Input.Type == LevelGenInput.GenerationType.CA)
                this.LevelLayoutText.text = this.LevelLayoutCAString;
            else
                this.LevelLayoutText.text = this.LevelLayoutOtherString;
        }
    }

    private void beginGameplay()
    {
        GlobalEvents.Notifier.SendEvent(new BeginGameplayEvent());
        this.enabled = false;
    }
}
