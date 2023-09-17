using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopitem : MonoBehaviour {

      Text nameLabel;
      Text effectLabel;
      Text buyPriceLabel;
      Button buyBtn;
      Image ico;
      int id;
	// Use this for initialization
	void Awake() {
        nameLabel = transform.Find("Name").GetComponent<Text>();
        effectLabel = transform.Find("Effect").GetComponent<Text>();
        buyPriceLabel = transform.Find("Buyprice").GetComponent<Text>();
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        ico = transform.Find("itemico").GetComponent<Image>();

        buyBtn.onClick.AddListener(OnClickBuyBtn);
	}

      void OnClickBuyBtn()
    {
        ShopPanel.Instance.GetId(id);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //设置物品信息
    public void SetItemInfo(int id)
    {
        this.id = id;
        Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(id);
        nameLabel.text = info.name;
        ico.sprite = Resources.Load<Sprite>("Icon/" + info.iconame);
        if(info.hp>0)
        {
            effectLabel.text = "效果:回复血量" + info.hp;
        }
        else if(info.mp>0)
        {
            effectLabel.text = "效果:回复蓝量" + info.mp;
        }
        else if(info.attack>0)
        {
            effectLabel.text = "效果:+攻击" + info.attack;
        }
        else if(info.def>0)
        {
            effectLabel.text = "效果:+防御" + info.def;
        }
        else if(info.speed>0)
        {
            effectLabel.text = "效果:+速度" + info.speed;
        }
        buyPriceLabel.text ="售价:"+ info.sellprice.ToString();
    }
}
