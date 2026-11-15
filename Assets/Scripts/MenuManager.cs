using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    public void Quitter()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
