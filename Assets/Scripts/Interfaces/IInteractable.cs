public interface IInteractable
{
    /// <summary>
    /// Toggles whether this object can be interacted with.
    /// </summary>
    public bool Interactable {  get; set; }

    /// <summary>
    /// Called from raycast hit result
    /// </summary>
    public abstract void OnFocus();

    /// <summary>
    /// Called to reset after raycast hit.
    /// </summary>
    public abstract void OnExitFocus();

    /// <summary>
    /// Begin Interaction
    /// </summary>
    public abstract void Interact();

}
