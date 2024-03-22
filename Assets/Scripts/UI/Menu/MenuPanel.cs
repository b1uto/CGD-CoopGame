using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuPanel : MonoBehaviour 
{
    public AnimationCurve animCurve;
    public Image animObj;

    public virtual void TogglePanel(bool showPanel) 
    {
        //TODO update to include animations/transitions
        
        gameObject.SetActive(showPanel);

        if (showPanel && animObj != null)
            AnimIn();
    }


    private void AnimIn() 
    {
        animObj.color = new Color(1,1,1,0);
        animObj.DOFade(1, 1.0f).SetEase(animCurve);
    }


}
