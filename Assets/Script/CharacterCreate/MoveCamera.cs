using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

      float moveSpeed = 15f;
      float endPointz = 0.0f;
      bool isBtnShow = false;
      static MoveCamera instance;

    public static MoveCamera Instance
    {
        get { return instance; }
    }
    public  bool IsBtnShow
    {
        get { return isBtnShow; }
    }
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.z>=endPointz)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);    
        }
        else
        {
            isBtnShow = true;
        }
	}
}
