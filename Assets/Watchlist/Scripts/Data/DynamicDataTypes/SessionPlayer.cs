using Rewired;

public class SessionPlayer
{
    public int PlayerIndex { get { return _playerIndex; } }
    public int RewiredId { get { return _rewiredId; } }
    public bool HasJoined { get { return _rewiredId != -1; } }
    public string Name { get { return "P" + (_playerIndex + 1); } }
    public bool UsingJoystick { get { return _usingJoystick; } }

    public SessionPlayer(int playerIndex)
    {
        _playerIndex = playerIndex;
        _rewiredId = -1;
        _usingJoystick = false;
    }

    public void JoinSession(int rewiredId)
    {
        _rewiredId = rewiredId;
        Player p = ReInput.players.GetPlayer(rewiredId);
        p.isPlaying = true;

        if (p.controllers.joystickCount > 0)
            _usingJoystick = true;
    }

    public void LeaveSession()
    {
        ReInput.players.GetPlayer(_rewiredId).isPlaying = false;
        _rewiredId = -1;
        _usingJoystick = false;
    }

    private int _playerIndex;
    private int _rewiredId;
    private bool _usingJoystick;
}
