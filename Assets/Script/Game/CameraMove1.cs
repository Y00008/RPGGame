using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove1 : MonoBehaviour {

      float distance;
      float moveSpeed = 3f;
      Transform player;
      float mousex;
      float mousey;
      float minAngle=5;
      float maxAngle = 180;
      Vector3 offset;
      float scrollSpeed = 10;
      PlayerAttack playerattack;
	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag(Tags.player).transform;
        offset = transform.position - player.position;
        playerattack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //打开了面板冻结移动
        if (UIManager.Instance.PanelSatck.Count > 1 || playerattack.state == PlayerState.Death)
        {
            return;
        }

        mousex += Input.GetAxis("Mouse X") * moveSpeed;
        mousey -= Input.GetAxis("Mouse Y") * moveSpeed;


        mousey = ClampAngle(mousey, minAngle, maxAngle);

        Quaternion cameramoveRotation = Quaternion.Euler(mousey, mousex, 0);

        distance = distance - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, 3, 7);//对拉近拉远做限制

        Vector3 cameraMovePosition = cameramoveRotation * new Vector3(0, 2, -distance) + player.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, cameramoveRotation, Time.deltaTime*moveSpeed);
        transform.position = Vector3.Lerp(transform.position, cameraMovePosition, Time.deltaTime * moveSpeed);
	}

    float ClampAngle(float angle,float min,float max)
    {
        if(angle<0)
        {
            angle = 0;
        }
        if(angle>30)
        {
            angle = 30;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
