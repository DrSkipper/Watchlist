using UnityEngine;

[ExecuteInEditMode]
public class GameplayPalette : MonoBehaviour
{
    public Color[] PlayerColor;
    public Color[] PlayerProjectileColor;
    public Color[] PlayerExplosionColor;
    public Color[] PlayerDamagedColor;
    public Color[] PlayerUIPrimaryColor;
    public Color[] PlayerUISecondaryColor;
    
    public Color EnemyColor;
    public Color EnemyProjectileColor;
    public Color EnemyExplosionColor;
    public Color EnemyDamagedColor;
    public Color EnemyGibsColor;

    public Color WallColor;
    public Color PickupColor;
    public Color BackgroundColor;

    public Color UIPrimaryColor;
    public Color UISecondaryColor;

    public Color TextColor;

    void Awake()
    {
        _playerColor = new Color[this.PlayerColor.Length];
        this.PlayerColor.CopyTo(_playerColor, 0);

        _playerProjectileColor = new Color[this.PlayerProjectileColor.Length];
        this.PlayerProjectileColor.CopyTo(_playerProjectileColor, 0);

        _playerExplosionColor = new Color[this.PlayerExplosionColor.Length];
        this.PlayerExplosionColor.CopyTo(_playerExplosionColor, 0);

        _playerDamagedColor = new Color[this.PlayerDamagedColor.Length];
        this.PlayerDamagedColor.CopyTo(_playerDamagedColor, 0);

        _playerUIPrimaryColor = new Color[this.PlayerUIPrimaryColor.Length];
        this.PlayerUIPrimaryColor.CopyTo(_playerUIPrimaryColor, 0);

        _playerUISecondaryColor = new Color[this.PlayerUISecondaryColor.Length];
        this.PlayerUISecondaryColor.CopyTo(_playerUISecondaryColor, 0);

        _enemyColor = this.EnemyColor;
        _enemyProjectileColor = this.EnemyProjectileColor;
        _enemyExplosionColor = this.EnemyExplosionColor;
        _enemyDamagedColor = this.EnemyDamagedColor;
        _enemyGibsColor = this.EnemyGibsColor;

        _wallColor = this.WallColor;
        _pickupColor = this.PickupColor;
        _backgroundColor = this.BackgroundColor;

        _uiPrimaryColor = this.UIPrimaryColor;
        _uiSecondaryColor = this.UISecondaryColor;
        _textColor = this.TextColor;
    }

    public static Color GetPlayerColor(int playerNum)
    {
        if (playerNum < _playerColor.Length)
            return _playerColor[playerNum];
        return _playerColor[0];
    }

    public static Color GetPlayerProjectileColor(int playerNum)
    {
        if (playerNum < _playerProjectileColor.Length)
            return _playerProjectileColor[playerNum];
        return _playerProjectileColor[0];
    }

    public static Color GetPlayerExplosionColor(int playerNum)
    {
        if (playerNum < _playerExplosionColor.Length)
            return _playerExplosionColor[playerNum];
        return _playerExplosionColor[0];
    }

    public static Color GetPlayerDamagedColor(int playerNum)
    {
        if (playerNum < _playerDamagedColor.Length)
            return _playerDamagedColor[playerNum];
        return _playerDamagedColor[0];
    }

    public static Color GetPlayerUIPrimaryColor(int playerNum)
    {
        if (playerNum < _playerUIPrimaryColor.Length)
            return _playerUIPrimaryColor[playerNum];
        return _playerUIPrimaryColor[0];
    }

    public static Color GetPlayerUISecondaryColor(int playerNum)
    {
        if (playerNum < _playerUISecondaryColor.Length)
            return _playerUISecondaryColor[playerNum];
        return _playerUISecondaryColor[0];
    }

    public static Color GetEnemyColor()
    {
        return _enemyColor;
    }

    public static Color GetEnemyProjectileColor()
    {
        return _enemyProjectileColor;
    }

    public static Color GetEnemyExplosionColor()
    {
        return _enemyExplosionColor;
    }

