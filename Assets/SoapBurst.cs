using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoapBurst : MonoBehaviour
{
    public GameObject bubblePrefab; // Reference to the bubble prefab
    public Vector2 scale = Vector2.one;
    public Vector2 velocityY = Vector2.one;
    public Vector2 velocityX = Vector2.one;
    public Vector2 fade = Vector2.one;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BurstIntoBubbles(false);
        }
    }

    // Call this method to burst the soap into bubbles
    public void BurstIntoBubbles(bool destroy = true)
    {
        if (spriteRenderer == null || bubblePrefab == null)
        {
            Debug.LogError("SpriteRenderer or BubblePrefab not assigned.");
            return;
        }

        // Get the texture from the sprite
        Texture2D texture = spriteRenderer.sprite.texture;

        if (texture == null)
        {
            Debug.LogError("Sprite texture is missing.");
            return;
        }

        // Read all pixels of the texture
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Get the pixel color
                Color pixelColor = pixels[x + y * width];

                // Check alpha value
                if (pixelColor.a > 0.7f && Random.value > 0.8f)
                {
                    // Spawn a bubble prefab at the corresponding position
                    Vector3 worldPosition = transform.position + new Vector3((float)x / width - 0.5f, (float)y / height - 0.5f, 0f);
                    GameObject bubble = Instantiate(bubblePrefab, worldPosition, Quaternion.identity);

                    // Set the bubble's color
                    SpriteRenderer bubbleSpriteRenderer = bubble.GetComponent<SpriteRenderer>();
                    if (bubbleSpriteRenderer != null)
                    {
                        bubbleSpriteRenderer.color = pixelColor;
                    }

                    // Set the bubble's velocity upwards
                    Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = new Vector2(velocityX.RandomRange(), velocityY.RandomRange());
                    }
                    bubbleSpriteRenderer.DOFade(0.2f, fade.RandomRange()).SetEase(Ease.OutSine);
                }
            }
        }

        // Destroy the soap game object after bursting
        if(destroy) Destroy(gameObject);
    }
}
