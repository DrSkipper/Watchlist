using Rewired;

public class SessionPlayer
{
    public int PlayerIndex { get { return _playerIndex; } }
    public int RewiredId { get { return _rewiredId; } }
    public bool HasJoined { get { return _rewiredId != -1; } }

    public SessionPlayer(int playerIndex)
    {
        _playerIndex = playerIndex;
        _rewiredId = -1;
    }

    public void JoinSession(int rewiredId)
    {
        _rewiredId = rewiredId;
        ReInput.players.GetPlayer(rewiredId).isPlaying = true;
    }

    public void LeaveSession()
    {
        ReInput.players.GetPlayer(_rewiredId).isPlaying = false;
        _rewiredId = -1;
    }

    private int _playerIndex;
    private int _rewiredId;
}
