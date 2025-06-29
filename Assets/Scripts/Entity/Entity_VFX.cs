using System.Collections;
using Unity.Android.Gradle;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject critHitVFX;

    [Header("Elemental Colors")]
    [SerializeField] private Color chillVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color electrifyVFX = Color.yellow;
    private Color originalHitVFXColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        originalHitVFXColor = hitVFXColor;
    }

    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCO(duration, chillVFX));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCO(duration, burnVFX));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVFXCO(duration, electrifyVFX));
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVFXCO(float duration, Color effectColor)
    {
        float tickInterval = 0.25f;
        float timePassed = 0f;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.9f;

        bool toggle = false;

        while (timePassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
        }
        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;

        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0); // rotates the critical hit VFX according to entity
    }

    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.None)
            hitVFXColor = originalHitVFXColor;
        
        if (element == ElementType.Ice)
            hitVFXColor = chillVFX;
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);

        onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCo());
    }

    private IEnumerator OnDamageVFXCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }
}