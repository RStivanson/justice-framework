using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JusticeFramework.Utility {
	[RequireComponent(typeof(Button))]
	public class ButtonTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		[SerializeField]
		private Text buttonText;

		[SerializeField]
		private Color highlightColor;
		
		[SerializeField]
		private Color pressedColor;

		[SerializeField]
		[HideInInspector]
		private Button button;
		
		[SerializeField]
		[HideInInspector]
		private Color initialColor;

		[SerializeField]
		[HideInInspector]
		private bool isHovering;

		private void Start() {
			initialColor = buttonText.color;
			isHovering = false;
		}
		
		public void OnPointerEnter(PointerEventData eventData) {
			isHovering = true;
			SetColor(highlightColor);
		}

		public void OnPointerExit(PointerEventData eventData) {
			isHovering = false;
			SetColor(initialColor);
		}

		public void OnPointerDown(PointerEventData eventData) {
			SetColor(pressedColor);
		}

		public void OnPointerUp(PointerEventData eventData) {
			SetColor(isHovering ? highlightColor : initialColor);
		}

		private void SetColor(Color color) {
			buttonText.color = color;
		}
	}
}