using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyntheticSlot : MonoBehaviour {

    private Text numLabel;
    private Image icon;
	// Use this for initialization
	void Awake () {
        numLabel = transform.Find("num").GetComponent<Text>();
        icon = transform.Find("icon").GetComponent<Image>();
	}
	//设置格子信息
    public bool setinfo(int id,int num)
    {
        if(id>0)
        {
            Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(id);
            icon.sprite = Resources.Load("Icon/" + info.iconame, typeof(Sprite)) as Sprite;
            //设置数量
            numLabel.text = BagPanel.Instance.GetNum(id)+"/" + num;

        }
        else
        {
            icon.gameObject.SetActive(false);
            numLabel.gameObject.SetActive(false);
        }
        if (BagPanel.Instance.GetNum(id) >= num)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
