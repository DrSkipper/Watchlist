using UnityEngine;

[RequireComponent(typeof(TimedCallbacks))]
public class LevelInfoPanelHandler : MonoBehaviour
{
    public GameObject LevelInfoRootObject;
    public PlayerShopHandler[] Shops;
    public float BeginGameplayDelay = 0.4f;

    public void Update()
    {
        if (!_readied)
        {
            _readied = true;
            for (int i = 0; i < this.Shops.Length; ++i)
            {
                if (!this.Shops[i].HasReadied)
                {
                    _readied = false;
                    break;
                }
            }

            if (_readied)
            {
                Destroy(this.LevelInfoRootObject);
                this.GetComponent<TimedCallbacks>().AddCallback(this, beginGameplay, this.BeginGameplayDelay);
            }
        }
    }

    private bool _readied;

    private void beginGameplay()
    {
        GlobalEvents.Notifier.SendEvent(new BeginGameplayEvent());
        this.enabled = false;
    }
}
