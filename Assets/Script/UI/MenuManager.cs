using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ChangeScene(int idScene)
    {
        SceneManager.LoadScene(idScene);
    }
    public void ExitAplication()
    {
        Application.Quit();
    }
}
