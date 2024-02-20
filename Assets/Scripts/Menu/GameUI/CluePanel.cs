using BrunoMikoski.AnimationSequencer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluePanel : Menu
{
    public AnimationSequencerController animSeqController;

    private string newAlias;

    public override void OnMenuChanged(string newMenuAlias)
    {
        newAlias = newMenuAlias;

        //TODO update to include animations/transitions
        if (gameObject.activeInHierarchy && alias != newMenuAlias)
        {
            animSeqController.PlayBackwards();

        }
        else if (alias == newMenuAlias)
        {
            animSeqController.SetPlayType(AnimationSequencerController.PlayType.Forward);
            gameObject.SetActive(true);
            animSeqController.Play();
        }

    }

    public void OnSequenceFinished() => gameObject.SetActive(alias == newAlias);
}
