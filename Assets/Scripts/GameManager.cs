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

    private int curLevelIndex;

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
        StartCoroutine(LoadLevelRoutine(1));
#else
        StartCoroutine(FadeImageOutRoutine());
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

    public IEnumerator LoadLevelRoutine(int levelIndex)
    {
        if (curLevelIndex != 0)
        {
            yield return new DOTweenCYInstruction.WaitForCompletion(fadeImage.DOFade(1.0f, fadeTime));
            yield return SceneManager.UnloadSceneAsync(curLevelIndex);
        }

        yield return SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);

        UpdateReferences();
        curLevelIndex = levelIndex;

        yield return new DOTweenCYInstruction.WaitForCompletion(fadeImage.DOFade(0.0f, fadeTime));
    }

    public static void AddScore(int amount)
    {
        PlayerUI.TargetScore += amount;
    }
}
