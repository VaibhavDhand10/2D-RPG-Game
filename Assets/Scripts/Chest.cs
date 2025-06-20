using UnityEditor.Tilemaps;
using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX entityVFX => GetComponentInChildren<Entity_VFX>();

    [Header("Chest Open")]
    [SerializeField] private Vector2 knockback;

    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        entityVFX.PlayOnDamageVFX();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);
        // drop items

        return true;
    }
}