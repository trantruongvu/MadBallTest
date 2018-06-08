using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    private NetworkManager networkManager;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Text statusTxt;

    [SerializeField]
    private Transform roomListParent;

    // Start
    void Start ()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList ()
    {
        ClearRoomList();

        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        statusTxt.text = "Loading . . .";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        statusTxt.text = "";

        if(matchList == null)
        {
            statusTxt.text = "Couldn't get any room.";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            // Add component on gameObject 
            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);           
        }
        // When there is no room
        if (roomList.Count == 0)
        {
            statusTxt.text = "No room at the moment.";
        }
    }

    void ClearRoomList ()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]); 
        }
        roomList.Clear();
    }

    public void JoinRoom (MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        statusTxt.text = "Joining . . .";
    }
}
