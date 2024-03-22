using BrunoMikoski.AnimationSequencer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluePanel : MenuPanel
{
    public AnimationSequencerController animSeqController;

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

    //private void OnSequenceFinished() => gameObject.SetActive(false);
}
