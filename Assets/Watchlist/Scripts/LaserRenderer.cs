using UnityEngine;
using System.Collections;

public class LaserRenderer : VoBehavior
{
    public GameObject LinePrefab;
    public string LineObjectKey = "laser_line";

    void Awake()
    {
        this.localNotifier.Listen(LaserCastEvent.NAME, this, this.OnLaserCast);
    }

    public void OnLaserCast(LocalEventNotifier.Event localEvent)
    {
        LaserCastEvent castEvent = (LaserCastEvent)localEvent;
        CollisionManager.RaycastResult raycast = castEvent.RaycastResult;

        IntegerVector distance = raycast.FarthestPointReached - castEvent.Origin;
        if (distance.X == 0 && distance.Y == 0)
            return;

        //_lineObject = Instantiate(LinePrefab, this.transform.position, Quaternion.identity) as GameObject;
        _lineObject = ObjectPools.GetPooledObject(this.LineObjectKey);
        //lineObject.transform.parent = this.transform;
        _lineObject.transform.position = new Vector3(castEvent.Origin.X, castEvent.Origin.Y, this.transform.position.z);

        _lineObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(raycast.FarthestPointReached.X - castEvent.Origin.X, raycast.FarthestPointReached.Y - castEvent.Origin.Y, 0));
        _lineObject.GetComponent<AllegianceColorizer>().UpdateVisual(castEvent.AllegianceInfo);
    }

    public void Reset()
    {
        if (_lineObject != null)
            ObjectPools.ReturnPooledObject(this.LineObjectKey, _lineObject);
        _lineObject = null;
    }

    public override void OnDestroy()
    {
        if (_lineObject != null)
            ObjectPools.ReturnPooledObject(this.LineObjectKey, _lineObject);
        _lineObject = null;
        base.OnDestroy();
    }

    /**
     * Private
     */
    private GameObject _lineObject;
}
