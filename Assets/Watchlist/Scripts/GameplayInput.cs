using UnityEngine;
using Rewired;

public static class GameplayInput
{
    private const string MOVE_HORIZONTAL = "MoveHorizontal";
    private const string MOVE_VERTICAL = "MoveVertical";
    private const string AIM_HORIZONTAL = "AimHorizontal";
    private const string AIM_VERTICAL = "AimVertical";
    private const string FIRE = "Fire";

    public static Vector2 GetMovementAxis(int playerIndex, bool normalized = false)
    {
        Vector2 axis = ReInput.players.GetPlayer(playerIndex).GetAxis2D(MOVE_HORIZONTAL, MOVE_VERTICAL);
        if (normalized)
            axis.Normalize();
        return axis;
    }

    public static Vector2 GetAimingAxis(int playerIndex, bool normalized = true)
    {
        Vector2 axis = ReInput.players.GetPlayer(playerIndex).GetAxis2D(AIM_HORIZONTAL, AIM_VERTICAL);
        if (normalized)
            axis.Normalize();
        return axis;
    }

    public static bool GetFireButton(int playerIndex)
    {
        return ReInput.players.GetPlayer(playerIndex).GetButton(FIRE);
    }
}
