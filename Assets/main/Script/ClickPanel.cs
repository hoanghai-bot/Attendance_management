using UnityEngine;
using UnityEngine.EventSystems;

namespace main.Script
{
    public class ClickPanel : MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            this.gameObject.SetActive(false);
        }
    }
}
