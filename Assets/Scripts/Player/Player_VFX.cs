using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.5f;
    [SerializeField] private GameObject imageEchoPrefab;

    private Coroutine imageEchoCo;

    public void DoImageEchoEffect(float duration)
    {
        if(imageEchoCo != null)
            StopCoroutine(imageEchoCo);

        imageEchoCo = StartCoroutine(ImageEchoEffectCO(duration));
    }
    
    private IEnumerator ImageEchoEffectCO(float duration)
    {
        float timeTracker = 0;
        while (timeTracker < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker = timeTracker + imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}