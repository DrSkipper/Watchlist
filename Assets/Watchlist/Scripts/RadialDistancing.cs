using UnityEngine;
using System.Collections.Generic;

public class RadialDistancing : VoBehavior
{
    [System.Serializable]
    public struct RadialElement
    {
        public Transform transform;
        public float desiredAngle;
    }

    public List<RadialElement> RadialElements;
    public float Radius = 300.0f;
    public float RadiusChangeSpeed = -100.0f;
    public bool Running = true;

    void Update()
    {
        if (this.Running)
        {
            this.Radius += this.RadiusChangeSpeed * Time.deltaTime;

            foreach (RadialElement element in this.RadialElements)
            {
                element.transform.localPosition = new Vector3(this.Radius * Mathf.Cos(element.desiredAngle * Mathf.Deg2Rad), this.Radius * Mathf.Sin(element.desiredAngle * Mathf.Deg2Rad), element.transform.localPosition.z);
            }
        }
    }
}
