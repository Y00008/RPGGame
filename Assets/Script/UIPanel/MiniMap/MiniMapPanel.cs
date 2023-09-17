using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPanel : MonoBehaviour {

      Camera miniCamera;
      Button maxBtn;
      Button minBtn;
	// Use this for initialization
	void Awake() {
        maxBtn = transform.Find("MaxBtn").GetComponent<Button>();
        minBtn = transform.Find("MinBtn").GetComponent<Button>();

        //miniCamera = GameObject.FindGameObjectWithTag(Tags.MiniMap).GetComponent<Camera>();
        miniCamera =GameObject.FindGameObjectWithTag(Tags.player).transform.Find("MiniCamera").GetComponent<Camera>();

        maxBtn.onClick.AddListener(OnClickMaxBtn);
        minBtn.onClick.AddListener(OnClickMinBtn);


	}


      void OnClickMinBtn()
    {
        miniCamera.orthographicSize--;
        miniCamera.orthographicSize = Mathf.Clamp(miniCamera.orthographicSize, 2.75f, 5.75f);
    }

      void OnClickMaxBtn()
    {
        miniCamera.orthographicSize++;
        miniCamera.orthographicSize = Mathf.Clamp(miniCamera.orthographicSize,2.75f, 5.75f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
