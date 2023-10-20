using UnityEngine;
using UnityEngine.EventSystems;

namespace main.Script.service
{
    public class ClickPanel : MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            this.gameObject.SetActive(false);
        }

        public void Clickbutton()
        {
            this.gameObject.SetActive(false);
        }
    }
}
