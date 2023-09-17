using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterCreate : MonoBehaviour
{

    private AudioSource clickListen;//点击音效
    private Button nextBtn;
    private Button preBtn;
    private Button confirmBtn;
    private InputField name;
    private int selectIndex;
    private string[] characterPerfabPath;
    private GameObject[] player;
    private Transform characterParent;
    //private Transform progress;
    // Use this for initialization
    void Start()
    {
        //progress = transform.Find("ProgressBarBg");
        characterParent = GameObject.Find("Character").transform;
        nextBtn = transform.Find("NextBtn").GetComponent<Button>();
        preBtn = transform.Find("PreviousBtn").GetComponent<Button>();
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        name = transform.Find("Name").GetComponent<InputField>();

        clickListen = GameObject.FindGameObjectWithTag(Tags.SelectMusicClickListen).GetComponent<AudioSource>();

        nextBtn.gameObject.SetActive(false);
        preBtn.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);
        name.gameObject.SetActive(false);
        //progress.gameObject.SetActive(false);

        nextBtn.onClick.AddListener(OnClickNextBtn);
        preBtn.onClick.AddListener(OnClickPreBtn);
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);

        TextAsset ta = Resources.Load<TextAsset>("TextInfo/charaperfabpath");
        characterPerfabPath = ta.text.Split(',');
        player = new GameObject[characterPerfabPath.Length];
        for (int i = 0; i < characterPerfabPath.Length; i++)
        {
            player[i] = GameObject.Instantiate(Resources.Load<GameObject>(characterPerfabPath[i]), characterParent, true);
        }
        UpdateCharacterShow();
    }

    void Update()
    {
        if (MoveCamera.Instance.IsBtnShow)
        {
            nextBtn.gameObject.SetActive(true);
            preBtn.gameObject.SetActive(true);
            confirmBtn.gameObject.SetActive(true);
            name.gameObject.SetActive(true);
        }
    }
    void OnClickConfirmBtn()
    {
        if (!name.text.Equals(""))
        {
            PlayerPrefs.SetString("Character", characterPerfabPath[selectIndex]);
            PlayerPrefs.SetString("name", name.text);
            //progress.gameObject.SetActive(true);
            //progress.GetComponent<AsyncLoading>().loadscene();
            SceneManager.LoadScene(2);
        }
    }

    void OnClickPreBtn()
    {
        clickListen.Play();
        selectIndex--;
        if (selectIndex < 0)
        {
            selectIndex = characterPerfabPath.Length - 1;
        }
        UpdateCharacterShow();
    }

    void OnClickNextBtn()
    {
        clickListen.Play();
        selectIndex++;
        selectIndex %= characterPerfabPath.Length;
        UpdateCharacterShow();
    }
    void UpdateCharacterShow()
    {
        player[selectIndex].SetActive(true);
        for (int i = 0; i < characterPerfabPath.Length; i++)
        {
            if (i != selectIndex)
            {
                player[i].SetActive(false);
            }
        }
    }
}
