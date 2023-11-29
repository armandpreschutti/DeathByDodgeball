using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectHandler : MonoBehaviour
{

    [SerializeField] float _selfDestructTime;
    [SerializeField] bool _fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        if(_fadeOut)
        {
            StartCoroutine(LowerAlphaOverTime(GetComponent<SpriteRenderer>(), _selfDestructTime));
        }
        Destroy(this.gameObject, _selfDestructTime);

    }
    private IEnumerator LowerAlphaOverTime(SpriteRenderer spriteRenderer, float duration)
    {
        float currentTime = -2f;
        Color originalColor = spriteRenderer.color;
        Color transparentColor = originalColor;

        while (currentTime < duration +4)
        {
            currentTime += Time.deltaTime;
            transparentColor.a = Mathf.Lerp(originalColor.a, 0f, currentTime / duration);
            spriteRenderer.color = transparentColor;
            yield return null;
        }

        transparentColor.a = 0f;
        spriteRenderer.color = transparentColor;
    }


}
