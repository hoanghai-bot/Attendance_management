using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.Script
{
    public class SceneSwitcher : MonoBehaviour
    {
        public bool reset = false;
        public void SwitchToScene(string sceneName)
        {
            if (reset)
            {
                PlayerPrefs.SetString("id","");
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
