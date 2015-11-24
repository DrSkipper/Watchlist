﻿using UnityEngine;

[ExecuteInEditMode]
public class AllegianceColorizer : VoBehavior
{
    public AllegianceInfo AllegianceInfo;
    public ColorPaletteState DefaultColorState;

    void Start()
    {
        if (this.spriteRenderer != null)
        {
            _colorSetter = setSpriteColor;
        }
        else
        {
            _lineRenderer = this.GetComponent<LineRenderer>();
            if (_lineRenderer != null)
            {
                _colorSetter = setLineColor;
            }
            else
            {
                _camera = this.GetComponent<Camera>();
                if (_camera != null)
                    _colorSetter = setSkyboxColor;
                else
                    _colorSetter = emptyColorSetter;
            }
        }

        this.UpdateVisual(this.AllegianceInfo);
    }

    public void UpdateVisual(AllegianceInfo allegianceInfo)
    {
        this.UpdateVisual(allegianceInfo, this.DefaultColorState);
    }

    public void UpdateVisual(AllegianceInfo allegianceInfo, ColorPaletteState colorState)
    {
        this.AllegianceInfo = allegianceInfo;
        if (_colorSetter != null)
            _colorSetter(GameplayPalette.GetColorForAllegiance(allegianceInfo, colorState));
    }

    public void UpdateVisual(ColorPaletteState colorState)
    {
        if (_colorSetter != null)
            _colorSetter(GameplayPalette.GetColorForAllegiance(this.AllegianceInfo, colorState));
    }

    /**
     * Private
     */
    private delegate void ColorSetter(Color color);
    private ColorSetter _colorSetter;
    private LineRenderer _lineRenderer;
    private Camera _camera;

    private void setSpriteColor(Color color)
    {
        this.spriteRenderer.color = color;
    }

    private void setLineColor(Color color)
    {
        _lineRenderer.SetColors(color, color);
    }

    private void setSkyboxColor(Color color)
    {
        _camera.backgroundColor = color;
    }

    private void emptyColorSetter(Color color)
    {
    }
}
