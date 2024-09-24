using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInvincibilityEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float cycleDuration;// 1 second for a full cycle
    public float lowAlpha;
    public float highAlpha;


    private void OnDisable()
    {
        // Apply the alpha value to the sprite's color
        Color newColor = spriteRenderer.color;
        newColor.a = 1f;
        spriteRenderer.color = newColor;
    }
    // Update is called once per frame
    void Update()
    {
        // Mathf.PingPong returns a value that oscillates between 0 and cycleDuration
        float pingPong = Mathf.PingPong(Time.time, cycleDuration) / cycleDuration;

        // Map pingPong value (0 to 1) to alpha range (0.25 to 1.0)
        float alpha = Mathf.Lerp(lowAlpha, highAlpha, pingPong);

        // Apply the alpha value to the sprite's color
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}