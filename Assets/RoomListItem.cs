using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    private Text roomNameTxt;

    private MatchInfoSnapshot match;

    public void Setup (MatchInfoSnapshot _match)
    {
        match = _match;

        roomNameTxt.text = match.name + " (" + match.currentSize + " / " + match.maxSize + ")";
    }
	
}
