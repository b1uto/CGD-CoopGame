using Photon.Pun;
using System.IO.Pipes;
using UnityEngine;


namespace CGD
{
    public class Torch : Item
    {
        [Header("Settings")]
        [SerializeField] private float intensity;
        [SerializeField] private Renderer emissiveMat;


        private bool flashLightOn = false;
        private Color alphaStart;

        private static Light pointLight;
        public static Light PointLight { get { return pointLight; } }

        
        #region MonoBehaviour
        private void Awake()
        {
            pointLight = GetComponentInChildren<Light>();
            alphaStart = emissiveMat.material.color;
        }
        #endregion

        #region Interface Function Overrides
        public override void Interact() => photonView.RPC(nameof(ToggleTorch), RpcTarget.All, flashLightOn);
        public override void Equip(int viewId) => photonView.RPC(nameof(TorchEquipped), RpcTarget.All, viewId);
        public override void Unequip() => photonView.RPC(nameof(TorchDropped), RpcTarget.All);
        #endregion

        #region RPCs
        [PunRPC]
        private void TorchEquipped(int viewId)
        {
            var view = PhotonView.Find(viewId);

            if (view != null && view.TryGetComponent<PlayerAnimController>(out var animController))
            {
                var slot = animController.GetEquipSlot(itemEquipSlot);
                OnExitFocus();
                rb.isKinematic = true;

                transform.SetParent(slot);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                Owner = slot;
            }
        }

        [PunRPC]
        private void TorchDropped()
        {
            transform.SetParent(null);
            Owner = null;
            rb.isKinematic = false;
        }

        [PunRPC]
        private void ToggleTorch(bool lastFlashLightStatus)
        {
            flashLightOn = !lastFlashLightStatus;

            if (flashLightOn)
            {
                pointLight.intensity = intensity;
                emissiveMat.material.SetColor("_EmissionColor", alphaStart * pointLight.intensity);
            }
            else
            {
                pointLight.intensity = 0.0f;
                emissiveMat.material.SetColor("_EmissionColor", alphaStart * Color.black);
            }
        }
        #endregion

    }
}