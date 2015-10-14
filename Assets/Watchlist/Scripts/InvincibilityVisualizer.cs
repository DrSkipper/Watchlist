using UnityEngine;

public class InvincibilityVisualizer : VoBehavior
{
    public Color FlashColor = Color.red;
    public float FlashOnDuration = 0.2f;
    public float FlashOffDuration = 0.2f;

    void Start()
    {
        this.localNotifier.Listen(InvincibilityToggleEvent.NAME, this, this.OnInvincibilityToggle);
    }

    void Update()
    {
        if (_activated)
        {
            if (_blinkTimer <= 0.0f)
            {
                if (_blinking)
                    flashOff();
                else
                    flashOn();
            }

            _blinkTimer -= Time.deltaTime;
        }
    }

    public void OnInvincibilityToggle(LocalEventNotifier.Event localEvent)
    {
        _activated = ((InvincibilityToggleEvent)localEvent).ToggledOn;

        if (_activated)
        {
            _normalColor = this.spriteRenderer.color;
            flashOn();
        }
        else
        {
            _blinking = false;
            this.spriteRenderer.color = _normalColor;
            _blinkTimer = 0.0f;
        }
    }

    /**
     * Private
     */
    private bool _activated;
    private bool _blinking;
    private Color _normalColor;
    private float _blinkTimer;

    private void flashOn()
    {
        _blinking = true;
        this.spriteRenderer.color = this.FlashColor;
        _blinkTimer = this.FlashOnDuration;
    }

    private void flashOff()
    {
        _blinking = false;
        this.spriteRenderer.color = _normalColor;
        _blinkTimer = this.FlashOffDuration;
    }
}
