using UnityEngine;

public class GameplayPalette : MonoBehaviour
{
    public Color[] PlayerColor;
    public Color[] PlayerProjectileColor;
    public Color[] PlayerExplosionColor;
    public Color[] PlayerDamagedColor;
    
    public Color EnemyColor;
    public Color EnemyProjectileColor;
    public Color EnemyExplosionColor;
    public Color EnemyDamagedColor;

    public Color WallColor;
    public Color PickupColor;
    public Color BackgroundColor;

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

        _enemyColor = this.EnemyColor;
        _enemyProjectileColor = this.EnemyProjectileColor;
        _enemyExplosionColor = this.EnemyExplosionColor;
        _enemyDamagedColor = this.EnemyDamagedColor;

        _wallColor = this.WallColor;
        _pickupColor = this.PickupColor;
    }

    public static Color GetPlayerColor(int playerNum)
    {
        return _playerColor[playerNum];
    }

    public static Color GetPlayerProjectileColor(int playerNum)
    {
        return _playerProjectileColor[playerNum];
    }

    public static Color GetPlayerExplosionColor(int playerNum)
    {
        return _playerExplosionColor[playerNum];
    }

    public static Color GetPlayerDamagedColor(int playerNum)
    {
        return _playerDamagedColor[playerNum];
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
            case "enemy":
                return GetEnemyColor();
            case "enemy_projectile":
                return GetEnemyProjectileColor();
            case "enemy_explosion":
                return GetEnemyExplosionColor();
            case "enemy_damaged":
                return GetEnemyDamagedColor();
            case "wall":
                return GetWallColor();
            case "pickup":
                return GetPickupColor();
            case "background":
                return GetBackgroundColor();
            default:
                return Color.white;
        }
    }

    public static Color GetColorForAllegiance(AllegianceInfo allegianceInfo, ColorPaletteClass colorClass)
    {
        string colorClassAddition = "";

        switch (colorClass)
        {
            default:
            case ColorPaletteClass.Main:
                colorClassAddition = "";
                break;
            case ColorPaletteClass.Projectile:
                colorClassAddition = "_projectile";
                break;
            case ColorPaletteClass.Explosion:
                colorClassAddition = "_explosion";
                break;
            case ColorPaletteClass.Damaged:
                colorClassAddition = "_damaged";
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

    private static Color _enemyColor;
    private static Color _enemyProjectileColor;
    private static Color _enemyExplosionColor;
    private static Color _enemyDamagedColor;

    private static Color _wallColor;
    private static Color _pickupColor;
    private static Color _backgroundColor;
}
