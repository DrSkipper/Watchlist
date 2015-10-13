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

        List<IntegerCollider> potentialCollisions = null;

        if (Mathf.Abs(incX) > MAX_POSITION_INCREMENT || Mathf.Abs(incY) > MAX_POSITION_INCREMENT)
        {
            Vector2 dNormalized = d.normalized * MAX_POSITION_INCREMENT;
            incX = dNormalized.x;
            incY = dNormalized.y;

            potentialCollisions = this.integerCollider.GetPotentialCollisions(d.x, d.y, 0, 0, this.CollisionMask);
        }

        Vector2 soFar = Vector2.zero;
        Vector2 projected = Vector2.zero;
        Vector2 oldVelocity = this.Velocity;
        float dMagnitude = d.magnitude;

        while (true)
        {
            projected.x += incX;
            projected.y += incY;

            if (projected.magnitude > dMagnitude)
            {
                if (!_haltX) moveX(d.x - soFar.x, _collisionsFromMove, _horizontalCollisions, potentialCollisions);
                if (!_haltY) moveY(d.y - soFar.y, _collisionsFromMove, _verticalCollisions, potentialCollisions);
                soFar = d;
                break;
            }

            if (!_haltX) soFar.x += moveX(incX, _collisionsFromMove, _horizontalCollisions, potentialCollisions);
            if (!_haltY) soFar.y += moveY(incY, _collisionsFromMove, _verticalCollisions, potentialCollisions);

            if (_haltX && _haltY)
                break;
            
            if (soFar.magnitude >= dMagnitude)
                break;
        }

        if (_haltX) this.Velocity.x = 0;
        if (_haltY) this.Velocity.y = 0;

        if (_collisionsFromMove.Count > 0)
        {
            GameObject[] collisions = _collisionsFromMove.ToArray();
            _haltX = false;
            _haltY = false;
            _collisionsFromMove.Clear();
            _horizontalCollisions.Clear();
            _verticalCollisions.Clear();
            this.localNotifier.SendEvent(new CollisionEvent(collisions, oldVelocity, soFar));
        }
    }

    /**
     * Private
     */
    private Vector2 _positionModifier = Vector2.zero;
    private List<GameObject> _collisionsFromMove = new List<GameObject>();
    private List<GameObject> _horizontalCollisions = new List<GameObject>();
    private List<GameObject> _verticalCollisions = new List<GameObject>();
    private bool _haltX;
    private bool _haltY;
    
    // Returns actual amount applied to movement
    private float moveX(float dx, List<GameObject> collisions, List<GameObject> horizontalCollisions, List<IntegerCollider> potentialCollisions)
    {
        _positionModifier.x += dx;
        int unitMove = Mathf.RoundToInt(_positionModifier.x);

        if (unitMove != 0)
        {
            int moves = 0;
            int unitDir = Math.Sign(unitMove);
            _positionModifier.x -= unitMove;

            while (unitMove != 0)
            {
                int oldCount = horizontalCollisions.Count;
                this.integerCollider.Collide(horizontalCollisions, unitDir, 0, this.CollisionMask, null, potentialCollisions);

                if (horizontalCollisions.Count > oldCount)
                {
                    for (int i = oldCount; i < horizontalCollisions.Count; ++i)
                    {
                        collisions.AddUnique(horizontalCollisions[i]);

                        if (((1 << horizontalCollisions[i].layer) & this.HaltMovementMask) != 0)
                        {
                            _positionModifier.x = 0.0f;
                            _haltX = true;
                            return moves;
                        }
                    }
                }

                this.transform.position += new Vector3(unitDir, 0, 0);
                unitMove -= unitDir;
                ++moves;
            }
        }

        return dx;
    }
    
    private float moveY(float dy, List<GameObject> collisions, List<GameObject> verticalCollisions, List<IntegerCollider> potentialCollisions)
    {
        _positionModifier.y += dy;
        int unitMove = Mathf.RoundToInt(_positionModifier.y);

        if (unitMove != 0)
        {
            int moves = 0;
            int unitDir = Math.Sign(unitMove);
            _positionModifier.y -= unitMove;

            while (unitMove != 0)
            {
                int oldCount = verticalCollisions.Count;
                this.integerCollider.Collide(verticalCollisions, 0, unitDir, this.CollisionMask, null, potentialCollisions);

                if (verticalCollisions.Count > oldCount)
                {
                    for (int i = oldCount; i < verticalCollisions.Count; ++i)
                    {
                        collisions.AddUnique(verticalCollisions[i]);

                        if (((1 << verticalCollisions[i].layer) & this.HaltMovementMask) != 0)
                        {
                            _positionModifier.y = 0.0f;
                            _haltY = true;
                            return moves;
                        }
                    }
                }

                this.transform.position += new Vector3(0, unitDir, 0);
                unitMove -= unitDir;
                ++moves;
            }
        }

        return dy;
    }
}
