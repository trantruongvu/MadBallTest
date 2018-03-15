using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // singleton
    public static GameController instance;

    public MatchSetting matchSetting;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.Log("More than one GameController");
        }
        else
        {
            instance = this;
        }
    }

    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        // Add player to Dictionary
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        // Display name on Hierarchy
        _player.transform.name = _playerID;
    }

    public static void DeRegisterPlayer (string _playerID)
    {
        players.Remove(_playerID);
    }

    // Get player
    public static Player GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

    // Display Player with GUILayout
    //void OnGUI ()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string _playerID in players.Keys)
    //    {
    //        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    #endregion


}
