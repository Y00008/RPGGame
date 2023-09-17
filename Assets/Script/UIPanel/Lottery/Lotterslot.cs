using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lotterslot : MonoBehaviour {
    private lottery info;
    private Image icon;
    private Text nunLabel;
    public Image mask;
    private int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

	// Use this for initialization
	void Awake () {
        icon = transform.Find("icon").GetComponent<Image>();
        nunLabel = transform.Find("num").GetComponent<Text>();
        mask = transform.Find("mask").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //设置信息
    public void  Setinfo(int id)
    {
        this.id = id;
        info = LotteryPanel.instance.getLotterybyid(id);
        icon.sprite = Resources.Load("Icon/" + info.rewardicon, typeof(Sprite)) as Sprite;
        nunLabel.text = info.rewardnum.ToString();
    }
}
