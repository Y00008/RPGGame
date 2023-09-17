using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSlot : MonoBehaviour {
      int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public  int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //相同物品放入格子
    public void Plus(int count)
    {
        BagItem item = this.GetComponentInChildren<BagItem>();
        item.SetCount(count);
    }
    //设置物品信息
    public void  Setiteminfo(int id,int count)
    {
        this.id = id;
        Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(id);
        BagItem item = this.GetComponentInChildren<BagItem>();
        item.SetCount(id, count);
        item.SetIco(info.iconame);
    }
    //设置id
    public void Setid(int id)
    {
        this.id = id;
    }
    //清楚id
    public void Clearid()
    {
        this.Id = 0;
    }

    //快捷栏减少数量
    public int ReduceNum(int count=1)
    {
        //获取格子下的物品
        BagItem item = transform.GetComponentInChildren<BagItem>();
        return  item.UpdateItemNun(count);

    }
}
