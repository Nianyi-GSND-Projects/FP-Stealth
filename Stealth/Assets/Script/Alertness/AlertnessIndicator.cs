using UnityEngine;
using UnityEngine.UI;

public class AlertnessIndicator : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RectMask2D rectMask;

	float value;
	public float Value {
		get => value;
		set
		{
			this.value = value;

			float height = rectMask.rectTransform.rect.height;
			var padding = rectMask.padding;
			padding.w = height * (1 - value);
			rectMask.padding = padding;

			canvasGroup.alpha = value;
		}
	}
}
