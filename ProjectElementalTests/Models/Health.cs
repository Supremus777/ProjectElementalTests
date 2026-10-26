using System;

public class Health
{
    private bool isDead;
    private int healthCurrent;
    private int healthMax;

    public int HealthCurrent => healthCurrent;
    public int HealthMax => healthMax;
    public bool IsAlive => !isDead;

    public event Action<int, int> OnHealthChanged;
    public event Action<DamageData> OnDeath;

    public Health(int maxHealth)
    {
        healthMax = Math.Max(1, maxHealth);
        healthCurrent = healthMax;
    }

    public void TakeDamage(DamageData damage)
    {
        if (isDead || damage.Damage <= 0)
            return;

        healthCurrent = (int)Math.Max(0L, (long)healthCurrent - damage.Damage);

        OnHealthChanged?.Invoke(healthCurrent, healthMax);

        if (healthCurrent <= 0)
        {
            healthCurrent = 0;
            isDead = true;

            OnDeath?.Invoke(damage);
        }
    }

    public void Heal(int healAmount)
    {
        if (healAmount <= 0 || isDead || healthCurrent >= healthMax)
            return;

        healthCurrent = (int)Math.Min((long)healthCurrent + healAmount, healthMax);

        OnHealthChanged?.Invoke(healthCurrent, healthMax);
    }

    public void SetMaxHealth(int maxHealth, bool healback)
    {
        healthMax = Math.Max(1, maxHealth);

        if (healback)
            healthCurrent = healthMax;
        else
            healthCurrent = Math.Min(healthCurrent, healthMax);

        OnHealthChanged?.Invoke(healthCurrent, healthMax);
    }
}
