using UnityEngine;

/// <summary>
/// Зона урона — объект с Collider (IsTrigger = true).
/// При входе/пребывании в триггере наносит урон любому объекту
/// с компонентом HealthSystem.
///
/// Настройка в редакторе:
///   • Добавьте Collider (BoxCollider / SphereCollider и т.д.) → Is Trigger = true
///   • Укажите damageAmount, damageInterval, damageTag
/// </summary>
public class DamageZone : MonoBehaviour
{
    [Header("Настройки урона")]
    [SerializeField] private float damageAmount   = 10f;   // урон за тик
    [SerializeField] private float damageInterval = 1f;    // секунды между тиками
    [SerializeField] private string damageTag     = "Player"; // тег цели ("" = все)

    [Header("Одиночный урон при входе")]
    [SerializeField] private bool damageOnEnterOnly = false; // true = только при входе

    [Header("Визуал (необязательно)")]
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f);

    // Внутренний таймер для каждой цели в зоне
    // (простая реализация — один таймер на всю зону)
    private float timer = 0f;

    // -------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTarget(other)) return;

        // Нанести урон сразу при входе
        ApplyDamage(other);

        // Сбросить таймер
        timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (damageOnEnterOnly) return;
        if (!IsValidTarget(other)) return;

        timer += Time.deltaTime;
        if (timer >= damageInterval)
        {
            timer = 0f;
            ApplyDamage(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Сбросить таймер когда цель покидает зону
        timer = 0f;
    }

    // -------------------------------------------------------
    private void ApplyDamage(Collider other)
    {
        HealthSystem health = other.GetComponent<HealthSystem>();
        if (health == null)
            health = other.GetComponentInParent<HealthSystem>(); // для составных персонажей

        if (health != null)
        {
            health.TakeDamage(damageAmount);
            Debug.Log($"[DamageZone] {other.name} получил {damageAmount} урона. HP: {health.CurrentHealth}/{health.MaxHealth}");
        }
    }

    private bool IsValidTarget(Collider other)
    {
        if (string.IsNullOrEmpty(damageTag)) return true;
        return other.CompareTag(damageTag);
    }

    // -------------------------------------------------------
    // Отображение зоны в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Collider col = GetComponent<Collider>();

        if (col is BoxCollider box)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(box.center, box.size);
        }
        else if (col is SphereCollider sphere)
        {
            Gizmos.DrawSphere(transform.position + sphere.center,
                              sphere.radius * Mathf.Max(transform.lossyScale.x,
                                                        transform.lossyScale.y,
                                                        transform.lossyScale.z));
        }
    }
}
