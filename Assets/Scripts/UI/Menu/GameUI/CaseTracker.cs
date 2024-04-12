using CGD.Case;
using System.Linq;
using UnityEngine;

namespace CGD.Gameplay
{
    public class CaseTracker : MonoBehaviour
    {
        [SerializeField] private GameObject clueCardPrefab;

        [SerializeField] private Transform suspectContainer;
        [SerializeField] private Transform motiveContainer;
        [SerializeField] private Transform weaponContainer;

        private int clueCount = 0;

        private void OnEnable()
        {
            var clues = PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().Clues;

            if (clueCount != clues.Count)
            {
                clueCount = clues.Count;
                DrawPanel(clues.Keys.ToArray());
            }

        }

        private void DrawPanel(string[] clueIds)
        {
            foreach (Transform child in suspectContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in motiveContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in weaponContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var id in clueIds)
            {
                if (ItemCollection.Instance.TryGetCaseData<Clue>(id, out var clue))
                {
                    if (string.IsNullOrEmpty(clue.name))
                        continue;

                    Transform parent = motiveContainer;

                    switch (clue.name[0])
                    {
                        case 'S':
                            parent = suspectContainer;
                            break;
                        case 'W':
                            parent = weaponContainer;
                            break;
                    }

                    var card = Instantiate(clueCardPrefab).GetComponent<ClueCard>();
                    card.DrawCard(id, true, null);
                    card.transform.SetParent(parent, false);
                    card.transform.localScale = Vector3.one * .7f;
                }
            }


        }
    }
}