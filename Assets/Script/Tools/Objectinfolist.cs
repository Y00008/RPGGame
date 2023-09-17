using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectinfolist : MonoBehaviour {

      Dictionary<int, Objectinfo> objectinfoDic = new Dictionary<int, Objectinfo>();
      static Objectinfolist instance;
    public static Objectinfolist Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    public Objectinfo GetObjectifobyId(int id)
    {
        Objectinfo info = null;
        objectinfoDic.TryGetValue(id, out info);
        return info; 
    }
	// Use this for initialization
	void Start () {
        instance = this;
        ReadInfo();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void  ReadInfo()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/ObjectinfoList");
        string[] info = ta.text.Split('\n');
        foreach(string it in info)
        {
            string[] proinfoarray = it.Split(',');
            Objectinfo objectinfo = new Objectinfo();
            objectinfo.id = int.Parse(proinfoarray[0]);
            objectinfo.name = proinfoarray[1];
            objectinfo.iconame = proinfoarray[2];
            objectinfo.objectType = (ObjectType)System.Enum.Parse(typeof(ObjectType), proinfoarray[3]);
            if (objectinfo.objectType == ObjectType.Drug || objectinfo.objectType == ObjectType.Material)
            {
                objectinfo.hp = int.Parse(proinfoarray[4]);
                objectinfo.mp = int.Parse(proinfoarray[5]);
                objectinfo.buyprice = int.Parse(proinfoarray[6]);
                objectinfo.sellprice = int.Parse(proinfoarray[7]);
            }
            else if(objectinfo.objectType == ObjectType.Equip)
            {
                objectinfo.attack = int.Parse(proinfoarray[4]);
                objectinfo.def = int.Parse(proinfoarray[5]);
                objectinfo.speed = int.Parse(proinfoarray[6]);
                objectinfo.dresstype = (dressType)System.Enum.Parse(typeof(dressType), proinfoarray[7]);
                objectinfo.applytype = (applyheroType)System.Enum.Parse(typeof(applyheroType), proinfoarray[8]);
                objectinfo.buyprice = int.Parse(proinfoarray[9]);
                objectinfo.sellprice = int.Parse(proinfoarray[10]);

                objectinfo.syntheticmaterialOneid = int.Parse(proinfoarray[11]);
                objectinfo.syntheticmaterialOneName = proinfoarray[12];
                objectinfo.syntheticmaterialOneNum = int.Parse(proinfoarray[13]);
                objectinfo.syntheticmaterialTwoid = int.Parse(proinfoarray[14]);
                objectinfo.syntheticmaterialTwoName = proinfoarray[15];
                objectinfo.syntheticmaterialTwoNum = int.Parse(proinfoarray[16]);

                objectinfo.syntheticmaterialThreeid = int.Parse(proinfoarray[17]);
                objectinfo.syntheticmaterialThreeName = proinfoarray[18];
                objectinfo.syntheticmaterialThreeNum = int.Parse(proinfoarray[19]);
                objectinfo.syntheticmaterialFourid = int.Parse(proinfoarray[20]);
                objectinfo.syntheticmaterialFourName = proinfoarray[21];
                objectinfo.syntheticmaterialFourNum = int.Parse(proinfoarray[21]);
            }

            objectinfoDic.Add(objectinfo.id, objectinfo);
        }
    }


}
public enum ObjectType
{
    Drug,
    Material,
    Equip,
    All
}
//适用类型
public enum dressType
{
    shoe,
    necklace,
    armour,
    jadeplate,
    weapon,
    casque
}
//适用职业
public enum applyheroType
{
    Swordman,
    Magician,
    Comon
}
public class Objectinfo
{
    public int id;
    public string name;
    public string iconame;
    public ObjectType objectType;
    public int hp;
    public int mp;

    public int buyprice;
    public int sellprice;

    public int attack;
    public int def;
    public int speed;
    public dressType dresstype;
    public applyheroType applytype;
    public int syntheticmaterialOneid;
    public string syntheticmaterialOneName;
    public int syntheticmaterialOneNum;

    public int syntheticmaterialTwoid;
    public string syntheticmaterialTwoName;
    public int syntheticmaterialTwoNum;

    public int syntheticmaterialThreeid;
    public string syntheticmaterialThreeName;
    public int syntheticmaterialThreeNum;
    public int syntheticmaterialFourid;
    public string syntheticmaterialFourName;
    public int syntheticmaterialFourNum;

    
}
