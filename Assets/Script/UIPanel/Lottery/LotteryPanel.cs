using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum lotteryType
{
    One,
    Ten,
    None
}
public class LotteryPanel : BasePanel
{
    private Button oneBtn;
    private Button tenBtn;
    public static LotteryPanel instance;
    private Button closeBtn;
    private CanvasGroup canvasGroup;
    private Dictionary<int, lottery> lotterydic = new Dictionary<int, lottery>();
    private Transform slotparent;
    private int slotnum = 12;
    private List<Lotterslot> slotList = new List<Lotterslot>();
    private int turns = 4;//抽奖转的圈数
    private float speed = 0.5f;//起始速度
    private float turnsSpeed = 0.032f;//转动速度
    private Transform ontTip;//抽奖一次的物品提示框
    private Image oneicon;
    private Text onenumLabel;
    private Button oneConfirmBtn;
    private int ten = 10;
    private lotteryType lotterytype = lotteryType.None;
    private List<int> idList = new List<int>();//十连抽的id

    private Transform tenToolTip;
    private Button tenconfirmBtn;
    private Transform tencontent;
    private Button skipBtn;
    private List<lotteryitem> tenitemlist = new List<lotteryitem>();//十次抽奖的物品

    private bool isLottery = false;//是否处在抽奖中

    private Playerstatus playerstatus;

    private int oneNeedGold = 50;//一次抽奖五十金币
    private int tenNeedGold = 450;//十抽奖四百五金币

    private bool isend = false;//协程是否结束0
    // Use this for initialization
    void Awake()
    {
        instance = this;
        skipBtn = transform.Find("skipBtn").GetComponent<Button>();
        playerstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        tenToolTip = transform.Find("tenToolTip");
        tenconfirmBtn = tenToolTip.Find("Button").GetComponent<Button>();
        tencontent = tenToolTip.Find("ten");

        ontTip = transform.Find("ToolTip");
        oneicon = ontTip.Find("Icon").GetComponent<Image>();
        onenumLabel = ontTip.Find("Num").GetComponent<Text>();
        oneConfirmBtn = ontTip.Find("Button").GetComponent<Button>();

        load();

        ontTip.gameObject.SetActive(false);
        tenToolTip.gameObject.SetActive(false);
        skipBtn.gameObject.SetActive(false);

        oneBtn = transform.Find("one").GetComponent<Button>();
        tenBtn = transform.Find("ten").GetComponent<Button>();
        slotparent = transform.Find("GameObject");
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);

        Read();
        Debug.Log(lotterydic.Count);

        for (int i = 0; i < slotnum; i++)
        {
            //设置信息
            slotparent.Find("slot" + (i + 1)).GetComponent<Lotterslot>().Setinfo((6001 + i));
            slotList.Add(slotparent.Find("slot" + (i + 1)).GetComponent<Lotterslot>());
        }

        oneBtn.onClick.AddListener(OnClickOneBtn);
        tenBtn.onClick.AddListener(OnClickTenBtn);
        oneConfirmBtn.onClick.AddListener(OnClickOneConfiemBtn);
        tenconfirmBtn.onClick.AddListener(OnClickTenConfiemBtn);

