using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeadPanel : MonoBehaviour {
      static HeadPanel instance;

    public static HeadPanel Instance
    {
        get { return HeadPanel.instance; }
        set { HeadPanel.instance = value; }
    }
      Playerstatus player;
      Image hpFill;
      Text hpLabel;
      Image mpFill;
      Text lvLabel;
      Text expLabel;
      Image expFill;
      Text mpLabel;
      Text nameLabel;
	// Use this for initialization
	void Awake() {
        instance = this;
        nameLabel = transform.Find("NameBg/Name").GetComponent<Text>();
        hpLabel = transform.Find("HpBg/Text").GetComponent<Text>();
        mpLabel = transform.Find("MpBg/Text").GetComponent<Text>();
        hpFill = transform.Find("HpBg/HpFill").GetComponent<Image>();
        mpFill = transform.Find("MpBg/MpFill").GetComponent<Image>();
        expFill = transform.Find("ExpBg/ExpFill").GetComponent<Image>();
        lvLabel = transform.Find("ExpBg/Lv").GetComponent<Text>();
        expLabel = transform.Find("ExpBg/Text").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        Updateshowinfo();
        SetExp(0);
	}
	
	// Update is called once per frame
	void Update () {

	}
    //更新信息
    public void Updateshowinfo()
    {
        nameLabel.text = player.heroName;
        hpFill.fillAmount = player.remainHp / player.maxHp;
        mpFill.fillAmount = player.remainMp / player.maxMp;
        hpLabel.text = player.remainHp / player.maxHp * 100 + "%";
        mpLabel.text = player.remainMp / player.maxMp * 100 + "%";
        //lvLabel.text = "LV." + player.level;
        ////当前经验和升级所需的百分比,升级公式 (100 + player.level * 30)
        //float exp = player.exp / (100 + player.level * 30);
        //expFill.fillAmount = exp;
        //expLabel.text = exp.ToString("F0") + "%";
    }
    //设置经验条
    public void SetExp(float exp)
    {
        lvLabel.text = "LV." + player.level;
        expFill.fillAmount = exp;
        expLabel.text = (exp*100).ToString("F0") + "%";
    }
}
