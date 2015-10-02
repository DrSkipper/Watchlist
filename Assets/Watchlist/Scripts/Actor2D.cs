using UnityEngine;
using System;
using System.Collections.Generic;

public class Actor2D : VoBehavior
{
    public Vector2 Velocity;
    public LayerMask HaltMovementMask;
    public LayerMask CollisionMask;

    public const float MAX_POSITION_INCREMENT = 1.0f;

    public virtual void Update()
    {
        if (this.Velocity.x != 0.0f || this.Velocity.y != 0.0f)
        {
            Move(this.Velocity * Time.deltaTime);
            //this.transform.position += new Vector3(this.Velocity.x * Time.deltaTime, this.Velocity.y * Time.deltaTime, 0);
        }
    }

    public void Move(Vector2 d)
    {
        float incX = d.x;
        float incY = d.y;

        if (Mathf.Abs(incX) > MAX_POSITION_INCREMENT || Mathf.Abs(incY) > MAX_POSITION_INCREMENT)
        {
            Vector2 dNormalized = d.normalized * MAX_POSITION_INCREMENT;
            incX = dNormalized.x;
            incY = dNormalized.y;
        }

        Vector2 soFar = Vector2.zero;
        Vector2 oldVelocity = this.Velocity;
        
        while (true)
        {
            bool haltX = moveX(incX, _collisionsFromMove);
            bool haltY = moveY(incY, _collisionsFromMove);

            if (haltX || haltY)
            {
                this.Velocity = Vector2.zero;
                break;
            }

            soFar.x += incX;
            soFar.y += incY;

            Vector2 difference = d - soFar;
            if (difference.magnitude == 0.0f)
                break;

            if (soFar.magnitude > d.magnitude)
            {
                moveX(d.x - soFar.x, _collisionsFromMove);
                moveY(d.y - soFar.y, _collisionsFromMove);
                break;
            }
        }

        if (_collisionsFromMove.Count > 0)
        {
            this.localNotifier.SendEvent(new CollisionEvent(_collisionsFromMove.ToArray(), oldVelocity));
            _collisionsFromMove.Clear();
        }
    }

    /**
     * Private
     */
    private Vector2 _positionModifier = Vector2.zero;
    private List<GameObject> _collisionsFromMove = new List<GameObject>();
    
    // Returns true if movement was halted
    private bool moveX(float dx, List<GameObject> collisions)
    {
        GameObject collidedObject = this.boxCollider2D.CollideFirst(dx, 0, this.CollisionMask);

        if (collidedObject)
        {
            if (!collisions.Contains(collidedObject))
                collisions.Add(collidedObject);

            if ((collidedObject.layer & this.HaltMovementMask) != 0)
            {
                _positionModifier.x = 0.0f;
                return true;
            }
        }

        this.transform.position += new Vector3(dx, 0, 0);

        return false;
    }
    
    private bool moveY(float dy, List<GameObject> collisions)
    {
        _positionModifier.y += dy;
        int unitMove = Mathf.RoundToInt(_positionModifier.y);

        if (unitMove != 0)
        {
            int unitDir = Math.Sign(unitMove);
            _positionModifier.y -= unitMove;

            while (unitMove != 0)
            {
                /*GameObject collidedObject = this.boxCollider2D.CollideFirst(0, unitDir, this.CollisionMask);

                if (collidedObject)
                {
                    collisions.Add(collidedObject);
                    if ((collidedObject.layer & this.HaltMovementMask) != 0)
                    {
                        _positionModifier.y = 0.0f;
                        return true;
                    }
                }*/

                this.transform.position += new Vector3(0, unitDir, 0);
                unitMove -= unitDir;
            }
        }

        return false;
    }
}
