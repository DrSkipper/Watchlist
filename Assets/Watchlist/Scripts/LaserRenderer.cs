using UnityEngine;
using System.Collections;

public class LaserRenderer : VoBehavior
{
    public GameObject LinePrefab;

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
        
        GameObject lineObject = Instantiate(LinePrefab, this.transform.position, Quaternion.identity) as GameObject;
        lineObject.transform.parent = this.transform;
        lineObject.transform.position = new Vector3(castEvent.Origin.X, castEvent.Origin.Y, this.transform.position.z);
        
        lineObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(raycast.FarthestPointReached.X - castEvent.Origin.X, raycast.FarthestPointReached.Y - castEvent.Origin.Y, 0));
    }
}
