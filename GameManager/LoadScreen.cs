using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public Slider _progressBar;
    public Text _loadingText;
    private string _sceneLoading;
    private GameState _stateLoading;
    private string _defaultLoad = SceneList.Systems;



    void Start() 
    {
        if (LoadingScene.Instance != null)
        {
            _sceneLoading = LoadingScene.Instance._sceneToLoad;
            
            _stateLoading = LoadingScene.Instance._stateToLoad;

        }
        
        else
        {
            _sceneLoading = _defaultLoad;
        }

        StartCoroutine(LoadAsyncOperation());

    }


    IEnumerator LoadAsyncOperation()
    {

        AsyncOperation gameScene = SceneManager.LoadSceneAsync(_sceneLoading);
        gameScene.allowSceneActivation = false;


        while (!gameScene.isDone)
        {
            float progress = Mathf.Clamp01(gameScene.progress / 0.9f);


            if (_progressBar != null)
            {
                _progressBar.value = progress;
            }


            if (_loadingText != null)
            {
                _loadingText.text = (int)(progress * 100) + "%";
            }


            if (gameScene.progress >= 0.9f)
            {
                if (_progressBar != null)
                {
                    _progressBar.value = 1f;
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChangeGameState(_stateLoading);
                }

                gameScene.allowSceneActivation = true;

            }


            yield return null;
        }

    }
}
