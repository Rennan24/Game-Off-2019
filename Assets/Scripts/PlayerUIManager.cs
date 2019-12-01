using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerUIManager : MonoBehaviour
{
    public int TargetScore;
    protected float SmoothScore = 0;

    public Image GunTypeOutline;
    public TextMeshProUGUI AmmoCountText;

    public TextMeshProUGUI DeathText;

    [SerializeField]
    private float ScoreSmoothing;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private float WaveTextFadeTime;

    [SerializeField]
    private float WaveTextHoldTime;

    [SerializeField]
    private RectTransform healthbarRect;

    [SerializeField]
    private float punchAddition;
    [SerializeField]
    private Vector2 punchRangeX, punchRangeY;
    [SerializeField]
    private float punchDuration;

    [SerializeField]
    private TextMeshProUGUI WaveText;

    [SerializeField]
    private Image[] hearts;

    private HealthBehaviour playerHealth;

    private PlayerController player;

    private void Start()
    {
        player = GameManager.Player;

        playerHealth = player.Health;

        playerHealth.Damaged += OnDamaged;

        playerHealth.Killed += PlayerHealthOnKilled;
        UpdateHealth();

        StartCoroutine(ShowWaveRoutine(1));
    }

    private IEnumerator PlayerKilled()
    {
        yield return new WaitForSeconds(3.0f);
        DeathText.DOFade(1, 2f);
        StartCoroutine(GameManager.Inst.LoadLevelRoutine(1));
    }

    private void PlayerHealthOnKilled(Vector3 hitpos, Vector2 hitdir)
    {
        StartCoroutine(PlayerKilled());
    }

    private void OnDamaged(Vector3 hitpos, Vector2 hitdir, int hitamount, int curhealth)
    {
        var x = (punchAddition + Random.Range(punchRangeX.x, punchRangeX.y)) * (Random.value > 0.5f ? 1 : -1);
        var y = (punchAddition + Random.Range(punchRangeY.x, punchRangeY.y)) * (Random.value > 0.5f ? 1 : -1);
        healthbarRect.DOPunchPosition(new Vector3(x, y), punchDuration);
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = false;

        for (int i = 0; i < playerHealth.CurHealth; i++)
            hearts[i].enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        SmoothScore = Mathf.Lerp(SmoothScore, TargetScore, ScoreSmoothing * Time.deltaTime);
        ScoreText.text = $"{Mathf.RoundToInt(SmoothScore):N0}";
        GunTypeOutline.overrideSprite = player.CurWeapon.OutlineSprite;
        AmmoCountText.text = $"{player.CurWeapon.AmmoCount}";
    }

    public IEnumerator ShowWaveRoutine(int waveNum)
    {
        WaveText.text = $"Wave {waveNum}";
        WaveText.DOFade(1f, WaveTextFadeTime);
        yield return new WaitForSeconds(WaveTextFadeTime + WaveTextHoldTime);
        WaveText.DOFade(0f, WaveTextFadeTime);
    }
}
