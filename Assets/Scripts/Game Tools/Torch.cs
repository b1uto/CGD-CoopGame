using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IEquippable
{
    [Header("Scene References")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Light pointLight;
    [SerializeField] private EmissionMaterialGlassTorchFadeOut emissiveMat;

    [Header("Settings")]
    public float intensity;

    [SerializeField] private bool interactable;
    [SerializeField] private EquipSlot itemEquipSlot;

    private bool flashLightOn = false;
    private Transform Owner;

    public bool Interactable { get { return interactable; } set { } }
    public EquipSlot ItemEquipSlot { get { return itemEquipSlot; } set { } }
    public bool Equipped { get { return Owner; } set { } }
    public bool Equippable { get { return Owner == null; } set { } }


    #region IInteractable Functions
    public void OnExitFocus()
    {
        interactionPrompt.SetActive(false);
    }

    public void OnFocus()
    {
        if(!Equipped)
            interactionPrompt.SetActive(true);
    }
    public void Interact() => ToggleTorch();
    #endregion

    #region IEquippable Functions
    public void Equip(Transform slot)
    {
        OnExitFocus();
        rb.isKinematic = true;

        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Owner = slot; 

    }
    public void Unequip()
    {
        Owner = null;
        rb.isKinematic = false;
        transform.SetParent(null);
    }
    #endregion
    private void ToggleTorch()
    {
        flashLightOn = !flashLightOn;

        if (flashLightOn)
        {
            pointLight.intensity = intensity;
            emissiveMat.OnEmission();
        }
        else
        {
            pointLight.intensity = 0.0f;
            emissiveMat.OffEmission();
        }
    }
}
