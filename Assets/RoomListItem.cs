using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallBack;

    [SerializeField]
    private Text roomNameTxt;

    private MatchInfoSnapshot match;

    public void Setup (MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallBack)
    {
        match = _match;
        joinRoomCallBack = _joinRoomCallBack;

        roomNameTxt.text = match.name + " (" + match.currentSize + " / " + match.maxSize + ")";
    }

    public void JoinRoom ()
    {
        joinRoomCallBack.Invoke(match);
    }
	
}
