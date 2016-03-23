﻿using UnityEngine;
using Rewired;
using System.Collections.Generic;

public class ControllerAssigner : MonoBehaviour
{
    public const string ASSIGNMENT_CATEGORY = "Assignment";
    public const string JOIN_ACTION = "Join";
    public delegate void ControllerAssigned(SessionPlayer player);
    public delegate void ControllerUnassigned(SessionPlayer player);

    public bool AssignFirstPlayerOnStart = true;
    public bool AllowReassignment = false;
    public float UnassignButtonHoldDuration = 2.0f;

    void Start()
    {
        if (this.AssignFirstPlayerOnStart)
        {
            SessionPlayer p1 = DynamicData.GetSessionPlayer(0);
            if (!p1.HasJoined)
            {
                // Find most recent active input and join p1 with it
                Player lastActive = null;
                float lastActiveTime = 0.0f;
                foreach (Player p in ReInput.players.Players)
                {
                    Controller c = p.controllers.GetLastActiveController();
                    float cActiveTime = c.GetLastTimeActive();
                    if (lastActive == null || cActiveTime > lastActiveTime)
                    {
                        lastActiveTime = cActiveTime;
                        lastActive = p;
                    }
                }
                p1.JoinSession(lastActive.id);
            }
        }
    }

    void Update()
    {
        if (this.AllowReassignment)
        {
            for (int i = 0; i < DynamicData.MAX_PLAYERS; ++i)
            {
                SessionPlayer p = DynamicData.GetSessionPlayer(i);
                if (!p.HasJoined)
                {
                    // Assignment
                    foreach (Player rewiredPlayer in ReInput.players.Players)
                    {
                        if (!rewiredPlayer.isPlaying && rewiredPlayer.GetButtonDown(JOIN_ACTION))
                        {
                            joinPlayer(p, rewiredPlayer.id);
                            break;
                        }
                    }
                }
                else
                {
                    // Unassignment
                    if (ReInput.players.GetPlayer(p.RewiredId).GetButtonDown(MenuInput.EXIT))
                    {
                        //TODO - Should be checking if button held rather than just pressed
                        dropPlayer(p);
                    }
                }
            }
        }
    }

    public void AddAssignmentCallback(int playerIndex, ControllerAssigned callback)
    {
        if (_assignmentCallbacks == null)
            _assignmentCallbacks = new Dictionary<int, List<ControllerAssigned>>();
        if (!_assignmentCallbacks.ContainsKey(playerIndex))
            _assignmentCallbacks.Add(playerIndex, new List<ControllerAssigned>());
        _assignmentCallbacks[playerIndex].Add(callback);
    }

    public void AddUnassignmentCallback(int playerIndex, ControllerUnassigned callback)
    {
        if (_unassignmentCallbacks == null)
            _unassignmentCallbacks = new Dictionary<int, List<ControllerUnassigned>>();
        if (!_unassignmentCallbacks.ContainsKey(playerIndex))
            _unassignmentCallbacks.Add(playerIndex, new List<ControllerUnassigned>());
        _unassignmentCallbacks[playerIndex].Add(callback);
    }

    /**
     * Private
     */
    private List<Player> _gameplayPlayers;
    private Dictionary<int, List<ControllerAssigned>> _assignmentCallbacks;
    private Dictionary<int, List<ControllerUnassigned>> _unassignmentCallbacks;

    private void joinPlayer(SessionPlayer p, int rewiredId)
    {
        p.JoinSession(rewiredId);
        if (_assignmentCallbacks != null && _assignmentCallbacks.ContainsKey(p.PlayerIndex))
        {
            foreach (ControllerAssigned callback in _assignmentCallbacks[p.PlayerIndex])
            {
                callback(p);
            }
        }
    }

    private void dropPlayer(SessionPlayer p)
    {
        p.LeaveSession();
        if (_unassignmentCallbacks != null && _unassignmentCallbacks.ContainsKey(p.PlayerIndex))
        {
            foreach (ControllerUnassigned callback in _unassignmentCallbacks[p.PlayerIndex])
            {
                callback(p);
            }
        }
    }
}
