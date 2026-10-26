namespace GameObjects.Models
{
    public class Reaction
    {
        public ReactionType Type { get; set; }
        public DamageData Damage { get; set; }

        public bool ApplyVulnerability { get; set; }
        public float VulnerabilityTime { get; set; }

        public int MaxHits { get; set; }
    }

}
