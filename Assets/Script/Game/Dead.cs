using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour {
    private Button ExitBtn;
    private Button AgainBtn;
	// Use this for initialization
	void Start () {
        ExitBtn = transform.Find("exit").GetComponent<Button>();
        AgainBtn = transform.Find("again").GetComponent<Button>();

        ExitBtn.onClick.AddListener(OnClickExitBtn);
        AgainBtn.onClick.AddListener(OnClickAgainBtn);
	}

    private void OnClickAgainBtn()
    {
        //UIManager.Instance.PanelSatck.Clear();
        //UIManager.Instance.PanelDict.Clear();
        //UIManager.Instance.PanelPathDict.Clear();
        SceneManager.LoadScene(3);
    }

    private void OnClickExitBtn()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update() {
		
	}
}
