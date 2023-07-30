using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles scene loading and the loading screen.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader current;

    [Header("Scene Loading References")]
    [Tooltip("The loading screen object.")]
    public GameObject loadingScreen;
    [Tooltip("The loading screen canvas group.")]
    public CanvasGroup canvasGroup;
    [Tooltip("The loading screen progress bar.")]
    public Slider progressBar;
    [Tooltip("The loading screen progress text.")]
    public TextMeshProUGUI progressText;
    [Tooltip("The loading screen title.")]
    public TextMeshProUGUI loadTitle;
    [Tooltip("The loading screen sound.")]
    public AudioClip sound;

    AsyncOperation loadingOp;
    bool delayOver = false;
    bool isLoading = false;
    bool animTitleInvoked = false;
    int curDots = 3;
    int maxDots = 3;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(loadingScreen);
        //DontDestroyOnLoad(loadingScreen);
        canvasGroup.alpha = 1f;
        loadingScreen.SetActive(false);
        //TransitionCanvas.current.Animate("blackFadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        {
            // Waits for the pre-load delay of 2 seconds before beginning anim for loading bar.
            if (delayOver)
            {
                float progressValue = Mathf.Clamp01(loadingOp.progress * 1f);
                progressBar.value = progressValue;
                progressText.text = Mathf.Round(progressValue * 100) + "%";
            }

            if (!animTitleInvoked)
            {
                InvokeRepeating("AnimTitle", 0f, 0.25f);
                animTitleInvoked = true;
                Debug.Log("Invoked.");
            }
        }
    }

    public void AnimTitle()
    {
        if (curDots < maxDots)
        {
            curDots++;
            string dots = new string('.', curDots);
            loadTitle.text = "LOADING" + dots;
        }
        else if (curDots == maxDots)
        {
            curDots = 0;
            loadTitle.text = "LOADING";
        }
    }

    /// <summary>
    /// Loads the specified scene.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName, bool noScreen = false)
    {
        //StartCoroutine(Timer(x => AudioHandler.current.StopMusic(), 1f));
        if (!noScreen)
        {
            //AudioHandler.current.PlaySound(sound);
            TransitionCanvas.current.Animate("blackFadeIn");
            StartCoroutine(Timer(x => StartCoroutine(StartLoad(sceneName, true)), 1.2f));
        }
        //StartCoroutine(Timer(x => SceneManager.LoadScene(sceneName), 1.2f));
        SceneManager.LoadScene(sceneName);
        //StartCoroutine(Timer(x => TransitionCanvas.current.Animate("blackFadeOut"), 1.2f));
    }

    /// <summary>
    /// Asynchronous scene load.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator StartLoad(string sceneName, bool wait = false)
    {
        isLoading = true;

        // Delay of 2 seconds before loading actually begins.
        if (wait)
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(2);
        }

        delayOver = true;
        loadingScreen.SetActive(true);
        canvasGroup.alpha = 1f;

        loadingOp = SceneManager.LoadSceneAsync(sceneName);
        while (!loadingOp.isDone)
        {
            yield return null;
        }

        StartCoroutine(FadeScreen(0, 1, LoadingScreenFadeType.FadeOut));

        isLoading = false;
        delayOver = false;
        animTitleInvoked = false;
    }

    /// <summary>
    /// Fades the screen alpha to the specified value over time.
    /// </summary>
    /// <param name="endVal"></param>
    /// <param name="dur"></param>
    /// <returns></returns>
    IEnumerator FadeScreen(float endVal, float dur, LoadingScreenFadeType fadeType)
    {
        float startVal = canvasGroup.alpha;
        //float startValAudio = AudioHandler.current.soundSource.volume;
        float time = 0;

        while (time < dur)
        {
            /*
            if (sound != null)
            {
                AudioHandler.current.soundSource.volume = Mathf.Lerp(startValAudio, endVal, time / dur);
            }*/
            canvasGroup.alpha = Mathf.Lerp(startVal, endVal, time / dur);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endVal;
        
        if (fadeType == LoadingScreenFadeType.FadeOut)
        {
            loadingScreen.SetActive(false);
            //AudioHandler.current.soundSource.Stop();
            isLoading = false;
            canvasGroup.alpha = 1;
            progressBar.value = 0;
            progressText.text = 0 + "%";
        }
        //AudioHandler.current.soundSource.volume = startValAudio;
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<SceneLoader>() != null)
            return;

        Instantiate(Resources.Load("SceneLoader"));
    }
}

public enum LoadingScreenFadeType
{
    FadeIn, FadeOut
}
