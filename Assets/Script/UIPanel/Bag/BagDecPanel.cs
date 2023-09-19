using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BagDecPanel : BasePanel {
      CanvasGroup canvasGroup;
      Text desLabel;
      static BagDecPanel instance;

    public static BagDecPanel Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        canvasGroup = this.GetComponent<CanvasGroup>();
        desLabel = transform.Find("desLabel").GetComponent<Text>();
    }

    public override void OnEnter()
    {
        transform.gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        transform.gameObject.SetActive(false);
    }
    public override void OnPause()
    {

    }
    public override void OnResume()
    {

    }

    public void Showinfo(int id)
    {
        transform.position = new Vector3(180,-120) + Input.mousePosition;
        Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(id);
        switch(info.objectType)
        {
            case ObjectType.Drug:
                desLabel.text = GetItemInfo(info);
                break;
            case ObjectType.Equip:
                desLabel.text = GetWeaponInfo(info);
                break;
            case ObjectType.Material:
                desLabel.text = GetMaterialInfo(info);
                break;
        }
       
    }

    string GetItemInfo(Objectinfo info)
    {
        string str="";
        str+="名字:"+info.name+"\n";
        str += "+Hp:" + info.hp + "\n";
        str += "+mp:" + info.mp + "\n";
        str += "售卖价:" + info.sellprice + "\n";
        str += "购买价:" + info.buyprice + "\n";
        return str;
    }
    //材料
    string GetMaterialInfo(Objectinfo info)
    {
        string str = "";
        str += "名字:" + info.name + "\n";
        str += "作用：合成武器\n";
        str += "售卖价:" + info.sellprice + "\n";
        str += "购买价:" + info.buyprice + "\n";
        return str;
    }

    string GetWeaponInfo(Objectinfo info)
    {
        string str = "";
        str += "名字:" + info.name + "\n";
        str += "+攻击:" + info.attack + "\n";
        str += "+防御:" + info.def + "\n";
        str += "+速度:" + info.speed + "\n";
        str += "适用部位:" + info.dresstype + "\n";
        str += "适用英雄:" + info.applytype + "\n";
        str += "售卖价:" + info.sellprice + "\n";
        str += "购买价:" + info.buyprice + "\n";
        return str;
    }
}
