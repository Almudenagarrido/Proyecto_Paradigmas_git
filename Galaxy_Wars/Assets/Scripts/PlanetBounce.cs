using UnityEngine;

public class PlanetBounce : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite planetSprite;
    public Sprite[] noiseSprites;
    private SpriteRenderer noiseRenderer;
    private int noiseSpriteIndex = 0;
    public float repeatTime = 0.05f;
    public float noiseAlpha = 0.5f;
    public float noiseScale = 0.13f;

    private void Awake()
    {
        // Obtener el SpriteRenderer del planeta base
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = planetSprite;

        // Crear un nuevo GameObject para el ruido
        GameObject noiseObject = new GameObject("NoiseLayer");
        noiseObject.transform.SetParent(transform);
        noiseObject.transform.localPosition = Vector3.zero;

        // Añadimos el SpriteRenderer al GameObject del ruido
        noiseRenderer = noiseObject.AddComponent<SpriteRenderer>();
        noiseRenderer.sortingOrder = 1;
        noiseObject.transform.localScale = new Vector3(noiseScale, noiseScale, 1f);
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateNoiseSprite), repeatTime, repeatTime);
    }

    private void AnimateNoiseSprite()
    {
        noiseSpriteIndex++;
        if (noiseSpriteIndex >= noiseSprites.Length)
        {
            noiseSpriteIndex = 0;
        }

        noiseRenderer.sprite = noiseSprites[noiseSpriteIndex];

        Color newColor = noiseRenderer.color;
        newColor.a = noiseAlpha;
        noiseRenderer.color = newColor;
    }

    private void Update()
    {
    }
}
