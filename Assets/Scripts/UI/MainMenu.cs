using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Scenes/RemakeUI");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
