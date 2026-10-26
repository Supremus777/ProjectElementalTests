using GameObjects.Models;

namespace GameTests
{
    [TestFixture]
    public class HealthTests
    {
        private Health health;

        // SetUp é chamado antes de cada teste
        [SetUp]
        public void SetUp()
        {
            health = new Health(100);
        }

        [Test]
        public void Constructor_SetsCurrentAndMaxHealth()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.EqualTo(100));
                Assert.That(health.HealthMax, Is.EqualTo(100));
                Assert.That(health.IsAlive, Is.True);
            }
        }

        [Test]
        public void TakeDamage_WithValidDamage_ReducesHealth()
        {
            var damage = CreateDamage(30);

            health.TakeDamage(damage);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.EqualTo(70));
                Assert.That(health.IsAlive, Is.True);
            }
        }

        [Test]
        public void TakeDamage_WhenLethal_SetsHealthToZeroAndDies()
        {
            var damage = CreateDamage(100);

            health.TakeDamage(damage);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.Zero);
                Assert.That(health.IsAlive, Is.False);
            }
        }

        [Test]
        public void TakeDamage_WhenExcessive_HealthNeverGoesNegative()
        {
            var damage = CreateDamage(500);

            health.TakeDamage(damage);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.Zero);
                Assert.That(health.IsAlive, Is.False);
            }
        }

        [Test]
        public void TakeDamage_AfterDeath_DoesNothing()
        {
            health.TakeDamage(CreateDamage(100));

            health.TakeDamage(CreateDamage(20));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.Zero);
                Assert.That(health.IsAlive, Is.False);
            }
        }

        [Test]
        public void Heal_WithValidAmount_IncreasesHealth()
        {
            health.TakeDamage(CreateDamage(40));

            health.Heal(20);

            Assert.That(health.HealthCurrent, Is.EqualTo(80));
        }

        [Test]
        public void Heal_AboveMaxHealth_ClampsToMax()
        {
            health.TakeDamage(CreateDamage(20));

            health.Heal(500);

            Assert.That(health.HealthCurrent, Is.EqualTo(100));
        }

        [Test]
        public void Heal_AfterDeath_DoesNothing()
        {
            health.TakeDamage(CreateDamage(100));

            health.Heal(50);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.Zero);
                Assert.That(health.IsAlive, Is.False);
            }
        }

        [Test]
        public void SetMaxHealth_LowerMax_ClampsCurrentHealth()
        {
            health.TakeDamage(CreateDamage(20));

            health.SetMaxHealth(50, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthMax, Is.EqualTo(50));
                Assert.That(health.HealthCurrent, Is.EqualTo(50));
            }
        }

        [Test]
        public void SetMaxHealth_WithHealback_RestoresHealth()
        {
            health.TakeDamage(CreateDamage(60));

            health.SetMaxHealth(150, true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthMax, Is.EqualTo(150));
                Assert.That(health.HealthCurrent, Is.EqualTo(150));
            }
        }

        [Test]
        public void SetMaxHealth_WithoutHealback_DoesNotRestoresHealth()
        {
            health.TakeDamage(CreateDamage(60));

            health.SetMaxHealth(150, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthMax, Is.EqualTo(150));
                Assert.That(health.HealthCurrent, Is.Not.EqualTo(150));
            }
        }

        [Test]
        public void TakeDamage_InvokesHealthChanged()
        {
            int receivedCurrent = -1;
            int receivedMax = -1;

            health.OnHealthChanged += (current, max) =>
            {
                receivedCurrent = current;
                receivedMax = max;
            };

            health.TakeDamage(CreateDamage(30));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedCurrent, Is.EqualTo(70));
                Assert.That(receivedMax, Is.EqualTo(100));
            }
        }

        [Test]
        public void TakeDamage_WhenLethal_InvokesDeathEvent()
        {
            bool deathEventCalled = false;

            health.OnDeath += damage =>
            {
                deathEventCalled = true;
            };

            health.TakeDamage(CreateDamage(100));

            Assert.That(deathEventCalled, Is.True);
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void TakeDamage_WhenDamageIsNotPositive_DoesNothing(int amount)
        {
            // Arrange
            int eventCount = 0;

            health.OnHealthChanged += (_, _) => eventCount++;

            // Act
            health.TakeDamage(CreateDamage(amount));

            // Asssert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(health.HealthCurrent, Is.EqualTo(100));
                Assert.That(health.IsAlive, Is.True);
                Assert.That(eventCount, Is.Zero);
            }
        }

        private static DamageData CreateDamage(int amount)
        {
            return new DamageData
            {
                damage = amount
            };
        }
    }
}