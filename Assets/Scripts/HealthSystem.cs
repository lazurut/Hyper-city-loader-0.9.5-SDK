using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Система жизни персонажа с Image Fill для отображения полоски здоровья.
/// Прикрепите к объекту персонажа. 
/// В Image HP Bar установите Image Type = Filled, Fill Method = Horizontal.
/// </summary>
public class HealthSystem : MonoBehaviour
{
    [Header("Настройки здоровья")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private Image hpBarImage;
    [SerializeField] private Text hpText;

    [Header("Плавность полоски")]
    [SerializeField] private bool enableSmooth = true;
    [SerializeField] private float smoothSpeed = 5f; // скорость плавного изменения

    [Header("Рестарт сцены")]
    [SerializeField] private bool enableSceneRestart = true;
    [SerializeField] private float restartDelay = 2f;

    [Header("Объект при смерти")]
    [SerializeField] private bool enableDeathObject = true;
    [SerializeField] private GameObject deathObject;

    public event System.Action OnDeath;

    private bool isDead = false;
    private float targetFill;      // целевое значение fillAmount
    private Coroutine smoothCoroutine;

    // -------------------------------------------------------
    private void Awake()
    {
        currentHealth = maxHealth;
        targetFill = 1f;

        if (deathObject != null)
            deathObject.SetActive(false);

        UpdateUI();
    }

    // -------------------------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateUI();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateUI();
    }

    // -------------------------------------------------------
    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} умер.");
        OnDeath?.Invoke();

        if (enableDeathObject && deathObject != null)
            deathObject.SetActive(true);

        if (enableSceneRestart)
            Invoke(nameof(RestartScene), restartDelay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // -------------------------------------------------------
    private void UpdateUI()
    {
        targetFill = currentHealth / maxHealth;

        // Текст обновляем сразу
        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(currentHealth)} / {maxHealth}";

        if (hpBarImage == null) return;

        if (enableSmooth)
        {
            // Запускаем / перезапускаем корутину плавного изменения
            if (smoothCoroutine != null)
                StopCoroutine(smoothCoroutine);
            smoothCoroutine = StartCoroutine(SmoothFill());
        }
        else
        {
            hpBarImage.fillAmount = targetFill;
        }
    }

    private IEnumerator SmoothFill()
    {
        while (!Mathf.Approximately(hpBarImage.fillAmount, targetFill))
        {
            hpBarImage.fillAmount = Mathf.MoveTowards(
                hpBarImage.fillAmount,
                targetFill,
                smoothSpeed * Time.deltaTime
            );
            yield return null;
        }
        hpBarImage.fillAmount = targetFill;
        smoothCoroutine = null;
    }

    // -------------------------------------------------------
    public float CurrentHealth => currentHealth;
    public float MaxHealth     => maxHealth;
    public bool  IsDead        => isDead;
}