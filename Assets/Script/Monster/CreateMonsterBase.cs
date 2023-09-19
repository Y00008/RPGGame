using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonsterBase : MonoBehaviour {
    protected MonsterType monsterType;
    protected GameObject EnenyPrefab;
    protected int maxMonsterNum;//最大怪物数量
    public int NowMonsterNum = 0;//当前怪物数量

    protected float Nowtime=0;
    protected float time;//生成怪物时间
	// Use this for initialization
	public virtual void Start () {
        switch (monsterType)
        {
            case MonsterType.Bear:
                EnenyPrefab = Resources.Load<GameObject>("Monster/Monster_Bear");
                break;
            case MonsterType.Lowrie:
                EnenyPrefab = Resources.Load<GameObject>("Monster/Monster_FoxElite #109672");
                break;
            case MonsterType.Nian:
                EnenyPrefab = Resources.Load<GameObject>("Monster/Monster_NianElite");
                break;
        }
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        //如果当前怪物小于最大数量怪物
		if(NowMonsterNum<maxMonsterNum)
        {
            //计时
            Nowtime += Time.deltaTime;
            //大于间隔时间，生成怪物
            if(Nowtime>=time)
            {
                //随机位置生成
                Vector3 pos = transform.position;
                pos.x += Random.Range(-3, 3);
                pos.x += Random.Range(-3, 3);
                GameObject monster = GameObject.Instantiate(EnenyPrefab, pos, Quaternion.identity);
                monster.GetComponent<MonsterBase>().createmonsterbase = this;
                monster.transform.SetParent(transform);
                //当前怪物数量增加
                NowMonsterNum++;
                Nowtime = 0;
            }

        }
	}
}
