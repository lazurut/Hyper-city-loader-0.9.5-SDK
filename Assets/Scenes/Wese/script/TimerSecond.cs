using UnityEngine;
using TMPro;

public class StopwatchTimer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI savedTimeText; // Отображает сохранённое время

    [Header("Settings")]
    [SerializeField] private bool startOnAwake = false;

    [Header("Save")]
    [SerializeField] private string saveKey = "SavedStopwatchTime"; // Ключ для PlayerPrefs

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        LoadSavedTime();

        if (startOnAwake)
            StartTimer();

        UpdateDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateDisplay();
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        isRunning = false;
        elapsedTime = 0f;
        UpdateDisplay();
    }

    public void ToggleTimer() => isRunning = !isRunning;

    // Сохраняет текущее время в PlayerPrefs и обновляет savedTimeText
    public void SaveCurrentTime()
    {
        PlayerPrefs.SetFloat(saveKey, elapsedTime);
        PlayerPrefs.Save();
        UpdateSavedDisplay(elapsedTime);
        Debug.Log($"[Stopwatch] Время сохранено: {elapsedTime}");
    }

    // Загружает сохранённое время при старте сцены
    private void LoadSavedTime()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            float saved = PlayerPrefs.GetFloat(saveKey);
            UpdateSavedDisplay(saved);
            Debug.Log($"[Stopwatch] Загружено сохранённое время: {saved}");
        }
        else
        {
            if (savedTimeText != null)
                savedTimeText.text = "Сохранено: --:--:--";
        }
    }

    // Удаляет сохранённое время
    public void ClearSavedTime()
    {
        PlayerPrefs.DeleteKey(saveKey);
        if (savedTimeText != null)
            savedTimeText.text = "Сохранено: --:--:--";
    }

    private void UpdateDisplay()
    {
        if (timerText != null)
            timerText.text = FormatTime(elapsedTime);
    }

    private void UpdateSavedDisplay(float time)
    {
        if (savedTimeText != null)
            savedTimeText.text = "Сохранено: " + FormatTime(time);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public float GetElapsedTime() => elapsedTime;
    public bool IsRunning() => isRunning;
}


