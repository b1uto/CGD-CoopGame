using BrunoMikoski.AnimationSequencer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CluePanel : MenuPanel
{
    public AnimationSequencerController animSeqController;

    [Header("Panel References")]
    [SerializeField] private Image img;
    [SerializeField] private ScrollingText txt;

    public override void TogglePanel(bool showPanel)
    {
        //TODO update to include animations/transitions
        if (showPanel)
        {
            gameObject.SetActive(true);
            animSeqController.PlayForward();
        }
        else if (gameObject.activeInHierarchy)
        {
            animSeqController.PlayBackwards(true, () => gameObject.SetActive(false));
        }
    }

    private void SetPanel(Sprite icon, string text) 
    {
        img.sprite = icon;

        txt.SetMessage("YOU HAVE FOUND A CLUE\n\n" + text);
    }

    //private void OnSequenceFinished() => gameObject.SetActive(false);
}
