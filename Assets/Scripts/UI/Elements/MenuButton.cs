using CGD.Audio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour
{
    public bool forward;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            AudioController.Instance.PlayUI(forward ? UI.Forward : UI.Back);
        });
    }

}
