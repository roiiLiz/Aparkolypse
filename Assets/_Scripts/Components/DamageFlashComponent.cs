using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlashComponent : MonoBehaviour
{
    [SerializeField]
    private float flashTime = 0.25f;
    [SerializeField]
    private Color flashColor = new Color(255, 0, 0, 255);
    private Color defaultColor;
    [SerializeField]
    private Image sprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

        if (sprite == null)
        {
            sprite = GetComponent<Image>();
            if (sprite == null)
            {
                return;
            }
        }
        defaultColor = sprite.color;
    }

    public void BeginDamageFlash()
    {
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        // Debug.Log("Damage Flasher");
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = flashColor;
            } else
            {
                sprite.color = flashColor;
            }

            yield return null;
        }


        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        } else
        {
            sprite.color = defaultColor;
        }
    }
}