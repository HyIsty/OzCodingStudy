using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private bool isOpened = false;

    public void Interact()
    {
        Debug.Log($"[Chest] Interact {name}");
        if (isOpened)
            return;

        Debug.Log($"[Chest] Open {name}");
    }

    public void Hovering()
    {
        text.text = $"[E] Open {name}";
        text.gameObject.SetActive(true);
    }

    public void UnHovering()
    {
        text.gameObject.SetActive(false);
    }
}
