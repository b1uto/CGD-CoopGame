using CGD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerCard : MonoBehaviour
{
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private Image icon;
    [SerializeField] private Image teamIndicator;
    [SerializeField] private TextMeshProUGUI nameTMP;

    private int actorNumber;
    public int ActorNumber { get { return actorNumber; } set {  actorNumber = value; } }


    public void Draw(string name, int iconId, int actorNumber, byte team)
    {
        ActorNumber = actorNumber;
        nameTMP.text = name;

        if(team != 0) 
        {
            teamIndicator.color = team == 0 ? Color.cyan : Color.red;
        }

        icon.sprite = ItemCollection.Instance.Avatars[iconId];
    }

    public void ToggleTurn(int actorNumber) => turnIndicator.SetActive(ActorNumber == actorNumber);



}
