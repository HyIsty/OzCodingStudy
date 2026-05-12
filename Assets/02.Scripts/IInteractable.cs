using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void Hovering();
    public void UnHovering();
}

public interface IDamageable
{
    public void TakeDamage(int amount);
}