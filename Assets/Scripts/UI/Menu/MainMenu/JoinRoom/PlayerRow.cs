using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numTMP;
    [SerializeField] private TextMeshProUGUI nameTMP;

    public Player player;

    public void DrawRow(Player player, int i, byte code) 
    {
        this.player = player;

        string name = player.NickName;

        if (player.IsMasterClient)
            name += " [HOST]";

        nameTMP.text = name;
        numTMP.text = i.ToString();

        Color textColor = Color.white;

        if (code == 1)
            textColor = Color.blue;
        else if(code == 2) 
            textColor = Color.red;
        
        nameTMP.color = textColor;
        numTMP.color = textColor;
    }

}
