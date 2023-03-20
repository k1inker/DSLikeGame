using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider _loadingSlider;

    public void LoadLevelBtn(int idScene)
    {
        _mainMenu.SetActive(false);
        _loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(idScene));
    }
    IEnumerator LoadLevelASync(int idLevel)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(idLevel);
        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
    }    
}
