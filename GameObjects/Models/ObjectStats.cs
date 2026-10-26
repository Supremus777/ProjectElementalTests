namespace GameObjects.Models
{
    public class ObjectStats
    {
        // ATRIBUTOS BASE:
        float moveSpeedBase = 5f;
        int maxHealthBase = 40;
        int attackBase = 10;
        int defenseBase = 0;
        float projectileSpeedBase = 20f;
        int elementalGaugeForce = 1;
        float fireRateBase = 1f;

        // REAÇÕES:
        //"Overload"
        float overloadMultiplierBase = 1.3f;
        float overloadRadiusBase = 1f;
        float overloadKnockbackForceBase = 2f;

        //"Vaporize"
        float vaporizeMultiplierBase = 0.3f;
        float vaporizeVulnerabilityTime = 5f;
        float vaporizeVulnerabilityDamageMultiplier = 0.3f;
        float vaporizeVulnerabilityMovespeedPenalty = 0.3f;
        float vaporizeRadiusBase = 1f;

        //"ElectroCharged"
        float electroChargedMultiplierBase = 1.2f;
        float electroChargedRadiusBase = 1f;
        int electroChargedMaxHitsBase = 3;

        // ATRIBUTOS BÔNUS:
        float moveSpeedBonus = 0f;
        int maxHealthBonus = 0;
        int attackBonus = 0;
        int defenseBonus = 0;
        int elementalGaugeBonus = 0;
        float projectileSpeedBonus = 0f;
        float fireRateBonus = 0f;

        // ATRIBUTOS MULTIPlICADORES:
        private float moveSpeedMultiplier = 1f;
        float maxHealthMultiplier = 1f;
        float attackMultiplier = 1f;
        float defenseMultiplier = 1f;
        float projectileSpeedMultiplier = 1f;
        float skillCooldownMultiplier = 1f;
        float fireRateMulitplier = 1f;

        // ATRIBUTOS PÚBLICOS:
        public float MoveSpeed => (moveSpeedBase + moveSpeedBonus) * moveSpeedMultiplier;
        public int MaxHealth => (int)Math.Ceiling((maxHealthBase + maxHealthBonus) * maxHealthMultiplier);
        public int Attack => (int)Math.Ceiling((attackBase + attackBonus) * attackMultiplier);
        public float FireRate => (fireRateBase + fireRateBonus) * fireRateMulitplier;
        public float ProjectileSpeed => (projectileSpeedBase + projectileSpeedBonus) * projectileSpeedMultiplier;

        public float SkillCooldownMultiplier => skillCooldownMultiplier;
        public int ElementalForceGauge => elementalGaugeForce + elementalGaugeBonus;

        public float OverloadDamageMultiplier => overloadMultiplierBase;
        public float VaporizeDamageMultiplier => vaporizeMultiplierBase;
        public float ElectroChargedMultiplier => electroChargedMultiplierBase;

        public float OverloadRadius => overloadRadiusBase;
        public int ElectrochargedMaxHits => electroChargedMaxHitsBase;
    }
}
