using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanceplayerinfo : MonoBehaviour {

	// Use this for initialization
	void Awake() {
        Debug.Log("实例化角色");
        Debug.Log(PlayerPrefs.GetString("Character"));
        GameObject.Instantiate(Resources.Load<GameObject>(PlayerPrefs.GetString("Character") + "Game"));
        //Debug.Log(PlayerPrefs.GetString("Character"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
