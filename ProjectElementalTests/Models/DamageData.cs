using ProjectElementalTests.Models;

public struct DamageData
{
    public int damage;
    public Elements element;
    public ObjectStats stats;
    public bool applyVulnerability;
    public float vulnerabilityTime;
    public bool isReaction;
}