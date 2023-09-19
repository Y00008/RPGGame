using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillItem : MonoBehaviour {
      Image ico;
      Text nameLabel;
      Text desLabel;
      Text mpLabel;//消耗蓝量
      Text skillTypeLabel;//类别群体，单体技能
      Transform mask;
      SkillInfo info;
      int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }


	// Use this for initialization
	void Awake () {
        nameLabel = transform.Find("Name").GetComponent<Text>();
        desLabel = transform.Find("Des").GetComponent<Text>();
        mpLabel = transform.Find("Mp").GetComponent<Text>();
        skillTypeLabel = transform.Find("ApplyType").GetComponent<Text>();
        ico = transform.Find("SkillIco").GetComponent<Image>();
        mask = transform.Find("Mask");
	}
	
    //设置信息
    public void SetSkillInfo(int id)
    {
        this.id = id;
        info = SkillInfoList.Instance.GetskillByid(id);
        ico.sprite = Resources.Load<Sprite>("SkillIco/" + info.iconame);
        nameLabel.text = "名字:" + info.name;
        desLabel.text = info.des;
        mpLabel.text = info.mp + "MP";
        switch (info.skilltype)
        {
            case skillType.SingleTarget:
                skillTypeLabel.text = "单体";
                break;
            case skillType.Buff:
                skillTypeLabel.text = "增强";
                break;
            case skillType.MultiTarget:
                skillTypeLabel.text = "群体";
                break;
            case skillType.Passive:
                skillTypeLabel.text = "增益";
                break;
        }
    }
    //显示是否是可用状态
    public void Updateshow(int level)
    {
        if(info.level<=level)
        {
            mask.gameObject.SetActive(false);
        }
        else
        {
            mask.gameObject.SetActive(true);
        }
    }
}
