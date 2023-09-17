using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPanel : BasePanel
{
      CanvasGroup canvasGroup;
    // Use this for initialization
    void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    public override void OnPause()
    {
        //canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        //canvasGroup.blocksRaycasts = true;
    }
}
