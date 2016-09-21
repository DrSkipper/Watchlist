using UnityEngine;
using System.Collections.Generic;

public class LaserRenderer : VoBehavior
{
    public GameObject LinePrefab;
    public string LineObjectKey = "laser_line";

    void Awake()
    {
        _lineObjects = new List<GameObject>();
        this.localNotifier.Listen(LaserCastEvent.NAME, this, this.OnLaserCast);
    }

    public void OnLaserCast(LocalEventNotifier.Event localEvent)
    {
        LaserCastEvent castEvent = (LaserCastEvent)localEvent;
        CollisionManager.RaycastResult raycast = castEvent.RaycastResult;

        IntegerVector distance = raycast.FarthestPointReached - castEvent.Origin;
        if (distance.X == 0 && distance.Y == 0)
            return;
        
        GameObject lineObject = ObjectPools.GetPooledObject(this.LineObjectKey);
        _lineObjects.Add(lineObject);
        //lineObject.transform.parent = this.transform;
        lineObject.transform.position = new Vector3(castEvent.Origin.X, castEvent.Origin.Y, this.transform.position.z);

        lineObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(raycast.FarthestPointReached.X - castEvent.Origin.X, raycast.FarthestPointReached.Y - castEvent.Origin.Y, 0));
        lineObject.GetComponent<AllegianceColorizer>().UpdateVisual(castEvent.AllegianceInfo);
    }

    public void Reset()
    {
        for (int i = 0; i < _lineObjects.Count; ++i)
        {
            ObjectPools.ReturnPooledObject(this.LineObjectKey, _lineObjects[i]);
        }

        _lineObjects.Clear();
    }

    public override void OnDestroy()
    {
        this.Reset();
        base.OnDestroy();
    }

    /**
     * Private
     */
    private List<GameObject> _lineObjects;
}
