using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Signinitem : MonoBehaviour {

    public signinfo info;
    private Text dateLabel;
    private Image icon;
    private Text num;
    public Transform mask;
	// Use this for initialization
	void Awake () {
        dateLabel = transform.Find("Data").GetComponent<Text>();
        icon = transform.Find("icon").GetComponent<Image>();
        num = transform.Find("num").GetComponent<Text>();
        mask = transform.Find("mask");

        mask.gameObject.SetActive(false);
	}
	

    public void setinfo(int id)
    {
        info = SigninPanel.instance.GetsignByid(id);
        dateLabel.text = info.date;
        icon.sprite = Resources.Load<Sprite>("Icon/" + info.icon);
        num.text = "x" + info.num;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
