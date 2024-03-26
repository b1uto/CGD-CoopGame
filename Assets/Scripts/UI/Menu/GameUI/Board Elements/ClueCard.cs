using UnityEngine.UI;
using UnityEngine;
using TMPro;
using CGD.Case;
using System;

public class ClueCard : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Image pointerImg;
    [SerializeField] private Image outlineImg; 
    [SerializeField] private TextMeshProUGUI descriptionTMP;

    [SerializeField] private Sprite[] clueTypeSprites;


    [SerializeField] private Button button;
    [SerializeField] private RectTransform rectTransform;

    public void DrawCard(string id, bool analysed, Action<RectTransform, string> callback)
    {
        if (ItemCollection.Instance.TryGetCaseData(id, out Clue caseData))
        {
            iconImg.sprite = caseData.icon;
            outlineImg.color = analysed ? new Color(1, 0.5f, 0) : Color.black;
            descriptionTMP.text = analysed ? caseData.analysedDescription : caseData.shortDescription;
            SetPointerImage(caseData.elementId);

            button.onClick.RemoveAllListeners();
            
            if (callback != null)
                button.onClick.AddListener(() => { callback?.Invoke(rectTransform, id); });
        }
    }

    /// <summary>
    /// sets pointer image based on element type.
    /// </summary>
    /// <param name="id"></param>
    private void SetPointerImage(string id) 
    {
        if (ItemCollection.Instance.TryGetCaseData(id, out CaseElement element))
        {
            int i = 0;

            if (element.GetType() == typeof(SuspectElement))
                i = 1;
 
            if (element.GetType() == typeof(MotiveElement))
                i = 2;

            pointerImg.sprite = clueTypeSprites[i];
        }
    }


}
    
