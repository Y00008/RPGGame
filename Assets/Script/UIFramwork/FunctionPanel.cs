using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FunctionPanel : BasePanel
{
    private  Button playerBtn;
    private Button BagBtn;
    //  Button TaskBtn;
    private Button EquipBtn;
    private Button SkillBtn;
    private Button SystemBtn;
    private Button TaskBagBtn;
    private CanvasGroup canvasGroup;
    private Button hideBtn;
    private bool isHide;
    private Button SyntheticBtn;
    private Button LotteryBtn;
    private Button Signinbtn;

    private Image redpoint;

	// Use this for initialization
	void Awake () {
        canvasGroup = this.GetComponent<CanvasGroup>();
        SyntheticBtn = transform.Find("syntheticBtn").GetComponent<Button>();
        hideBtn = transform.Find("hideBtn").GetComponent<Button>();
        playerBtn = transform.Find("PlayerBtn").GetComponent<Button>();
        BagBtn = transform.Find("BagBtn").GetComponent<Button>();
        //TaskBtn = transform.Find("TaskBtn").GetComponent<Button>();
        EquipBtn = transform.Find("EquipBtn").GetComponent<Button>();
        SkillBtn = transform.Find("SkillBtn").GetComponent<Button>();
        SystemBtn = transform.Find("SystemBtn").GetComponent<Button>();
        TaskBagBtn = transform.Find("TaskBagBtn").GetComponent<Button>();
        LotteryBtn = transform.Find("LotteryBtn").GetComponent<Button>();
        Signinbtn = transform.Find("SignBtn").GetComponent<Button>();

        redpoint = transform.Find("SignBtn/Image").GetComponent<Image>();


        hideBtn.onClick.AddListener(OnClickHideBtn);
        TaskBagBtn.onClick.AddListener(OnClickTaskBagBtn);
        playerBtn.onClick.AddListener(OnClickPlayerBtn);
        BagBtn.onClick.AddListener(OnClickBagBtn);
        //TaskBtn.onClick.AddListener(OnClickTaskBtn);
        EquipBtn.onClick.AddListener(OnClickEquipBtn);
        SkillBtn.onClick.AddListener(OnClickSkillBtn);
        SystemBtn.onClick.AddListener(OnClickSystemBtn);
        SyntheticBtn.onClick.AddListener(OnClickSyntheticBtn);
        LotteryBtn.onClick.AddListener(OnClickLotteryBtn);
        Signinbtn.onClick.AddListener(OnClickSiginBtn);

        Tween tw= transform.DOLocalMoveX(1465, 0.5f);
        tw.SetAutoKill(false);//禁止自动销毁动画
        tw.Pause();//停止播放动画

	}

    void Update()
    {
        if(SigninPanel.instance.issigined)
        {
            redpoint.gameObject.SetActive(true);
        }
        else
        {
            redpoint.gameObject.SetActive(false);
        }
    }

    private void OnClickSiginBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Signin);
    }

    private void OnClickLotteryBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Lottery);
    }

    private void OnClickSyntheticBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Synthetic);
    }

    private void OnClickHideBtn()
    {

        if(!isHide)
        {
            transform.DOPlayForward();
            isHide = true;
            //transform.rotation = transform.rotation+Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else
        {
            transform.DOPlayBackwards();
            //transform.DOLocalMoveX(worldpos.x, 0.5f);
            isHide = false;
        }
        hideBtn.transform.Rotate(new Vector3(0, 0, 180));
    }

      void OnClickTaskBagBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.TaskBag);
    }

      void OnClickSkillBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Skill);
    }

      void OnClickEquipBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Equip);
    }

      void OnClickTaskBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Task);
    }

      void OnClickSystemBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.System);
    }

    //  void OnClickShopBtn()
    //{
    //    UIManager.Instance.PushPanel(UIPanelType.Shop);
    //}

      void OnClickBagBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Bag);
    }

      void OnClickPlayerBtn()
    {
        UIManager.Instance.PushPanel(UIPanelType.Player);
    }


    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
	
	// Update is called once per frame
}
