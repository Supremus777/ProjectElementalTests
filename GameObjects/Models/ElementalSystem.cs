namespace GameObjects.Models
{
    public class ElementalSystem
    {
        private readonly float reactionCooldown;

        private float cooldownRemaining;
        private bool invulnerable;

        public Elements Aura { get; private set; } = Elements.None;
        public bool Invulnerable => invulnerable;

        public ElementalSystem(float reactionCooldown = 1f)
        {
            if (reactionCooldown < 0f)
                throw new ArgumentOutOfRangeException(nameof(reactionCooldown));

            this.reactionCooldown = reactionCooldown;
        }

        public void AdvanceTime(float deltaTime)
        {
            if (deltaTime < 0f)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));

            if (!invulnerable)
                return;

            cooldownRemaining -= deltaTime;

            if (cooldownRemaining <= 0f)
            {
                cooldownRemaining = 0f;
                invulnerable = false;
            }
        }

        public Reaction ApplyElement(DamageData damageData)
        {
            if (invulnerable || damageData.element == Elements.None)
                return null;

            if (Aura == Elements.None)
            {
                Aura = damageData.element;
                return null;
            }

            if (Aura == damageData.element)
                return null;

            Reaction result = TryReaction(Aura, damageData.element, damageData);

            Aura = Elements.None;

            cooldownRemaining = reactionCooldown;
            invulnerable = reactionCooldown > 0f;

            return result;
        }

        private Reaction TryReaction(Elements aura, Elements otherElement, DamageData damageData)
        {
            if (IsReaction(aura, otherElement, Elements.Fire, Elements.Electric))
            {
                return Overload(damageData);
            }

            if (IsReaction(aura, otherElement, Elements.Water, Elements.Electric))
            {
                return ElectroCharged(damageData);
            }

            if (IsReaction(aura, otherElement, Elements.Water, Elements.Fire))
            {
                return Vaporize(damageData);
            }

            return null;
        }

        private static bool IsReaction(Elements a, Elements b, Elements first, Elements second)
        {
            return (a == first && b == second) || (a == second && b == first);
        }

        private Reaction Overload(DamageData reactor)
        {
            return new Reaction
            {
                Type = ReactionType.Overload,
                Damage = CreateReactionDamage(
                    reactor,
                    Elements.Fire,
                    reactor.stats.Attack * reactor.stats.OverloadDamageMultiplier
                )
            };
        }

        private Reaction Vaporize(DamageData reactor)
        {
            return new Reaction
            {
                Type = ReactionType.Vaporize,
                Damage = CreateReactionDamage(
                    reactor,
                    Elements.Water,
                    reactor.stats.Attack * reactor.stats.VaporizeDamageMultiplier
                ),
                ApplyVulnerability = true,
                VulnerabilityTime = 5f
            };
        }

        private Reaction ElectroCharged(DamageData reactor)
        {
            return new Reaction
            {
                Type = ReactionType.ElectroCharged,
                Damage = CreateReactionDamage(
                    reactor,
                    Elements.Electric,
                    reactor.stats.Attack * reactor.stats.ElectroChargedMultiplier
                ),
                MaxHits = reactor.stats.ElectrochargedMaxHits
            };
        }

        private DamageData CreateReactionDamage(DamageData reactor, Elements element, float damage)
        {
            return new DamageData
            {
                element = element,
                damage = (int)Math.Ceiling(damage),
                stats = reactor.stats,
                isReaction = true
            };
        }
    }
}
