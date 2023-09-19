using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lotteryitem : MonoBehaviour {

    private Image oneicon;
    private Text onenumLabel;
	// Use this for initialization
	void Awake () {
        oneicon = transform.Find("Icon").GetComponent<Image>();
        onenumLabel = transform.Find("Num").GetComponent<Text>();
	}
    public void setinfo(int id)
    {
        lottery info=LotteryPanel.instance.getLotterybyid(id);
        oneicon.sprite = Resources.Load("Icon/" + info.rewardicon, typeof(Sprite)) as Sprite;
        onenumLabel.text = "x" + info.rewardnum;
    }
}