        skipBtn.onClick.AddListener(OnClickSkipBtn);
    }

    private void OnClickSkipBtn()
    {
        Time.timeScale = 100;
        //StopCoroutine(rotate(0));
        //isend = true;
    }

    private void OnClickTenConfiemBtn()
    {
        tenToolTip.gameObject.SetActive(false);
        lotterytype = lotteryType.None;
        //删除本次抽到的物品
        for (int i = 0; i < tencontent.childCount; i++)
        {
            Destroy(tencontent.GetChild(i).gameObject);
        }
        showMask();
    }

    private void OnClickOneConfiemBtn()
    {
        ontTip.gameObject.SetActive(false);
        lotterytype = lotteryType.None;
        showMask();
    }
    //十次抽奖
    private void OnClickTenBtn()
    {
        skipBtn.gameObject.SetActive(true);
        //如果不在抽奖中，点击抽奖判断金币是否足够
        if (!isLottery&&!playerstatus.JudegeGold(tenNeedGold))
        {
            return;
        }
        if(isLottery)
        {
            return;
        }
        isLottery = true;
        for (int i = 0; i < ten; i++)
        {
            int id = Random.Range(6001, 6013);
            idList.Add(id);
        }
        lotterytype = lotteryType.Ten;
        StartCoroutine(rotate(0));
    }
    //点击抽奖一次
    private void OnClickOneBtn()
    {
        skipBtn.gameObject.SetActive(true);
        if (!isLottery && !playerstatus.JudegeGold(oneNeedGold))
        {
            return;
        }
        if (isLottery)
        {
            return;
        }
        //skipBtn.gameObject.SetActive(true);
        isLottery = true;
        lotterytype = lotteryType.One;
        //随机抽奖id
        int id = Random.Range(6001, 6013);
        //开启携程
        StartCoroutine(rotate(id));
        Setinfo(id);
        BagPanel.Instance.GetId(lotterydic[id].rewardid, lotterydic[id].rewardnum);
        //if(isend)
        //{
        //    ontTip.gameObject.SetActive(true);
        //    Setinfo(id);
        //    BagPanel.Instance.GetId(lotterydic[id].rewardid, lotterydic[id].rewardnum);
        //    isLottery = false;
        //}

    }
    //设置获取的道具的信息
    void Setinfo(int id)
    {
        oneicon.sprite = Resources.Load("Icon/" + lotterydic[id].rewardicon, typeof(Sprite)) as Sprite;
        onenumLabel.text = "x" + lotterydic[id].rewardnum;
    }
    //转圈协程
    IEnumerator rotate(int id)
    {
        isend = false;
        int index = 0;
        int lotterynum = 0;
        //如果点击的是抽奖一次，抽奖次数为一,十次为十
        lotterynum = (lotterytype == lotteryType.One) ? 1 : 10;
        while (lotterynum > 0)
        {
            index = 0;
            speed = 0.5f;
            while (index++ <= turns)
            {
                for (int i = 0; i < slotList.Count; i++)
                {
                    if (index >= 3)
                    {
                        speed = speed + turnsSpeed * 1.4f;
                    }
                    else
                    {
                        speed = speed - turnsSpeed;
                    }
                    slotList[i].mask.gameObject.SetActive(false);
                    yield return new WaitForSeconds(speed);
                    //抽奖一次,最后一圈,转到随机出来的id暂停
                    if (lotterytype == lotteryType.One)
                    {
                        if (index == turns && id == slotList[i].Id)
                        {
                            isend = true;
                            index++;
                            lotterynum--;
                            break;
                        }
                    }
                    else
                    {
                        //如果是十抽，结束当前抽奖，继续下一次抽奖
                        if (index == turns && idList[10 - lotterynum] == slotList[i].Id)
                        {
                            index++;
                            yield return new WaitForSeconds(0.5f);
                            //遮罩显示
                            showMask();
                            lotterynum--;
                            break;

                        }
                    }
                    slotList[i].mask.gameObject.SetActive(true);
                }
            }

            if (lotterytype == lotteryType.One)
            {
                Time.timeScale = 1;
                skipBtn.gameObject.SetActive(false);
                isLottery = false;
                ontTip.gameObject.SetActive(true);

            }
        }
        if (lotterytype == lotteryType.Ten)
        {
            Time.timeScale = 1;
            skipBtn.gameObject.SetActive(false);
            isLottery = false;
            tenToolTip.gameObject.SetActive(true);
            loadinfo(idList);
        }
    }

    //将所有遮罩全部显示
    void showMask()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].mask.gameObject.SetActive(true);
        }
    }
    //通过id读取信息
    public lottery getLotterybyid(int id)
    {
        lottery info = null;
        lotterydic.TryGetValue(id, out info);
        return info;
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
        showMask();
    }
    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        transform.DOLocalMoveX(2000, 0.5f).OnComplete(() => { canvasGroup.alpha = 0; });
        tenToolTip.gameObject.SetActive(false);
        ontTip.gameObject.SetActive(false);
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    //动态加载式个奖励物品、
    void load()
    {
        for (int i = 0; i < ten; i++)
        {
            //加载物品
            GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/LotteryItem"), tencontent);
            tenitemlist.Add(item.GetComponent<lotteryitem>());
        }
    }

    //设置信息
    void loadinfo(List<int> id)
    {
        for (int i = 0; i < tenitemlist.Count; i++)
        {
            //通过传过来的id设置每个物品的信息
            tenitemlist[i].GetComponent<lotteryitem>().setinfo(id[i]);
            BagPanel.Instance.GetId(lotterydic[id[i]].rewardid, lotterydic[id[i]].rewardnum);
        }
    }
    void Read()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Lottery");
        string[] str = ta.text.Split('\n');
        foreach (string item in str)
        {
            string[] lottery = item.Split(',');
            lottery info = new lottery();
            info.id = int.Parse(lottery[0]);
            info.rewardicon = lottery[1];
            info.rewardnum = int.Parse(lottery[2]);
            info.rewardid = int.Parse(lottery[3]);
            lotterydic.Add(info.id, info);
        }
    }
}
public class lottery
{
    public int id;
    public string rewardicon;
    public int rewardnum;
    public int rewardid;
}
