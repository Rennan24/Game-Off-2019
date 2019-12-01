using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public static PlayerController Player { get; private set; }
    public static PlayerUIManager PlayerUI { get; private set; }

    public static bool PlayerDead => Player.IsDead;

    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private float fadeTime = 1.0f;

    private string curLevelName;

    private AudioSource musicSource;

    public override void Awake()
    {
        base.Awake();

        UpdateReferences();

        musicSource = GetComponent<AudioSource>();

        musicSource.volume = 0;
        musicSource.DOFade(1, fadeTime * 2);

#if !UNITY_EDITOR
        fadeImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(LoadLevelRoutine("Forest"));
#else
        curLevelName = "Forest";
        LoadLevel(curLevelName);
#endif
    }

    private void UpdateReferences()
    {
        Player = FindObjectOfType<PlayerController>();
        PlayerUI = FindObjectOfType<PlayerUIManager>();
    }

    private IEnumerator FadeImageOutRoutine()
    {
        yield return new DOTweenCYInstruction.WaitForCompletion(fadeImage.DOFade(0.0f, fadeTime));
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelRoutine(levelName));
    }

    private IEnumerator LoadLevelRoutine(string levelName)
    {
        if (curLevelName != null)
        {
            yield return new DOTweenCYInstruction.WaitForCompletion(fadeImage.DOFade(1.0f, fadeTime));
            yield return SceneManager.UnloadSceneAsync("Forest");
        }

        yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        UpdateReferences();
        curLevelName = levelName;

        yield return new DOTweenCYInstruction.WaitForCompletion(fadeImage.DOFade(0.0f, fadeTime));
    }

    public static void AddScore(int amount)
    {
        PlayerUI.TargetScore += amount;
    }
}
