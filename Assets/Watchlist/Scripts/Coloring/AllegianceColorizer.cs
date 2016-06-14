using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AllegianceColorizer : VoBehavior
{
    public AllegianceInfo AllegianceInfo;
    public ColorPaletteState DefaultColorState;
    public AllegianceColorizer[] DependentColorizers;

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
                {
                    _colorSetter = setSkyboxColor;
                }
                else
                {
                    _image = this.GetComponent<Image>();
                    if (_image != null)
                    {
                        _colorSetter = setImageColor;
                    }
                    else
                    {
                        _text = this.GetComponent<Text>();
                        if (_text != null)
                            _colorSetter = setTextColor;
                        else
                            _colorSetter = emptyColorSetter;
                    }
                }
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

        if (this.DependentColorizers != null)
        {
            foreach (AllegianceColorizer dependent in this.DependentColorizers)
            {
                AllegianceInfo info = dependent.AllegianceInfo;
                info.Allegiance = allegianceInfo.Allegiance;
                info.MemberId = allegianceInfo.MemberId;
                dependent.UpdateVisual(info);
            }
        }

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
    private Image _image;
    private Text _text;

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

    private void setImageColor(Color color)
    {
        _image.color = color;
    }

    private void setTextColor(Color color)
    {
        _text.color = color;
    }

    private void emptyColorSetter(Color color)
    {
    }
}
