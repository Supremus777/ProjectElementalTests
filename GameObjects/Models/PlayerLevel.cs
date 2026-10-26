namespace GameObjects.Models
{
    public class PlayerLevel
    {
        private readonly int maxLevel;
        private readonly int minLevel;
        private readonly int expPerLevel;

        private int currentLevel;
        private int currentExp;

        public int CurrentLevel => currentLevel;
        public int CurrentExp => currentExp;

        public event Action LevelUp;

        public PlayerLevel(int maxLevel = 99, int minLevel = 1, int expPerLevel = 300)
        {
            if (minLevel < 1)
                throw new ArgumentOutOfRangeException(nameof(minLevel));

            if (maxLevel < minLevel)
                throw new ArgumentOutOfRangeException(nameof(maxLevel));

            if (expPerLevel <= 0)
                throw new ArgumentOutOfRangeException(nameof(expPerLevel));

            this.maxLevel = maxLevel;
            this.minLevel = minLevel;
            this.expPerLevel = expPerLevel;

            currentLevel = minLevel;
            currentExp = 0;
        }

        public void GiveExp(int exp)
        {
            if (exp <= 0 || currentLevel >= maxLevel)
                return;

            while (exp > 0 && currentLevel < maxLevel)
            {
                int expNeeded = expPerLevel - currentExp;
                int expToApply = Math.Min(exp, expNeeded);

                currentExp += expToApply;
                exp -= expToApply;

                if (currentExp >= expPerLevel)
                {
                    currentExp -= expPerLevel;
                    currentLevel++;

                    LevelUp?.Invoke();
                }
            }

            if (currentLevel >= maxLevel)
                currentExp = 0;
        }
    }
}