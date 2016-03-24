using UnityEngine;
using Rewired;

public static class GameplayInput
{
    private const string MOVE_HORIZONTAL = "MoveHorizontal";
    private const string MOVE_VERTICAL = "MoveVertical";
    private const string AIM_HORIZONTAL = "AimHorizontal";
    private const string AIM_VERTICAL = "AimVertical";
    private const string FIRE = "Fire";
    private const float MAX_MOUSE_EXTENSION_RATIO = 0.3f;

    public static Vector2 GetMovementAxis(int playerIndex, bool normalized = false)
    {
        SessionPlayer p = DynamicData.GetSessionPlayer(playerIndex);
        Vector2 axis = ReInput.players.GetPlayer(p.RewiredId).GetAxis2D(MOVE_HORIZONTAL, MOVE_VERTICAL);
        if (normalized)
            axis.Normalize();
        return axis;
    }

    public static Vector2 GetAimingAxis(int playerIndex, Vector2 playerWorldPosition, bool normalized = true)
    {
        SessionPlayer p = DynamicData.GetSessionPlayer(playerIndex);
        Player rewiredP = ReInput.players.GetPlayer(p.RewiredId);
        Vector2 axis;

        if (rewiredP.controllers.hasMouse)
        {
            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
            axis = rewiredP.controllers.Mouse.screenPosition - playerScreenPosition;

            if (!normalized)
            {
                float maxMouseExtension = MAX_MOUSE_EXTENSION_RATIO * Screen.width;
                if (axis.magnitude > maxMouseExtension)
                    axis = axis.normalized * maxMouseExtension;
                axis /= maxMouseExtension;
            }
        }
        else
        {
            axis = rewiredP.GetAxis2D(AIM_HORIZONTAL, AIM_VERTICAL);
        }

        if (normalized)
            axis.Normalize();
        return axis;
    }

    public static bool GetFireButton(int playerIndex)
    {
        SessionPlayer p = DynamicData.GetSessionPlayer(playerIndex);
        return ReInput.players.GetPlayer(p.RewiredId).GetButton(FIRE);
    }
}