    public static Color GetEnemyDamagedColor()
    {
        return _enemyDamagedColor;
    }

    public static Color GetEnemyGibsColor()
    {
        return _enemyGibsColor;
    }

    public static Color GetWallColor()
    {
        return _wallColor;
    }
    
    public static Color GetPickupColor()
    {
        return _pickupColor;
    }

    public static Color GetBackgroundColor()
    {
        return _backgroundColor;
    }

    public static Color GetPrimaryUIColor()
    {
        return _uiPrimaryColor;
    }

    public static Color GetSecondaryUIColor()
    {
        return _uiSecondaryColor;
    }

    public static Color GetTextColor()
    {
        return _textColor;
    }

    public static Color GetColorFromTag(string tag, int parameter = 0)
    {
        switch (tag.ToLower().Replace(' ', '_'))
        {
            case "player":
                return GetPlayerColor(parameter);
            case "player_projectile":
                return GetPlayerProjectileColor(parameter);
            case "player_explosion":
                return GetPlayerExplosionColor(parameter);
            case "player_damaged":
                return GetPlayerDamagedColor(parameter);
            case "player_gibs":
                return GetPlayerColor(parameter); //TODO ?
            case "player_uiprimary":
                return GetPlayerUIPrimaryColor(parameter);
            case "player_uisecondary":
                return GetPlayerUISecondaryColor(parameter);
            case "enemy":
                return GetEnemyColor();
            case "enemy_projectile":
                return GetEnemyProjectileColor();
            case "enemy_explosion":
                return GetEnemyExplosionColor();
            case "enemy_damaged":
                return GetEnemyDamagedColor();
            case "enemy_gibs":
                return GetEnemyGibsColor();
            case "wall":
                return GetWallColor();
            case "pickup":
                return GetPickupColor();
            case "background":
                return GetBackgroundColor();
            case "ui_primary":
                return GetPrimaryUIColor();
            case "ui_secondary":
                return GetSecondaryUIColor();
            case "text":
                return GetTextColor();
            default:
                return Color.white;
        }
    }

    public static Color GetColorForAllegiance(AllegianceInfo allegianceInfo, ColorPaletteState colorClass)
    {
        string colorClassAddition = "";

        switch (colorClass)
        {
            default:
            case ColorPaletteState.Main:
                colorClassAddition = "";
                break;
            case ColorPaletteState.Projectile:
                colorClassAddition = "_projectile";
                break;
            case ColorPaletteState.Explosion:
                colorClassAddition = "_explosion";
                break;
            case ColorPaletteState.Damaged:
                colorClassAddition = "_damaged";
                break;
            case ColorPaletteState.Gibs:
                colorClassAddition = "_gibs";
                break;
            case ColorPaletteState.UIPrimary:
                colorClassAddition = "_uiprimary";
                break;
            case ColorPaletteState.UISecondary:
                colorClassAddition = "_uisecondary";
                break;
        }

        switch (allegianceInfo.Allegiance)
        {
            case Allegiance.Player:
                return GetColorFromTag("player" + colorClassAddition, allegianceInfo.MemberId);
            case Allegiance.Enemy:
                return GetColorFromTag("enemy" + colorClassAddition, allegianceInfo.MemberId);
            default:
                return Color.white;
        }
    }

    /**
     * Private static
     */
    private static Color[] _playerColor;
    private static Color[] _playerProjectileColor;
    private static Color[] _playerExplosionColor;
    private static Color[] _playerDamagedColor;
    private static Color[] _playerUIPrimaryColor;
    private static Color[] _playerUISecondaryColor;

    private static Color _enemyColor;
    private static Color _enemyProjectileColor;
    private static Color _enemyExplosionColor;
    private static Color _enemyDamagedColor;
    private static Color _enemyGibsColor;

    private static Color _wallColor;
    private static Color _pickupColor;
    private static Color _backgroundColor;

    private static Color _uiPrimaryColor;
    private static Color _uiSecondaryColor;
    private static Color _textColor;
}
