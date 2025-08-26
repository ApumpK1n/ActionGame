using UnityEngine;
using UnityEngine.UI;
using DamageNumbersPro;

namespace DamageNumbersPro
{
    /// <summary>
    /// Attach this script to custom images or sprite renderers in your damage number prefab.
    /// It will handle fading them in and out with the popup.
    /// 
    /// You can also use Text Mesh Pro sprites instead.
    /// </summary>
    public class DamageNumberSprite : MonoBehaviour
    {
        [Header("Size:")]
        public bool matchTextSize = false;
        public Vector2 sizePadding = new Vector2(0.3f, 0.2f);
        public Vector2 sizeFactor = new Vector2(1f, 1f);

        void Start()
        {
            // Get damage number
            DamageNumber damageNumber = GetComponentInParent<DamageNumber>();

            // Fade graphic or image
            Graphic graphic = GetComponent<Graphic>();
            if (graphic != null)
            {
                // Update color
                Color color = graphic.color;
                float originalAlpha = graphic.color.a;
                damageNumber.OnUpdateFade += (alpha) =>
                {
                    graphic.color = new Color(color.r, color.g, color.b, originalAlpha * alpha);
                };

                // Update size
                if (matchTextSize)
                {
                    RectTransform rectTransform = graphic.rectTransform;

                    damageNumber.OnUpdateText += () =>
                    {
                        UpdateSize(damageNumber, rectTransform);
                    };
                }
            }
            else
            {
                // Fade sprite renderer
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Update color
                    Color color = spriteRenderer.color;
                    float originalAlpha = spriteRenderer.color.a;
                    damageNumber.OnUpdateFade += (alpha) =>
                    {
                        spriteRenderer.color = new Color(color.r, color.g, color.b, originalAlpha * alpha);
                    };

                    // Update size
                    if (matchTextSize)
                    {
                        damageNumber.OnUpdateText += () =>
                        {
                            UpdateSize(damageNumber, spriteRenderer);
                        };
                    }
                }
            }
        }

        /// <summary>
        /// This function will be called by the script itself.
        /// It's only public so that the editor script can call it.
        /// </summary>
        public void UpdateSize(DamageNumber damageNumber, SpriteRenderer spriteRenderer)
        {
            // Get text size
            Vector2 textSize = damageNumber.GetTextMesh().textBounds.size;

            if (spriteRenderer.drawMode == SpriteDrawMode.Simple)
            {
                // Update scale
                transform.localScale = new Vector3((textSize.x + sizePadding.x) * sizeFactor.x, (textSize.y + sizePadding.y) * sizeFactor.y, 1);
            }
            else
            {
                // Update sliced size
                spriteRenderer.size = new Vector2((textSize.x + sizePadding.x) * sizeFactor.x, (textSize.y + sizePadding.y) * sizeFactor.y);
            }
        }

        /// <summary>
        /// This function will be called by the script itself.
        /// It's only public so that the editor script can call it.
        /// </summary>
        public void UpdateSize(DamageNumber damageNumber, RectTransform rectTransform)
        {
            // Get text size
            Vector2 textSize = damageNumber.GetTextMesh().textBounds.size;

            // Update size delta
            rectTransform.sizeDelta = new Vector2((textSize.x + sizePadding.x) * sizeFactor.x, (textSize.y + sizePadding.y) * sizeFactor.y);
        }
    }
}
