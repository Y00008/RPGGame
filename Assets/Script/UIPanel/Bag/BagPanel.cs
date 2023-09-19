using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BagPanel : BasePanel
{
    private static BagPanel instance;

    public static BagPanel Instance
    {
        get { return BagPanel.instance; }
        set { BagPanel.instance = value; }
    }
    private Button dilatationBtn;
    private List<BagSlot> slotList = new List<BagSlot>();
    private List<BagItem> bagitemList = new List<BagItem>();//保存所有存入背包的物品

    public List<BagItem> BagitemList
    {
        get { return bagitemList; }
        set { bagitemList = value; }
    }
    private Button closeBtn;
    private CanvasGroup canvasGroup;
    private Text goldNumLabel;
    private Playerstatus player;
    //游戏开始背包初始格子数量
    private int SlotNum = 15;
    private Transform content;
    private Button allBtn;
    private Button equipBtn;
    private Button materialBtn;
    private Button drugBtn;
    public BagItem operateItem;//分解和售卖的物品
    public Transform function;
    private Button resolveBtn;
    private Button sellBtn;
    private Button cancelBtn;
    // Use this for initialization
    void Awake()
    {
        instance = this;
        dilatationBtn = transform.Find("dilatation").GetComponent<Button>();
        function = transform.Find("function");
        cancelBtn = function.Find("cancel").GetComponent<Button>();
        sellBtn = function.Find("Sell").GetComponent<Button>();
        resolveBtn = function.Find("Resolve").GetComponent<Button>();
        allBtn = transform.Find("button/all").GetComponent<Button>();
        equipBtn = transform.Find("button/equip").GetComponent<Button>();
        materialBtn = transform.Find("button/material").GetComponent<Button>();
        drugBtn = transform.Find("button/drug").GetComponent<Button>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        goldNumLabel = transform.Find("Gold/Text").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        content = transform.Find("Scroll View/Viewport/Content");

        function.gameObject.SetActive(false);

        closeBtn.onClick.AddListener(OnClickCloseBtn);
        allBtn.onClick.AddListener(OnClickAllBtn);
        equipBtn.onClick.AddListener(OnClickEquipBtn);
        materialBtn.onClick.AddListener(OnClickMaterialBtn);
        drugBtn.onClick.AddListener(OnClickDrugBtn);
        sellBtn.onClick.AddListener(OnClickSellBtn);
        resolveBtn.onClick.AddListener(OnClickResoveBtn);
        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        dilatationBtn.onClick.AddListener(OnClickDilatationBtn);

        //动态加载格子
        LoadBagSlot(SlotNum);
        //for (int i = 0; i < 20; i++)
        //{
        //    slotList.Add(transform.Find("Scroll View/Viewport/Content/Slot" + (i + 1)).GetComponent<BagSlot>());
        //}
    }

    private void OnClickDilatationBtn()
    {
        if(player.JudegeGold(50))
        {
            LoadBagSlot(5);
            UpadteGold();
        }
    }

    private void OnClickCancelBtn()
    {
        function.gameObject.SetActive(false);
    }
    //分解
    private void OnClickResoveBtn()
    {
        Objectinfo info = operateItem.Bagiteminfo;
        //不是武器，无法分解，直接退出
        if(info.objectType!=ObjectType.Equip)
        {
            return;
        }
        //删除物品链表中的物品
        bagitemList.Remove(operateItem);
        //清除格子id
        operateItem.transform.parent.GetComponent<BagSlot>().Clearid();
        //删除物品
        GameObject.DestroyImmediate(operateItem.gameObject);
        operateItem = null;
        //隐藏售卖面板
        function.gameObject.SetActive(false);
        //将分解的材料放入背包
        PutMaterial(info.syntheticmaterialOneid, info.syntheticmaterialOneNum);
        PutMaterial(info.syntheticmaterialTwoid, info.syntheticmaterialTwoNum);
        PutMaterial(info.syntheticmaterialThreeid, info.syntheticmaterialThreeNum);
        PutMaterial(info.syntheticmaterialFourid, info.syntheticmaterialFourNum);    
    }
    //将分解的材料放入背包
    void PutMaterial(int id,int count)
    {
        if (id != 0)
        {
            //分解得到的数量为合成的一半，最少为一
            int num = count / 2;
            if (num <= 1)
            {
                num = 1;
            }
            GetId(id, num);
        }
    }
    //售卖
    private void OnClickSellBtn()
    {
        //增加金币
        player.AddGold(operateItem.Bagiteminfo.sellprice*operateItem.Num);
        //删除物品链表中的物品
        bagitemList.Remove(operateItem);
        //清除格子id
        operateItem.transform.parent.GetComponent<BagSlot>().Clearid();
        //删除物品
        GameObject.DestroyImmediate(operateItem.gameObject);
        operateItem = null;
        //隐藏售卖面板
        function.gameObject.SetActive(false);
        UpadteGold();
    }
    //更新显示金币
    void UpadteGold()
    {
        goldNumLabel.text = player.gold.ToString();
    }
    //分类按钮
    private void OnClickDrugBtn()
    {
        Clear();
        BagItemClassify(ObjectType.Drug);
    }
    //分类按钮
    private void OnClickMaterialBtn()
    {
        Clear();
        BagItemClassify(ObjectType.Material);
    }
    //分类按钮
    private void OnClickEquipBtn()
    {
        Clear();
        BagItemClassify(ObjectType.Equip);
    }
    //分类按钮
    private void OnClickAllBtn()
    {
        Clear();
        //BagItemClassify(ObjectType.All);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetId(Random.Range(1001, 1004), 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            int id = Random.Range(2001, 2018);
            GetId(id, 1);
            Debug.Log(id);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            int id = Random.Range(3001, 3010);
            GetId(id, 1);
            Debug.Log(id);
        }
    }
    private void OnClickCloseBtn()
    {
        UIManager.Instance.PopPanel();
    }
    public override void OnEnter()
    {
        UpadteGold();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Vector3 temp = transform.localPosition;
        temp.x = 2000;
        transform.localPosition = temp;
        transform.DOLocalMoveX(0, 0.5f);
    }
    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        transform.DOLocalMoveX(2000, 0.5f).OnComplete(() => { canvasGroup.alpha = 0; });
        function.gameObject.SetActive(false);
    }
    public override void OnPause()
    {
        //canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        //canvasGroup.blocksRaycasts = true;
    }
    //将物品放入背包
    public void GetId(int id, int count = 1)
    {
        BagSlot slot = null;
        //遍历格子，是否有相同的物品
        foreach (BagSlot tempslot in slotList)
        {
            if (id == tempslot.Id)
            {
                slot = tempslot;
                break;
            }
        }
        //存在相同格子
        if (slot != null)
        {
            slot.Plus(count);
            //更新字典的数量
            //bagitemdic[id] += count;
        }
        else//不存在
        {
            //遍历，是否有空格子
            foreach (BagSlot tempslot in slotList)
            {
                if (tempslot.Id == 0)
                {
                    slot = tempslot;
                    break;
                }
            }
            //有空格子
            if (slot != null)
            {
                GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/Icon"));
                //将存入背包的物品存入链表
                bagitemList.Add(item.GetComponent<BagItem>());
                //bagitemdic.Add(id, count);
                //设置为格子的子节点
                item.transform.SetParent(slot.gameObject.transform);
                //将物品放在格子中央
                item.gameObject.transform.localPosition = Vector3.zero;
                slot.Setiteminfo(id, count);
            }
            else
            {
                //没有空格子
                Debug.Log("背包已满");
                LoadBagSlot(5);
                GetId(id, count);
            }
        }

    }

    //快捷栏使用药品
    public int UseDrug(int id, int count = 1)
    {
        BagSlot slot = null;
        //遍历格子，找到物品所在的格子
        foreach (BagSlot item in slotList)
        {
            if (item.Id == id)
            {
                slot = item;
                break;
            }
        }
        //找到了物品
        if (slot != null)
        {
            return slot.ReduceNum(count);
        }
        else
        {
            Debug.Log("背包中没有该物品");
            return -1;
        }
    }

    //动态加载格子
    void LoadBagSlot(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //生成物品
            GameObject slot = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/Slot"));
            //设置父节点
            slot.transform.SetParent(content);
            //添加到链表中
            slotList.Add(slot.GetComponent<BagSlot>());
            //设置索引
            slot.GetComponent<BagSlot>().Index = slotList.Count;
            //Debug.Log(slotList[i].Index);
        }
    }

    //清空格子物品
    void Clear()
    {
        for (int j = 0; j < slotList.Count; j++)
        {
            Debug.Log(slotList.Count);
            slotList[j].gameObject.SetActive(true);
        }
    }

    //背包物品分类
    void BagItemClassify(ObjectType type)
    {
        //如果和传进来的类型一样显示
        for (int i = 0; i < bagitemList.Count; i++)
        {
            if (bagitemList[i].Bagiteminfo.objectType != type)
            {
                bagitemList[i].transform.parent.gameObject.SetActive(false);
            }
        }
        for (int j = 0; j < slotList.Count; j++)
        {
            Debug.Log(slotList.Count);
            if (slotList[j].Id == 0)
            {
                slotList[j].gameObject.SetActive(false);
            }
        }
    }

    //获取指定id物品的数量
    public int GetNum(int id)
    {
        foreach (BagItem item in bagitemList)
        {
            if(item.Id==id)
            {
                //找到了返回数量
                return item.Num;
            }
        }
        //没有找到返回0
        return 0;
    }
}
