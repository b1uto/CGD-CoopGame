using CGD.Case;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CGD.Gameplay
{
    [RequireComponent(typeof(Button))]
    public class VoteCard : MonoBehaviour
    {
        public enum Type { Motive, Suspect, Weapon}
        public Type type;

        public Image img;
        public TextMeshProUGUI label;

        private CaseItem[] items;
        private int index = 0;

        private void Awake()
        {
            switch (type)
            {
                case Type.Motive:
                    items = ItemCollection.Instance.GetMotives();
                    break;
                case Type.Suspect:
                    items = ItemCollection.Instance.GetSuspects();
                    break;
                case Type.Weapon:
                    items = ItemCollection.Instance.GetWeapons();
                    break;
            }

            GetComponent<Button>().onClick.AddListener(DrawNext);
        }

        private void OnEnable()
        {
            DrawNext();
        }

        private void DrawNext() 
        {
            if(++index >= items.Length)
                index = 0;

            var item = items[index];
            
            if(item != null) 
            {
                img.sprite = item.icon;
                label.text = item.name;
            }
        }

        public CaseItem GetItem() => items[index];



    }
}
