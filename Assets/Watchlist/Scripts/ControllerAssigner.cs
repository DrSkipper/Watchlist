using UnityEngine;
using Rewired;
using System.Collections.Generic;

public class ControllerAssigner : MonoBehaviour
{
    public delegate void ControllerAssigned(Player player, Controller controller);
    public delegate void ControllerUnassigned();

    public bool AssignFirstPlayerOnStart = true;
    public bool AllowReassignment = false;
    public float UnassignButtonHoldDuration = 2.0f;

    void Start()
    {
        Player menuPlayer = ReInput.players.GetPlayer(MenuInput.MENU_PLAYER);
        _gameplayPlayers = new List<Player>(ReInput.players.GetPlayers());
        _gameplayPlayers.Remove(menuPlayer);

        foreach (Player player in _gameplayPlayers)
            player.controllers.ClearAllControllers();

        if (this.AssignFirstPlayerOnStart)
        {
            if (playerUnassigned(_gameplayPlayers[0]))
            {
                Controller mostRecentController = menuPlayer.controllers.GetLastActiveController();
                if (mostRecentController == null)
                {
                    Controller[] controllers = ReInput.controllers.GetControllers(ControllerType.Joystick);
                    if (controllers.Length > 0)
                        mostRecentController = controllers[0];
                    else
                        mostRecentController = ReInput.controllers.Keyboard;
                }
                
                assignController(_gameplayPlayers[0], mostRecentController);
            }
        }
    }

    void Update()
    {
        if (this.AllowReassignment)
        {
            Controller[] joysticks = null;
            Keyboard keyboard = null;

            foreach (Player player in _gameplayPlayers)
            {
                // Check for assignment
                if (playerUnassigned(player))
                {
                    if (joysticks == null)
                    {
                        joysticks = ReInput.controllers.GetControllers(ControllerType.Joystick);
                        keyboard = ReInput.controllers.Keyboard;
                    }
                    
                    Controller availableController = findAvailableControllerWithStartHeld(joysticks, keyboard);

                    if (availableController != null)
                        assignController(player, availableController);
                    else
                        break;
                }

                // Check for unassignment
                else
                {

                }
            }
        }
    }

    public void AddAssignmentCallback(int playerId, ControllerAssigned callback)
    {
        if (_assignmentCallbacks == null)
            _assignmentCallbacks = new Dictionary<int, List<ControllerAssigned>>();
        if (!_assignmentCallbacks.ContainsKey(playerId))
            _assignmentCallbacks.Add(playerId, new List<ControllerAssigned>());
        _assignmentCallbacks[playerId].Add(callback);
    }

    public void AddUnassignmentCallback(int playerId, ControllerUnassigned callback)
    {
        if (_unassignmentCallbacks == null)
            _unassignmentCallbacks = new Dictionary<int, List<ControllerUnassigned>>();
        if (!_unassignmentCallbacks.ContainsKey(playerId))
            _unassignmentCallbacks.Add(playerId, new List<ControllerUnassigned>());
        _unassignmentCallbacks[playerId].Add(callback);
    }

    /**
     * Private
     */
    private List<Player> _gameplayPlayers;
    private Dictionary<int, List<ControllerAssigned>> _assignmentCallbacks;
    private Dictionary<int, List<ControllerUnassigned>> _unassignmentCallbacks;

    private bool playerUnassigned(Player player)
    {
        return player.controllers.joystickCount == 0 && !player.controllers.hasMouse;
    }

    private void assignController(Player player, Controller controller)
    {
        player.controllers.ClearAllControllers();
        player.controllers.AddController(controller, false);
        player.isPlaying = true;

        if (controller == ReInput.controllers.Keyboard)
            player.controllers.AddController(ReInput.controllers.Mouse, false);

        if (_assignmentCallbacks.ContainsKey(player.id))
        {
            foreach (ControllerAssigned callback in _assignmentCallbacks[player.id])
            {
                callback(player, controller);
            }
        }
    }

    private Controller findAvailableControllerWithStartHeld(Controller[] joysticks, Keyboard keyboard)
    {
        bool available;
        foreach (Controller controller in joysticks)
        {
            if (controller.GetAnyButtonDown())
            {
                available = true;
                foreach (Player player in _gameplayPlayers)
                {
                    if (player.controllers.ContainsController(controller))
                    {
                        available = false;
                        break;
                    }
                }
                if (available)
                    return controller;
            }
        }

        if (keyboard.PollForFirstKeyDown().success)
        {
            available = true;
            foreach (Player player in _gameplayPlayers)
            {
                if (player.controllers.hasMouse)
                {
                    available = false;
                    break;
                }
            }
            if (available)
                return keyboard;
        }
        return null;
    }
}
