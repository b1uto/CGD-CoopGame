using UnityEngine;
using UnityEngine.UI;


namespace CGD.MiniGames
{
    public class TestTube : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Image fillImg;

        [SerializeField] private RectTransform targetMarker;
        [SerializeField] private RectTransform limitMarker;

        public float limitFill;
        public float targetFill;
        public bool isTargetTube;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetTube(float limit, float target, float initialVol)
        {
            limitFill = limit;
            targetFill = target;

            fillImg.fillAmount = initialVol;

            var pos = Vector2.zero;
            var height = rectTransform.rect.height;

            pos.y = height * limitFill;
            limitMarker.anchoredPosition = pos;

            pos.y = height * targetFill;
            targetMarker.anchoredPosition = pos;

            targetMarker.gameObject.SetActive(isTargetTube);
        }




    }
}
