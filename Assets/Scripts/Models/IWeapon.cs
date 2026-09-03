namespace Assets.Scripts.Models
{
    public interface IWeapon
    {
        bool IsAutomatic { get; set; }

        float Damage { get; set; }

        float Distance { get; set; }

        int DeviationAngle { get; set; }

        void Execute();
    }
}
