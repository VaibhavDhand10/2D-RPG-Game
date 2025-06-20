using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;
    [Header("Health Regen")]
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegenerateHealth = true;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("On Heavy Damage Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = 0.3f; // threshold of HP you lose after a heavy knockback
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHealth = entityStats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead) return false;

        if (AttackEvaded())
        {
            Debug.Log($"{ gameObject.name} evaded the attack!");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        return true;
    }

    private bool AttackEvaded() => Random.Range(0,100) < entityStats.GetEvasion();

    private void RegenerateHealth()
    {
        if (!canRegenerateHealth) return;

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    private void IncreaseHealth(float healAmount)
    {
        if (isDead) return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }


    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);
        entity?.ReceiveKnockback(knockback, duration);
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = isHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage)
    {
        return isHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool isHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() > heavyDamageThreshold;
}