﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BulletController : VoBehavior
{
    public Vector2 Velocity = new Vector2();
    public float Duration = 5.0f;

    public void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + this.Velocity.x * Time.deltaTime, 
                                              this.transform.position.y, 
                                              this.transform.position.z + this.Velocity.y * Time.deltaTime);
        _lifetime += Time.deltaTime;
        if (_lifetime >= this.Duration)
            Destroy(this.gameObject);
    }

    /**
     * Private
     */
    public float _lifetime = 0.0f;
}
