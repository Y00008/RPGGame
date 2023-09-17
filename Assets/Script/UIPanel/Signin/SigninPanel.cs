using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class SigninPanel : BasePanel {
    private Button closeBtn;
    private CanvasGroup canvasGroup;

    public static SigninPanel instance;
    public bool issigined;//是否可以签到

    public const string SignNumPrefs = "SignNum";//领取次数的字符串
    public const string SignDataPrefs = "lastDay";//上次领取的时间字符串
    int signNum;//签到次数  默认是0

    DateTime today;//今日日期
    DateTime lastDay;//上次领取日期
    TimeSpan Interval;//间隔时间

    Button reviceButton;//领取按钮
    Text reviceText;//领取和时间Text

    bool isShowTime;//是否显示时间

    private int sign = 7;//签到为七天
    private Transform content;
    private Playerstatus playerstatus;


    private Dictionary<int, signinfo> signdic = new Dictionary<int, signinfo>();
    private List<Signinitem> siginitemlist = new List<Signinitem>();

    //开放接口
    public signinfo GetsignByid(int id)
    {
        signinfo sign = null;
        signdic.TryGetValue(id, out sign);
        return sign;
    }
	// Use this for initialization
    void Awake()
    {
        instance = this;
        playerstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        content = transform.Find("Signin");
        reviceButton = transform.Find("reviceButton").GetComponent<Button>();
        reviceButton.onClick.AddListener(OnSignClick);
        reviceText = transform.Find("reviceText").GetComponent<Text>();


        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);


    }

    private void OnSignClick()
    {
        isShowTime = true;
        reviceText.gameObject.SetActive(true);
        reviceButton.gameObject.SetActive(false);
        signinfo iteminfo=siginitemlist[signNum].info;
        //显示遮罩
        siginitemlist[signNum].mask.gameObject.SetActive(true);
        //根据奖励类型发放奖励
        if (iteminfo.type==SignRewardType.Gold)
        {
            playerstatus.AddGold(iteminfo.num);
        }
        if (iteminfo.type == SignRewardType.Prop)
        {
            BagPanel.Instance.GetId(iteminfo.Rewardid, iteminfo.num);
        }

        signNum++;//领取次数
        lastDay = today;
        PlayerPrefs.SetString(SignDataPrefs, today.ToString());
        PlayerPrefs.SetInt(SignNumPrefs, signNum);

    }

    void  OnEnable()
    {
        today = DateTime.Now;
        //在注册表里面获取上次签到的时间和次数
        signNum = PlayerPrefs.GetInt(SignNumPrefs, 0);
        lastDay = DateTime.Parse(PlayerPrefs.GetString(SignDataPrefs, DateTime.MinValue.ToString()));
        issigined = IsOneDay();
        if (IsOneDay())//今天日期是否大于领取日期  可以领取
        {
            Debug.Log("可以领取！");
            if (signNum >= sign)//重新计算签到
            {
                PlayerPrefs.DeleteKey(SignNumPrefs);
                //TODO：把奖励物品重置
            }
            //TODO：把按钮text变成领取
            reviceText.fontSize = 25;
            //reviceText.text = "领取";
            reviceText.gameObject.SetActive(false);
            reviceButton.gameObject.SetActive(true);
        }
        else //签到日期未到
        {
            isShowTime = true;
            reviceText.gameObject.SetActive(true);
            reviceButton.gameObject.SetActive(false);
        }
        Read();
        instate();
    }

    void instate()
    {
        for (int i = 0; i < sign; i++)
        {
            GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/Signinitem"),content);
            item.GetComponent<Signinitem>().setinfo((7000+i+1));
            if(i<signNum)
            {
                item.GetComponent<Signinitem>().mask.gameObject.SetActive(true);
            }
            siginitemlist.Add(item.GetComponent<Signinitem>());
        }
    }

    //判断是否可以签到
    private bool IsOneDay()
    {
        if (lastDay.Year == today.Year && lastDay.Month == today.Month && lastDay.Day == today.Day)
        {
            return false;
        }
        if (DateTime.Compare(lastDay, today) < 0)//DateTime.Compare(t1,t2) 返回<0  t10 t1>t2
        {
            return true;
        }
        return false;
    }
    void OnClickCloseBtn()
    {
        UIManager.Instance.PopPanel();
    }
	// Update is called once per frame
	void Update () {
        issigined = IsOneDay();
        if (isShowTime)
        {
            Interval = lastDay.AddDays(1).Date - DateTime.Now;
            reviceText.text ="距离下次签到剩余时间:" +string.Format("{0:D2}:{1:D2}:{2:D2}", Interval.Hours, Interval.Minutes, Interval.Seconds);
        }
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

    //解析签到配置表
    void Read()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Sigin");
        string[] str = ta.text.Split('\n');
        foreach (string item in str)
        {
            string[] it = item.Split(',');
            signinfo info = new signinfo();
            info.id = int.Parse(it[0]);
            info.date = it[1];
            info.type = (SignRewardType)System.Enum.Parse(typeof(SignRewardType), it[2]);
            info.icon = it[3];
            info.Rewardid = int.Parse(it[4]);
            info.num = int.Parse(it[5]);
            signdic.Add(info.id, info);
        }
    }
}

public enum SignRewardType
{
    Prop,
    Gold
}
public  class signinfo
{
    public int id;
    public string date;
    public SignRewardType type;
    public string icon;
    public int Rewardid;
    public int num;
}