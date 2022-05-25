﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Singleton
    public static GameManager instance;
    public MatchSetting setting;

    [SerializeField]
    private GameObject sceneCamera;

    void Awake ()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager");
        else
            instance = this;
    }

    public void SetSceneCameraActive (bool _isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(_isActive);
    }

    #region Player tracking
    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        string _playerID = "Player" + _netID;
        players.Add(_playerID, _player);

        _player.transform.name = _playerID;
    }

    public static void DeRegisterPlayer (string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

    //// Display Player with GUILayout
    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(100, 100, 200, 500));
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
