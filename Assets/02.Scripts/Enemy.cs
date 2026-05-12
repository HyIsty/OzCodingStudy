using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stat")]
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int maxHp;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        currentHp = Mathf.Clamp(currentHp - amount, 0, maxHp);

        Debug.Log($"[Enemy] Take {amount} damage");
        if(currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
