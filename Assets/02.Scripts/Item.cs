using TMPro;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TMP_Text text;

    public void Interact()
    {
        Debug.Log($"[Item] Interact {name}");
    }

    public void Hovering()
    {
        text.text = $"[E] Pick {name}";
        text.gameObject.SetActive(true);
    }

    public void UnHovering()
    {
        text.gameObject.SetActive(false);
    }
}
