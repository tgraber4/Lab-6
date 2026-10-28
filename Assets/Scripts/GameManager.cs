using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Playing, WaveCleared, Won, Lost }

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int totalWaves = 3;
    public float timeBetweenWaves = 2f;

    [Header("Wave Scaling & Rules")]
    public int startingAmmo = 10;
    public int ammoRefillPerWave = 5;

    [Header("UI")]
    public TMP_Text scoreText, ammoText, waveText, resultText;

    [Header("Target Management")]
    public Target[] allTargets; // Drag all your target objects into this array in the Inspector

    public GameState state = GameState.Playing;
    private int score, ammo, projectilesInFlight, targetsNeededThisWave;

    void Awake()
    {
        if (Instance != null && Instance != this)
        { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        state = GameState.Playing;

        // Targets needed this wave equals how many targets are in your scene (or set a custom rule)
        targetsNeededThisWave = allTargets.Length;
        score = 0;

        ammo = startingAmmo + ((currentWave - 1) * ammoRefillPerWave);
        projectilesInFlight = 0;

        // Respawn all targets for this wave
        foreach (Target target in allTargets)
        {
            if (target != null)
            {
                // Optional: If you want them to respawn at random positions per wave, 
                // you can pass a new Vector3 here. Otherwise, leave empty to use their original positions.
                target.Respawn();
            }
        }

        UpdateHUD();
    }

    public bool CanShoot() => state == GameState.Playing && ammo > 0;

    public void ShotFired()
    {
        if (!CanShoot()) return;
        ammo--;
        projectilesInFlight++;
        UpdateHUD();
    }

    public void AddScore(int points)
    {
        if (state != GameState.Playing) return;
        score += points;

        if (score >= targetsNeededThisWave)
        {
            if (currentWave >= totalWaves)
            {
                state = GameState.Won;
            }
            else
            {
                state = GameState.WaveCleared;
                StartCoroutine(AdvanceToNextWaveRoutine());
            }
        }
        UpdateHUD();
    }

    public void ProjectileFinished()
    {
        projectilesInFlight = Mathf.Max(0, projectilesInFlight - 1);

        if (state == GameState.Playing && ammo == 0 && projectilesInFlight == 0 && score < targetsNeededThisWave)
        {
            state = GameState.Lost;
        }
        UpdateHUD();
    }

    IEnumerator AdvanceToNextWaveRoutine()
    {
        if (resultText) resultText.text = "WAVE " + currentWave + " CLEARED!";
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        StartWave();
    }

    void UpdateHUD()
    {
        if (scoreText) scoreText.text = "Score: " + score + "/" + targetsNeededThisWave;
        if (ammoText) ammoText.text = "Ammo: " + ammo;
        if (waveText) waveText.text = "Wave: " + currentWave + "/" + totalWaves;

        if (resultText)
        {
            if (state == GameState.Won) resultText.text = "YOU WIN!";
            else if (state == GameState.Lost) resultText.text = "GAME OVER!";
            else if (state == GameState.WaveCleared) resultText.text = "WAVE " + currentWave + " CLEARED!";
            else resultText.text = "";
        }
    }
}