namespace Assets.Scripts._Project.Code.Slasher.Game.EventBusMessages
{
    public readonly struct HealthChangeMessage
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public HealthChangeMessage(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}
