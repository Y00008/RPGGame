using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SystemPanel : BasePanel
{
    private AudioSource musicBg;
    private bool isMute=true;//是否静音,默认有声音
    private Image listenFill;
    private Button controopenBtn;
    private Button closeBtn;
    private CanvasGroup canvasGroup;
    // Use this for initialization
    void Awake()
    {
        musicBg = GameObject.FindGameObjectWithTag(Tags.Musicbg).GetComponent<AudioSource>();
        listenFill = transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
        controopenBtn = transform.Find("Button").GetComponent<Button>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();


        closeBtn.onClick.AddListener(OnClickCloseBtn);
        controopenBtn.onClick.AddListener(OnClickControopenBtn);
    }

    void Update()
    {
        musicBg.volume = listenFill.fillAmount;
    }
    private void OnClickControopenBtn()
    {
        if(isMute)
        {
            controopenBtn.GetComponent<Image>().sprite = Resources.Load("Icon/close", typeof(Sprite)) as Sprite;
            musicBg.mute = isMute;
        }
        else
        {
            controopenBtn.GetComponent<Image>().sprite = Resources.Load("Icon/open", typeof(Sprite)) as Sprite;
            musicBg.mute = isMute;
        }
        isMute = !isMute;
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
}
