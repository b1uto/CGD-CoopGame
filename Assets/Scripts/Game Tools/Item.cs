using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace CGD
{
    /// <summary>
    /// Base class for all equippable tools. Includes Photon Code.
    /// </summary>
    public abstract class Item : MonoBehaviour, /*IPunObservable*/ IEquippable
    {
        #region Photon Setup
        private PhotonView pvCache;

        /// <summary>A cached reference to a PhotonView on this GameObject.</summary>
        /// <remarks>
        /// If you intend to work with a PhotonView in a script, it's usually easier to write this.photonView.
        ///
        /// If you intend to remove the PhotonView component from the GameObject but keep this Photon.MonoBehaviour,
        /// avoid this reference or modify this code to use PhotonView.Get(obj) instead.
        /// </remarks>
        public PhotonView photonView
        {
            get
            {
#if UNITY_EDITOR
                // In the editor we want to avoid caching this at design time, so changes in PV structure appear immediately.
                if (!Application.isPlaying || this.pvCache == null)
                {
                    this.pvCache = PhotonView.Get(this);
                }
#else
                if (this.pvCache == null)
                {
                    this.pvCache = PhotonView.Get(this);
                }
#endif
                return this.pvCache;
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        #endregion

        [SerializeField] protected GameObject interactionPrompt;
        [SerializeField] protected Rigidbody rb;

        [Header("Lag Compensation Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private bool teleportIfFar = true;
        [SerializeField] private float teleportDistance = 50;
        private Vector3 lastPosition;
        private Vector3 positionDelta;

        #region Interface Properties
        [Header("Interface Settings")]
        [SerializeField] protected bool interactable;
        [SerializeField] protected EquipSlot itemEquipSlot;

        protected int owner;

        public bool Interactable { get { return interactable; } set { } }
        public EquipSlot ItemEquipSlot { get { return itemEquipSlot; } set { } }
        public bool Equipped { get { return transform.parent != null; } set { } }
        public bool Equippable { get { return transform.parent == null; } set { } }
        #endregion

        #region IInteractable Functions
        public void OnExitFocus()
        {
            interactionPrompt.SetActive(false);
        }
        public void OnFocus()
        {
            if (!Equipped)
                interactionPrompt.SetActive(true);
        }
        public virtual void Interact(int viewId) { }
        #endregion

        #region IEquippable Functions
        public virtual void Equip(int viewId) { }
        public virtual void Unequip() { }
        #endregion


        /// <summary>
        /// Last PhotonView serialized Position
        /// </summary>
        private Vector3 networkPosition;
        /// <summary>
        /// Last PhotonView Serialized Rotation
        /// </summary>
        private Quaternion networkRotation;


        /// <summary>
        /// Used for non-physics objects and kinematic rigidbodies.
        /// </summary>
        public void Update()
        {
            //if (!photonView.IsMine)
            //{
            //    //If Equipped Item should be zeroed on slot.
            //    if (Equipped && rb.isKinematic)
            //    {
            //        transform.position = Vector3.Lerp(transform.position, networkPosition, moveSpeed * Time.deltaTime);
            //        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            //    }
            //}
            //else if(Equipped && transform.localPosition != Vector3.zero) 
            //{
            //    transform.localPosition = Vector3.zero;
            //    transform.localRotation = Quaternion.identity;
            //}

            if (Equipped)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                if (!Equipped)
                {
                    if (networkPosition == null || networkRotation == null) return;

                    rb.position = Vector3.Lerp(rb.position, networkPosition, Time.fixedDeltaTime * moveSpeed);
                    rb.rotation = Quaternion.Lerp(rb.rotation, networkRotation, Time.fixedDeltaTime * rotationSpeed);

                    if (teleportIfFar && Vector3.Distance(rb.position, networkPosition) > teleportDistance)
                    {
                        rb.position = networkPosition;
                    }
                }
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (rb.isKinematic)
                {
                    positionDelta = transform.localPosition - lastPosition;
                    lastPosition = transform.localPosition;

                    stream.SendNext(transform.position);
                    stream.SendNext(transform.rotation);
                    stream.SendNext(positionDelta);
                }
                else
                {
                    stream.SendNext(rb.position);
                    stream.SendNext(rb.rotation);
                    stream.SendNext(rb.velocity);
                }
            }
            else
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));


                if (rb.isKinematic)
                {
                    positionDelta = (Vector3)stream.ReceiveNext();
                    networkPosition += positionDelta * lag;
                }
                else
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkPosition += (rb.velocity * lag);
                }
            }
        }

    }
}