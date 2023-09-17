using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour {

      float movespeed=5.0f;
      CharacterController charactercontroller;
      PlayerDir playerDir;
	// Use this for initialization
	void Start () {
        charactercontroller = this.GetComponent<CharacterController>();
        playerDir = this.gameObject.GetComponent<PlayerDir>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(playerDir.Targetpoint,transform.position)>0.3f)
        {
            charactercontroller.SimpleMove(transform.forward * movespeed);
        }
	}
}
