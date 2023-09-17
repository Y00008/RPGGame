using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoading : MonoBehaviour {
    private Image progressFill;
    private Text progressLabel;
    private AsyncOperation operation;
    private float TargetValue;
	// Use this for initialization
	void Start () {
        progressFill = transform.Find("ProgressBar").GetComponent<Image>();
        progressLabel = transform.Find("Text").GetComponent<Text>();
        progressFill.fillAmount = 0;

        loadscene();
	}
	
	// Update is called once per frame
	void Update () {
		if(operation!=null)
        {
            TargetValue = operation.progress;
            if(TargetValue>=0.9f)
            {
                TargetValue = 1;
            }
            if(TargetValue!=progressFill.fillAmount)
            {
                progressFill.fillAmount = Mathf.Lerp(progressFill.fillAmount, TargetValue, Time.deltaTime);
                if(Mathf.Abs( progressFill.fillAmount-TargetValue)<0.01f)
                {
                    progressFill.fillAmount = TargetValue;
                }
            }
            progressLabel.text = (int)(progressFill.fillAmount*100) + "%";
            if(progressFill.fillAmount==1)
            {
                operation.allowSceneActivation = true;
            }
        }
	}

    public void  loadscene()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        operation = SceneManager.LoadSceneAsync(3);
        operation.allowSceneActivation = false;
        yield return operation;
    }
}
