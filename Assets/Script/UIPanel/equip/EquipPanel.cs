using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EquipPanel : BasePanel
{
      List<equipitem> equipList=new List<equipitem>();
      static EquipPanel instance;

    public static EquipPanel Instance
    {
        get { return EquipPanel.instance; }
        set { EquipPanel.instance = value; }
    }

      Playerstatus playstatus;
      Playerstatus player;
      Button closeBtn;
      CanvasGroup canvasGroup;
      Transform shoe;//鞋子
      Transform necklace;//项链
      Transform armour;//盔甲
      Transform jadeplate;//玉佩
      Transform weapon;//武器
      Transform casque;//头盔
    //  Button itemBtn;
    // Use this for initialization
    void Awake()
    {
        instance = this;

        playstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        shoe = transform.Find("shoe");
        necklace = transform.Find("necklace");
        armour = transform.Find("armour");
        jadeplate = transform.Find("jadeplate");
        weapon = transform.Find("weapon");
        casque = transform.Find("casque");

        player = GameObject.FindWithTag(Tags.player).GetComponent<Playerstatus>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);

    }



      void OnClickCloseBtn()
    {
        UIManager.Instance.PopPanel();
    }
    public override void OnEnter()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Vector3 temp = transform.localPosition;
        temp.x = 2000;
        transform.localPosition = temp;
        transform.DOLocalMoveX(0, 0.5f);
    }
    public override void OnExit()
    {
        //;
        canvasGroup.blocksRaycasts = false;
        transform.DOLocalMoveX(2000, 0.5f).OnComplete(() => { canvasGroup.alpha = 0; });
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    //穿戴装备
    public bool Dress(int id)
    {
        Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(id);
        //如果不是武器
        if (info.objectType!=ObjectType.Equip)
        {
            return false;
        }
        //如果装备是通用的，或者和英雄类型一样继续执行
        if(info.applytype==applyheroType.Comon||info.applytype.ToString()==player.herotype.ToString())
        {
            Transform parent = null;
            switch (info.dresstype)
            {
                case dressType.shoe:
                    parent = shoe;
                    break;
                case dressType.necklace:
                    parent = necklace;
                    break;
                case dressType.armour:
                    parent = armour;
                    break;
                case dressType.jadeplate:
                    parent = jadeplate;
                    break;
                case dressType.weapon:
                    parent = weapon;
                    break;
                case dressType.casque:
                    parent = casque;
                    break;
            }
            //获取节点下的装备
            equipitem item = parent.GetComponentInChildren<equipitem>();
            //有装备
            if(item!=null)
            {
                //装备与穿戴装备一致，不做处理
                if(item.id==id)
                {
                    Debug.Log("穿戴装备一致");
                    return false;
                }
                else
                {
                    //保存id，先穿在脱，解决脱装备时背包没有空余格子
                    int temp = item.id;
                    //改变装备的id和信息
                    item.SetInfo(id);
                    //重新实例化装备放到背包
                    BagPanel.Instance.GetId(temp);
                }
            }
            //没有装备，实例化一个装备放到装备下
            else
            {
                GameObject equipitem = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/weapon"), parent);
                equipitem.transform.localPosition = Vector3.zero;
                equipitem.GetComponent<equipitem>().SetInfo(id);
                equipList.Add(equipitem.GetComponent<equipitem>());
            }
            UpdateProperty();
            return true;
        }
        return false;
    }

    //更新属性
    void UpdateProperty()
    {
        playstatus.attackEquip = 0;
        playstatus.defEquip = 0;
        playstatus.speedEquip = 0;
        for (int i = 0; i < equipList.Count; i++)
        {
            Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(equipList[i].id);
            playstatus.attackEquip += info.attack;
            playstatus.defEquip += info.def;
            playstatus.speedEquip += info.speed;
        }
    }

    //卸下装备
    public void TakeOff(int id,GameObject equipitem)
    {
        equipList.Remove(equipitem.GetComponent<equipitem>());
        BagPanel.Instance.GetId(id);
        GameObject.Destroy(equipitem);
        UpdateProperty();
    }
}
