using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDir : MonoBehaviour {

      Vector3 targetpoint;
    public Vector3 Targetpoint
    {
        get { return targetpoint; }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if(Physics.Raycast(ray,out hitinfo)&&hitinfo.collider.tag.Equals(Tags.ground))
            {
                Debug.Log("点击地面");
                LookAtTarget(hitinfo.point);
            }
        }
	}
    void LookAtTarget(Vector3 hitpoint)
    { 
        targetpoint = new Vector3(hitpoint.x, transform.position.y, hitpoint.z);
        transform.LookAt(targetpoint);
    }
}
