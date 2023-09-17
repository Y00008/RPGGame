using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SyntheticPanel : BasePanel {

    public static SyntheticPanel instance;

    private Button closeBtn;
    private CanvasGroup canvasGroup;

    private Button syntheticBtn;
    private Button selectBtn;
    private Image icon;

    private Transform material;

    public Dictionary<int, int> idAndnumdic = new Dictionary<int, int>();

    private Transform content;
    public Transform equipselectpanel;
    public string iconname="";
    public int equipid;
    private SyntheticSlot[] slot;
    private Transform tool;
    private Image SyntheticIcon;
    private Button confirmbtn;
    //  Button itemBtn;
    // Use this for initialization
    void Awake()
    {
        instance = this;
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        syntheticBtn = transform.Find("SyntheticBtn").GetComponent<Button>();
        selectBtn = transform.Find("SyntheticBg/SelectBtn").GetComponent<Button>();
        icon = selectBtn.transform.Find("icon").GetComponent<Image>();
        equipselectpanel = transform.Find("equipselectpanel");
        content = transform.Find("equipselectpanel/Scroll View/Viewport/Content");
        tool = transform.Find("ToolTip");
        SyntheticIcon = tool.Find("SyntheticIcon").GetComponent<Image>();
        confirmbtn = tool.Find("Button").GetComponent<Button>();

        material = transform.Find("SyntheticBg/material");
        tool.gameObject.SetActive(false);

        equipselectpanel.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        material.gameObject.SetActive(false);

        syntheticBtn.onClick.AddListener(OnClickSyntheticBtn);
        selectBtn.onClick.AddListener(OnclickSelectBtn);
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        confirmbtn.onClick.AddListener(OnClickConfirmBtn);
    }

    private void OnClickConfirmBtn()
    {
        icon.gameObject.SetActive(false);
        material.gameObject.SetActive(false);
        tool.gameObject.SetActive(false);
        update();
    }


    private void OnclickSelectBtn()
    {
        equipselectpanel.gameObject.SetActive(true);
        Read();

    }

    private void OnClickSyntheticBtn()
    {
        if(iconname=="")
        {
            return;
        }
        //材料足够
        if(SetInfo(iconname,idAndnumdic))
        {
            BagPanel.Instance.GetId(equipid);
            foreach (int id in idAndnumdic.Keys)
            {
                BagPanel.Instance.UseDrug(id, idAndnumdic[id]);
                SyntheticIcon.sprite = icon.sprite;
                tool.gameObject.SetActive(true);
            }
        }

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
        if(iconname!="")
        {
            SetInfo(iconname, idAndnumdic);
        }

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

    //更新id和数量
    public void update()
    {
        iconname = "";
        idAndnumdic.Clear();
        equipid = 0;
    }

    //选择锻造武器后设置材料格子信息
    public bool SetInfo(string path,Dictionary<int,int> dic)
    {
        bool issucces=true;
        int index = 0;
        icon.gameObject.SetActive(true);
        icon.sprite = Resources.Load<Sprite>("Icon/" + path);
        //获取到所有的格子
        slot = material.GetComponentsInChildren<SyntheticSlot>();
        material.gameObject.SetActive(true);
        foreach (int id in dic.Keys)
        {
            if(!slot[index].setinfo(id, dic[id]))
            {
                issucces = false;
            }
            index++;
        }
        return issucces;
    }
    void Read()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Shopproid");
        string[] ids = ta.text.Split(',');
        for (int i = 0; i < ids.Length; i++)
        {
            Objectinfo info = Objectinfolist.Instance.GetObjectifobyId(int.Parse(ids[i]));
            if(info.objectType==ObjectType.Equip)
            {
                GameObject Item = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/equip"), content);
                Item.GetComponent<Syntheticitem>().setinfo(int.Parse(ids[i]));
            }
        }
    }
}
