using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.Script.controller
{
    public class ControllerHome : FireBaseCommon
    {

        async void Start()
        {
            base.Start();
            if (await CheckPermission("system"))
            {
                SceneManager.LoadScene("HomeSystem");
            }
            
        }

        
        
    }
}
