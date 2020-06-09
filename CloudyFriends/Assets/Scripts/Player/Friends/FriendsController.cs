using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsController : MonoBehaviour
{
	[Serializable]
	public class Settings {
	}
	
	public Settings settings;
	
	private GameObject currentPlayer;
	
    void Start()
    {
        SetCurrentPlayer(EntityManager.GetPlayer());
		currentPlayer.GetComponent<FriendController>().settings.active = true;
		EntityManager.AddOnPlayerChangeAction(() => SetCurrentPlayer(EntityManager.GetPlayer()));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				GameObject hitObj = hit.transform.gameObject;
				if(hitObj.GetComponent<FriendController>() != null && hitObj.GetComponent<FriendController>().settings.active){
					SetCurrentPlayer(hitObj);
				}
			}
		}	
    }
	
	private void SetCurrentPlayer(GameObject player){
		if(currentPlayer != null)
			currentPlayer.GetComponent<FriendController>().settings.isCurrentPlayer = false;
		
		EntityManager.SetPlayer(player);
		player.GetComponent<FriendController>().settings.isCurrentPlayer = true;
		currentPlayer = player;
	}
	
}
