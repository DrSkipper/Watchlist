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
            Move(this.Velocity * Time.deltaTime);
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
        Vector2 projected = Vector2.zero;
        Vector2 oldVelocity = this.Velocity;
        float dMagnitude = d.magnitude;

        bool haltX = false;
        bool haltY = false;

        while (true)
        {
            projected.x += incX;
            projected.y += incY;

            if (projected.magnitude > dMagnitude)
            {
                if (!haltX) moveX(d.x - soFar.x, _collisionsFromMove, _horizontalCollisions);
                if (!haltY) moveY(d.y - soFar.y, _collisionsFromMove, _verticalCollisions);
                break;
            }

            if (!haltX) haltX = moveX(incX, _collisionsFromMove, _horizontalCollisions);
            if (!haltY) haltY = moveY(incY, _collisionsFromMove, _verticalCollisions);

            if (haltX && haltY)
                break;

            soFar.x += incX;
            soFar.y += incY;

            Vector2 difference = d - soFar;
            if (soFar.magnitude >= dMagnitude)
                break;
        }

        if (haltX) this.Velocity.x = 0;
        if (haltY) this.Velocity.y = 0;

        if (_collisionsFromMove.Count > 0)
        {
            this.localNotifier.SendEvent(new CollisionEvent(_collisionsFromMove.ToArray(), oldVelocity));
            _collisionsFromMove.Clear();
            _horizontalCollisions.Clear();
            _verticalCollisions.Clear();
        }
    }

    /**
     * Private
     */
    private Vector2 _positionModifier = Vector2.zero;
    private List<GameObject> _collisionsFromMove = new List<GameObject>();
    private List<GameObject> _horizontalCollisions = new List<GameObject>();
    private List<GameObject> _verticalCollisions = new List<GameObject>();
    
    // Returns true if movement was halted
    private bool moveX(float dx, List<GameObject> collisions, List<GameObject> horizontalCollisions)
    {
        _positionModifier.x += dx;
        int unitMove = Mathf.RoundToInt(_positionModifier.x);

        if (unitMove != 0)
        {
            int unitDir = Math.Sign(unitMove);
            _positionModifier.x -= unitMove;

            while (unitMove != 0)
            {
                int oldCount = horizontalCollisions.Count;
                this.boxCollider2D.Collide(horizontalCollisions, unitDir, 0, this.CollisionMask);

                if (horizontalCollisions.Count > oldCount)
                {
                    for (int i = oldCount; i < horizontalCollisions.Count; ++i)
                    {
                        if (!collisions.Contains(horizontalCollisions[i]))
                            collisions.Add(horizontalCollisions[i]);

                        if (((1 << horizontalCollisions[i].layer) & this.HaltMovementMask) != 0)
                        {
                            _positionModifier.x = 0.0f;
                            return true;
                        }
                    }
                }

                this.transform.position += new Vector3(unitDir, 0, 0);
                unitMove -= unitDir;
            }
        }

        return false;
    }
    
    private bool moveY(float dy, List<GameObject> collisions, List<GameObject> verticalCollisions)
    {
        _positionModifier.y += dy;
        int unitMove = Mathf.RoundToInt(_positionModifier.y);

        if (unitMove != 0)
        {
            int unitDir = Math.Sign(unitMove);
            _positionModifier.y -= unitMove;

            while (unitMove != 0)
            {
                int oldCount = verticalCollisions.Count;
                this.boxCollider2D.Collide(verticalCollisions, 0, unitDir, this.CollisionMask);

                if (verticalCollisions.Count > oldCount)
                {
                    for (int i = oldCount; i < verticalCollisions.Count; ++i)
                    {
                        if (!collisions.Contains(verticalCollisions[i]))
                            collisions.Add(verticalCollisions[i]);

                        if (((1 << collisions[i].layer) & this.HaltMovementMask) != 0)
                        {
                            _positionModifier.y = 0.0f;
                            return true;
                        }
                    }
                }

                this.transform.position += new Vector3(0, unitDir, 0);
                unitMove -= unitDir;
            }
        }

        return false;
    }
}
