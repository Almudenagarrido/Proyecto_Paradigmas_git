using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum PlanetType
    {
        Bounce,
        Gravity,
        Death
    }

    public PlanetType planetType;
    public Sprite planetSprite;
    public Sprite[] noiseSprites;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer noiseRenderer;

    private int noiseSpriteIndex = 0;
    public float repeatTime = 0.15f;
    public float noiseAlpha = 0.4f;
    public float noiseScale = 1.15f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = planetSprite;

        // Crear un objeto hijo para representar el ruido
        GameObject noiseObject = new GameObject("NoiseLayer");
        noiseObject.transform.SetParent(transform);
        noiseObject.transform.localPosition = Vector3.zero;

        noiseRenderer = noiseObject.AddComponent<SpriteRenderer>();
        noiseRenderer.sortingOrder = 1;
        float fixedNoiseScale = 1.0f; // Ajusta este valor para el tamaño deseado
        noiseObject.transform.localScale = new Vector3(fixedNoiseScale, fixedNoiseScale, 1);
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
        switch (planetType)
        {
            case PlanetType.Bounce:
                break;
            case PlanetType.Gravity:
                break;
            case PlanetType.Death:
                break;
        }
    }
}
