using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.DemiLib;
using UnityEngine.SceneManagement;
public class BtnEffect : MonoBehaviour {

      Button newGameBtn;
      //Button LoadGameBtn;
      Transform pressAnyKey;
      Transform btnContainer;
      Slider progressBar;
    //  AsyncOperation characterScene;
    //  AsyncOperation GameScene;
	// Use this for initialization
	void Start () {
        
        progressBar = transform.Find("ProgressBar").GetComponent<Slider>(); ;
        pressAnyKey = transform.Find("PressAnyKeyBtn");
        btnContainer = transform.Find("BtnContainer");
        //LoadGameBtn = btnContainer.Find("LoadGameBtn").GetComponent<Button>();
        newGameBtn = btnContainer.Find("NewGameBtn").GetComponent<Button>();

        //LoadGameBtn.onClick.AddListener(OnClickLoadGameBtn);
        newGameBtn.onClick.AddListener(OnClickNewGameBtn);

        btnContainer.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        //StartCoroutine(LoadScene());
	}

      void OnClickLoadGameBtn()
    {    
        //GameScene.allowSceneActivation = true;
        //Debug.Log("!");
        SceneManager.LoadScene(2);
    }

      void OnClickNewGameBtn()
    {
        //characterScene.allowSceneActivation = true;
        SceneManager.LoadScene(1);
    }


	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
        {
            show();
        }
	}
    void show()
    {
        pressAnyKey.gameObject.SetActive(false);
        btnContainer.gameObject.SetActive(true);
    }
    //IEnumerator LoadScene()
    //{
    //    characterScene=SceneManager.LoadSceneAsync(1);
    //    GameScene=SceneManager.LoadSceneAsync(2);
    //    characterScene.allowSceneActivation = false;
    //    GameScene.allowSceneActivation = false;
    //    yield return new WaitForSeconds(0.1f);
    //}
}

