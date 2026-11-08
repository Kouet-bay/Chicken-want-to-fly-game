using UnityEngine;

public class Character : Identity, IDamageable
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int hp;
    [SerializeField] protected int attackPoints;
    [SerializeField] protected int defensePoints;

    public int MaxHp
    {
        get { return maxHp; }
        protected set { maxHp = value; }
    }

    public int Hp
    {
        get { return hp; }
        // Encapsulation win: Hp can never be set outside 0..MaxHp from anywhere in the code.
        set { hp = Mathf.Clamp(value, 0, maxHp); }
    }

    protected virtual void Start()
    {
        Hp = MaxHp;
    }

    public virtual void TakeDamage(int damage, Character attacker)
    {
        int effectiveDamage = Mathf.Max(damage - defensePoints, 0);
        Hp -= effectiveDamage;
        if (Hp <= 0)
        {
            Die(attacker);
        }
    }
    public virtual void Die(Character attacker)
    {
        // Handle character death (e.g., play animation, disable character, etc.)
        if (attacker != null)
        {
            // Exp เป็นสถานะรวมของเกม (ความก้าวหน้าของผู้เล่น) จึงให้ GameManager ตัวเดียวเป็นคนเก็บและคำนวณเลเวลอัป
            GameManager.Instance?.GainExperience(50); // Example: 50 experience points for defeating this character
        }
        Debug.Log($"{IdentityName} has died.");
        this.enabled = false; // Disable the character's script to prevent further actions
        Destroy(gameObject,3); // Remove the character from the scene after 3 seconds
    }
    public virtual void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, MaxHp);
    }
    public virtual void Attack(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(attackPoints, this);
            Debug.Log($"{IdentityName} attacks {(target as Identity)?.IdentityName} for {attackPoints} damage.");
        }
    }
}