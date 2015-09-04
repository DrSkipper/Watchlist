using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BulletController : VoBehavior
{
    public Vector2 Velocity = new Vector2();

    public void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + this.Velocity.x * Time.deltaTime, 
                                              this.transform.position.y, 
                                              this.transform.position.z + this.Velocity.y * Time.deltaTime);
    }
}
