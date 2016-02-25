using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RadialDistancing))]
public class BossElderMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public float MinRadius = 10.0f;
    public float MaxRadius = 300.0f;
    public GameObject EndFlowObject;

    void Awake()
    {
        _radius = this.GetComponent<RadialDistancing>();
    }

    void Start()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }
    }

    void Update()
    {
        if ((_radius.Radius <= this.MinRadius && _radius.RadiusChangeSpeed < 0.0f) || 
            (_radius.Radius >= this.MaxRadius && _radius.RadiusChangeSpeed > 0.0f))
        {
            _radius.RadiusChangeSpeed *= -1;
        }
    }

    /**
     * Private
     */
    private RadialDistancing _radius;
    
    private void SubBossKilled(Damagable died)
    {
        GameObject killedObject = died.gameObject;
        this.SubBosses.Remove(killedObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }

        RadialDistancing.RadialElement? radialElement = null;

        foreach (RadialDistancing.RadialElement element in _radius.RadialElements)
        {
            if (element.transform.gameObject == killedObject)
            {
                radialElement = element;
                break;
            }
        }

        if (radialElement.HasValue)
            _radius.RadialElements.Remove(radialElement.Value);
    }
}
