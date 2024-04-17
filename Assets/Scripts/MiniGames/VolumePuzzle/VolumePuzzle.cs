
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace CGD.MiniGames
{

    public class VolumePuzzle : MiniGame
    {
        [SerializeField] private TestTube[] tubes;
        [SerializeField] private float[] tubePositions = new float[] { -450f, 0, 450f };

        [Header("Buttons")]
        [SerializeField] private Button[] btns;


        protected override void CheckPuzzleSolved() 
        {
            var targetTube = tubes.Single(x => x.isTargetTube == true);
            
            if (targetTube.targetFill == targetTube.fillImg.fillAmount)
                MiniGameFinished(true);
        }

        #region PuzzleGeneration
        protected override void InitialiseMiniGame()
        {
            return;
            //TODO
            var limits = new int[] { Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10) };

            int gcd = GCD(limits[0], GCD(limits[1], limits[2]));

            int maxMult = limits[0] / gcd;
            int target = Random.Range(1, maxMult+1) * gcd;

            int totalVol = (int)(1.5 * target);
            totalVol = (totalVol / gcd + (totalVol % gcd > 0 ? 1 : 0)) * gcd;



            for (int i = 0; i < tubes.Length; i++) 
            {
                int startVol = Random.Range(1, Mathf.Min(totalVol, limits[i]));
                totalVol -= startVol;

                tubes[i].SetTube(limits[i] / 10f, target/10f, startVol/10f);
            }
        }

        private int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        #endregion

        #region Buttons
        public void Btn_Fill() 
        {
            var fill0 = tubes[0].fillImg.fillAmount;
            var fill1 = tubes[1].fillImg.fillAmount;

            var amountToFill = (fill0 + fill1 >= tubes[0].limitFill) ?
                tubes[0].limitFill - fill0 :
                fill1;

            foreach (var btn in btns) { btn.interactable = false; }

            tubes[0].fillImg.DOFillAmount(fill0 + amountToFill, 0.5f);
            tubes[1].fillImg.DOFillAmount(fill1 - amountToFill, 0.5f).OnComplete(() =>
            {
                foreach (var btn in btns) { btn.interactable = true; }
                CheckPuzzleSolved();
            });
        }
        public void Btn_SwitchLeft()
        {
            foreach(var btn in btns ) { btn.interactable = false; }

            tubes[0].rectTransform.DOLocalMoveX(tubePositions[1], 0.5f);
            tubes[1].rectTransform.DOLocalMoveX(tubePositions[0], 0.5f).OnComplete(() =>
            {
                var origTube0 = tubes[0];
                tubes[0] = tubes[1];
                tubes[1] = origTube0;
                foreach (var btn in btns) { btn.interactable = true; }
            });
        }
        public void Btn_SwitchRight()
        {
            foreach (var btn in btns) { btn.interactable = false; }

            tubes[1].rectTransform.DOLocalMoveX(tubePositions[2], 0.5f);
            tubes[2].rectTransform.DOLocalMoveX(tubePositions[1], 0.5f).OnComplete(() =>
            {
                var origTube1 = tubes[1];
                tubes[1] = tubes[2];
                tubes[2] = origTube1;
                foreach (var btn in btns) { btn.interactable = true; }
            });
        }
        #endregion
    }
}