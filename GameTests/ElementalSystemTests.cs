using GameObjects.Models;

namespace GameTests
{
    [TestFixture]
    public class ElementalSystemTests
    {
        [Test]
        public void ApplyElement_WhenAuraIsNone_SetsAura()
        {
            // Arrange
            var system = new ElementalSystem();
            var damage = CreateDamage(Elements.Fire);

            // Act
            var result = system.ApplyElement(damage);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(system.Aura, Is.EqualTo(Elements.Fire));
                Assert.That(result, Is.Null);
            }
        }

        // Cria 3 Casos de testes
        [TestCase(Elements.Fire, Elements.Electric, ReactionType.Overload)]
        [TestCase(Elements.Water, Elements.Electric, ReactionType.ElectroCharged)]
        [TestCase(Elements.Water, Elements.Fire, ReactionType.Vaporize)]
        public void ApplyElement_WhenValidReaction_ReturnsReaction(Elements first, Elements second, ReactionType expected)
        {
            var system = new ElementalSystem();

            system.ApplyElement(CreateDamage(first));

            var result = system.ApplyElement(CreateDamage(second));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Type, Is.EqualTo(expected));
                Assert.That(system.Aura, Is.EqualTo(Elements.None));
            }
        }

        [Test]
        public void ApplyElement_Overload_ReturnsCorrectDamage()
        {
            var system = new ElementalSystem();

            var fire = CreateDamage(Elements.Fire);
            var electric = CreateDamage(Elements.Electric);

            system.ApplyElement(fire);
            var result = system.ApplyElement(electric);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Type, Is.EqualTo(ReactionType.Overload));
                Assert.That(result.Damage.damage, Is.EqualTo(13));
                Assert.That(result.Damage.element, Is.EqualTo(Elements.Fire));
                Assert.That(result.Damage.isReaction, Is.True);
            }
        }

        [Test]
        public void ApplyRepeatedElement_DoesNotReact()
        {
            var system = new ElementalSystem();

            var fire1 = CreateDamage(Elements.Fire);
            var fire2 = CreateDamage(Elements.Fire);

            system.ApplyElement(fire1);
            var result = system.ApplyElement(fire2);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ApplyElement_DuringCooldown_IgnoresElement()
        {
            var system = new ElementalSystem(1f);

            system.ApplyElement(CreateDamage(Elements.Fire));
            system.ApplyElement(CreateDamage(Elements.Electric));

            system.ApplyElement(CreateDamage(Elements.Water));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(system.Aura, Is.EqualTo(Elements.None));
                Assert.That(system.Invulnerable, Is.True);
            }
        }

        [Test]
        public void AdvanceTime_WhenCooldownExpires_AllowsNewElement()
        {
            var system = new ElementalSystem(1f);

            system.ApplyElement(CreateDamage(Elements.Fire));
            system.ApplyElement(CreateDamage(Elements.Electric));

            system.AdvanceTime(1f);
            system.ApplyElement(CreateDamage(Elements.Water));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(system.Aura, Is.EqualTo(Elements.Water));
                Assert.That(system.Invulnerable, Is.False);
            }
        }

        private static DamageData CreateDamage(Elements element)
        {
            return new DamageData
            {
                element = element,
                stats = CreateDefaultStatus(),
            };
        }

        private static ObjectStats CreateDefaultStatus()
        {
            return new ObjectStats();
        }
    }
}
