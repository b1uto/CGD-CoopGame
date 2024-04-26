using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CGD.UI
{
    public class ProfileMenu : MenuPanel
    {
        [SerializeField] private Image iconImg;
        [SerializeField] private Transform modelContainer;

        private int model = 0;
        private int icon = 0;

        private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        private void OnEnable()
        {
            if (ItemCollection.Instance && ItemCollection.Instance.UserProfile != null)
            {
                model = ItemCollection.Instance.UserProfile.model;
                icon = ItemCollection.Instance.UserProfile.icon;
                modelContainer.gameObject.SetActive(true);
                DrawModel();
                DrawIcon();
            }
        }

        private void OnDisable()
        {

            modelContainer.gameObject.SetActive(false);
        }

        private void DrawModel()
        {
            foreach (Transform child in modelContainer) { Destroy(child.gameObject); }

            var modelName = ItemCollection.GetModelName(model);
            var modelPath = System.IO.Path.Combine("Models", modelName);

            var prefab = prefabs.ContainsKey(modelName) ?
                prefabs[modelName] : Resources.Load<GameObject>(modelPath);

            if (prefab != null)
            {
                if (!prefabs.ContainsKey(modelName))
                    prefabs.Add(modelName, prefab);

                Instantiate(prefab, modelContainer);
            }
        }

        private void DrawIcon()
        {
            var sprite = ItemCollection.Instance.Avatars[icon];
            iconImg.sprite = sprite;
        }

        #region Buttons
        public void Btn_NextIcon()
        {
            if (++icon >= ItemCollection.Instance.Avatars.Length)
                icon = 0;

            DrawIcon();
        }
        public void Btn_NextModel()
        {
            if (++model >= ItemCollection.GetModelCount())
                model = 0;

            DrawModel();
        }

        public void Btn_Apply()
        {
            ItemCollection.Instance.UpdateUserProfile(model, icon);
        }
        #endregion

    }
}