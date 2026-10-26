using GameObjects.Models;

namespace GameTests
{
    [TestFixture]
    public class PlayerLevelTests
    {
        private PlayerLevel playerLevel;

        [SetUp]
        public void SetUp()
        {
            // Level 1, máximo 99, 300 EXP por nível
            playerLevel = new PlayerLevel(99, 1, 300);
        }

        [Test]
        public void Constructor_InitializesAtMinimumLevel()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(1));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [Test]
        public void GiveExp_WhenInsufficient_DoesNotLevelUp()
        {
            playerLevel.GiveExp(100);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(1));
                Assert.That(playerLevel.CurrentExp, Is.EqualTo(100));
            }
        }

        [Test]
        public void GiveExp_WhenEnough_LevelsUp()
        {
            playerLevel.GiveExp(300);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(2));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [Test]
        public void GiveExp_WhenExcessExp_PreservesRemainder()
        {
            playerLevel.GiveExp(350);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(2));
                Assert.That(playerLevel.CurrentExp, Is.EqualTo(50));
            }
        }

        [Test]
        public void GiveExp_WhenEnoughForMultipleLevels_LevelsUpMultipleTimes()
        {
            playerLevel.GiveExp(900);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(4));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [Test]
        public void GiveExp_WhenReachingMaxLevel_DoesNotExceedLimit()
        {
            playerLevel = new PlayerLevel(maxLevel: 3, minLevel: 1, expPerLevel: 100);

            playerLevel.GiveExp(500);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(3));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void GiveExp_WhenExpIsNotPositive_DoesNothing(int exp)
        {
            playerLevel.GiveExp(exp);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(1));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [Test]
        public void GiveExp_WhenLevelingUp_InvokesEventOncePerLevel()
        {
            int eventCount = 0;

            playerLevel.LevelUp += () => eventCount++;

            playerLevel.GiveExp(900);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(4));
                Assert.That(eventCount, Is.EqualTo(3));
            }
        }

        [Test]
        public void GiveExp_WhenAlreadyAtMaxLevel_DoesNothing()
        {
            playerLevel = new PlayerLevel(maxLevel: 2, minLevel: 1, expPerLevel: 100);

            playerLevel.GiveExp(100);

            playerLevel.GiveExp(500);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(playerLevel.CurrentLevel, Is.EqualTo(2));
                Assert.That(playerLevel.CurrentExp, Is.Zero);
            }
        }

        [Test]
        public void Constructor_WhenMaxLevelIsBelowMinLevel_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayerLevel(1, 2, 100));
        }

        [Test]
        public void Constructor_WhenExpPerLevelIsZero_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayerLevel(99, 1, 0));
        }
    }
}
