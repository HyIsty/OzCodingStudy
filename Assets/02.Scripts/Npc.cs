using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TMP_Text text;

    public void Interact()
    {
        Debug.Log($"[NPC] Interact {name}");
    }

    public void Hovering()
    {
        text.text = $"[E] Talk to {name}";
        text.gameObject.SetActive(true);
    }

    public void UnHovering()
    {
        text.gameObject.SetActive(false);
    }
}
