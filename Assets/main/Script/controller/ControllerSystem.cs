using UnityEngine;

namespace main.Script.controller
{
    public class ControllerSystem : FireBaseCommon
    {
        public GameObject button;
        // Start is called before the first frame update
        async void Start()
        {
            base.Start();
            if (await CheckPermission("system"))
            {
                button.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
