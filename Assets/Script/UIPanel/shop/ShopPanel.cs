using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShopPanel : BasePanel
{
    static ShopPanel instance;
    public static ShopPanel Instance
    {
        get { return ShopPanel.instance; }
        set { ShopPanel.instance = value; }
    }
    int id;
    private Transform buyContainer;
    private InputField input;//输入框

    private Transform content;
    private Button closeBtn;
    private CanvasGroup canvasGroup;
    private List<int> equipList = new List<int>();

    private Button confirm;
    private Button cancelBtn;
    private Button addbtn;
    private Button reduceBtn;
    private Text buyNumLabel;
    private Text itenName;
    private Image itemIcon;
    private int num = 1;
    private Objectinfo info;

    private Transform tip;
    private Text tipLabel;
    private Button okBtn;
    public CanvasGroup CanvasGroup
    {
        get { return canvasGroup; }
        set { canvasGroup = value; }
    }

    Playerstatus player;
    //  Button itemBtn;
    // Use this for initialization
    void Awake()
    {
        instance = this;


        player = GameObject.FindWithTag(Tags.player).GetComponent<Playerstatus>();

        input = transform.Find("Purchasebox/InputField").GetComponent<InputField>();

        buyContainer = transform.Find("Purchasebox");
        confirm = transform.Find("Purchasebox/confirm").GetComponent<Button>();
        cancelBtn = buyContainer.Find("cancel").GetComponent<Button>();
        addbtn = buyContainer.Find("Num/add").GetComponent<Button>();
        reduceBtn = buyContainer.Find("Num/reduce").GetComponent<Button>();
        buyNumLabel = buyContainer.Find("Num/Image/num").GetComponent<Text>();
        itenName = buyContainer.Find("itemname").GetComponent<Text>();
        itemIcon = buyContainer.Find("itemicon").GetComponent<Image>();
        buyContainer.gameObject.SetActive(false);

        tip = transform.Find("ToolTip");
        okBtn = tip.Find("Button").GetComponent<Button>();
        tipLabel = tip.Find("Text (2)").GetComponent<Text>();
        tip.gameObject.SetActive(false);

        content = transform.Find("shoppro/Viewport/Content");
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        confirm.onClick.AddListener(OnClickConfirmBtn);
        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        addbtn.onClick.AddListener(OnClickAddBtn);
        reduceBtn.onClick.AddListener(OnClickReduceBtn);
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        okBtn.onClick.AddListener(OnClickOkBtn);

        Read();
    }

    private void OnClickOkBtn()
    {
        tip.gameObject.SetActive(false);
    }

    private void OnClickReduceBtn()
    {
        num--;
        num = num <= 1 ? 1 : num; updateNum();
    }

    private void OnClickAddBtn()
    {
        num++;
        updateNum();
    }

    private void OnClickCancelBtn()
    {
        Clear();
    }

    //关闭购买面板，清空数据
    void Clear()
    {
        //影藏面板
        buyContainer.gameObject.SetActive(false);
        //清空id,数量
        id = 0;
        num = 1;
        info = null;
    }
    void OnClickConfirmBtn()
    {
        int sellprice = info.sellprice * num;
        if (player.Getcoin(sellprice))
        {
            buyContainer.gameObject.SetActive(false);
            BagPanel.Instance.GetId(id, num);
            tipLabel.text = "购买成功!";
            tip.gameObject.SetActive(true);
        }
        else
        {

            tip.gameObject.SetActive(true);
            tipLabel.text = "金币不足,请充值!";
        }
    }

    //保存购买物品的id，并且将输入框显示出来
    public void GetId(int id)
    {
        this.id = id;
        num = 1;
        updateNum();
        SetNameAndIcon();
        buyContainer.gameObject.SetActive(true);

    }

    //设置购买名字和图片
    void SetNameAndIcon()
    {
        info = Objectinfolist.Instance.GetObjectifobyId(id);
        itenName.text = info.name;
        itemIcon.sprite = Resources.Load("Icon/" + info.iconame, typeof(Sprite)) as Sprite;
    }
    //更新数量
    void updateNum()
    {
        buyNumLabel.text = num.ToString();
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
        canvasGroup.blocksRaycasts = false;
        transform.DOLocalMoveX(2000, 0.5f).OnComplete(() => { canvasGroup.alpha = 0; });
        Clear();
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    //根据解析出来的id实例化物品
    void Read()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Shopproid");
        string[] ids = ta.text.Split(',');
        for (int i = 0; i < ids.Length; i++)
        {
            GameObject shopItem = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/shopitem"), content);
            shopItem.GetComponent<shopitem>().SetItemInfo(int.Parse(ids[i]));
        }
    }
}
