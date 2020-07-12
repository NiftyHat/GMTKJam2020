using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingController : MonoBehaviour
{
    [SerializeField] [NonNull] private Animator _animator;
    [SerializeField] [NonNull] private RectTransform _loadingBar;

    protected string _sceneToLoad;
    protected string _sceneToUnload;
    private static readonly int Start = Animator.StringToHash("Start");
    private bool _isUnloadComplete;
    public const string _transitionSceneName = "LoadingTransition";
    
    // Start is called before the first frame update

    // Update is called once per frame
    void TranstionScenes(string sceneToLoad, string sceneToUnload)
    {
        _sceneToLoad = sceneToLoad;
        _sceneToUnload = sceneToUnload;
        StartCoroutine(LoadAsynchronously(sceneToLoad, sceneToUnload));
    }

    IEnumerator LoadAsynchronously(string levelToLoad, string levelToUnload) {
        float timer = 0f;
        float minLoadTime = 0.5f;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;
 
        while (!loadOperation.isDone) {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingBar.localScale = new Vector3(progress, _loadingBar.localScale.y, _loadingBar.localScale.z);
            timer += Time.deltaTime;
 
            if (timer > minLoadTime)
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(levelToUnload);
                if (unloadOperation != null)
                {
                    unloadOperation.completed += operation =>
                    {
                        loadOperation.allowSceneActivation = true;
                        _isUnloadComplete = true;
                    };
                    
                }
                else
                {
                    loadOperation.allowSceneActivation = true;
                    _isUnloadComplete = true;
                }
                
            }
 
            yield return null;
        }

        while (!_isUnloadComplete)
        {
            yield return null;
        }
        _animator.SetTrigger(Start);
        yield return null;
    }

    private void HandleUnloadComplete(AsyncOperation obj)
    {
        
    }

    public static void LoadingTranstion(string sceneToLoad,
        Action<AsyncOperation> onStarted = null)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        LoadingTranstion(sceneToLoad, activeScene.name, onStarted);
    }

    public static void LoadingTranstion(string sceneToLoad, string sceneToUnload, Action<AsyncOperation> onStarted = null)
    {
        var loadingLoadingOp = SceneManager.LoadSceneAsync(_transitionSceneName, LoadSceneMode.Additive);
        loadingLoadingOp.completed += operation =>
        {
            if (operation.isDone)
            {
                Scene loadingScene = SceneManager.GetSceneByName(_transitionSceneName);
                if (!loadingScene.IsValid())
                {
                    Debug.Log($"Loading Error, transition scenen {_transitionSceneName} is marked as invalid");
                }
                
                var loadingController = FindObjectOfType<SceneLoadingController>();
                if (loadingController == null)
                {
                    Debug.Log($"Loading Error, transition loadingController was null so no animation can happen");
                }
                loadingController.TranstionScenes(sceneToLoad, sceneToUnload);
            }
        };
    }

    public static void RemoveLoadingScene()
    {
       Scene loadingScene = SceneManager.GetSceneByName(_transitionSceneName);
       if (loadingScene.IsValid())
       {
           SceneManager.UnloadSceneAsync(loadingScene);
       }
    }
}
