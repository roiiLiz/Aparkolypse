using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TweenUIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    [SerializeField]
    private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField]
    private float tweenDuration = 0.2f;

    private Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = defaultScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(BeginTween(defaultScale, hoverScale, tweenDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(BeginTween(hoverScale, defaultScale, tweenDuration));
    }

    private IEnumerator BeginTween(Vector3 from, Vector3 to, float tweenDuration)
    {
        float t = 0.0f;
        float tweenRate = (1.0f / tweenDuration);

        while (t < 1.0f)
        {
            t += Time.unscaledDeltaTime * tweenRate;
            transform.localScale = Vector3.Lerp(from, to, easingCurve.Evaluate(t));
            yield return null;
        }
    }
}
