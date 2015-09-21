using UnityEngine;

public class PlayerSpriteController : VoBehavior
{
    public Sprite IdlingSprite;
    public Sprite CardinalDirectionSprite;
    public Sprite DiagonalSprite;
    public float CardinalCutoff = 2.0f;

    void Start()
    {
        _playerController = this.GetComponent<PlayerController>();
        _inverseCardinalCutoff = 1.0f / 2.0f;
    }

    void Update()
    {
        Vector2 movementAxis = GameplayInput.GetMovementAxis();

        // Detect if we're moving enough for sprite change
        if (Mathf.Abs(movementAxis.x) > 0.001f || Mathf.Abs(movementAxis.y) > 0.001f)
        {
            Vector3 depth = _playerController.Up.y > 0 ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);

            // Detect if predominantly cardinal or diagonal movement
            float xyRatio = movementAxis.x / movementAxis.y;
            if (xyRatio >= this.CardinalCutoff || xyRatio <= _inverseCardinalCutoff)
            {
                // cardinal
                if (this.spriteRenderer.sprite != this.CardinalDirectionSprite)
                    this.spriteRenderer.sprite = this.CardinalDirectionSprite;

                float rotation = Mathf.Atan2(movementAxis.y, movementAxis.x) * 180.0f / Mathf.PI + -90.0f;
                float rotationMod = rotation % 90.0f;
                this.transform.rotation = Quaternion.AngleAxis(rotation - rotationMod, depth);
            }
            else if (this.DiagonalSprite != null)
            {
                // diagonal
                if (this.spriteRenderer.sprite != this.DiagonalSprite)
                    this.spriteRenderer.sprite = this.DiagonalSprite;
            }
        }
        else
        {
            // idle
            if (this.spriteRenderer.sprite != this.IdlingSprite)
                this.spriteRenderer.sprite = this.IdlingSprite;
        }
	}

    /**
     * Private
     */
    private PlayerController _playerController;
    private float _inverseCardinalCutoff;
}
