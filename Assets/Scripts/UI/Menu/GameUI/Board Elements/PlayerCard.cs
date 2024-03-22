using CGD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerCard : MonoBehaviour
{
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTMP;

    private int actorNumber;
    public int ActorNumber { get { return actorNumber; } set {  actorNumber = value; } }


    public void Draw(string name, int iconId, int actorNumber)
    {
        ActorNumber = actorNumber;
        nameTMP.text = name;
        //TODO icon.sprite = SpriteManager.GetSprite(iconId);
    }

    public void ToggleTurn(int actorNumber) => turnIndicator.SetActive(ActorNumber == actorNumber);



}
