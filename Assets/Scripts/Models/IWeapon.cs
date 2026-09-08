namespace Assets.Scripts.Models
{
    public interface IWeapon
    {
        float Damage { get; set; }

        float Distance { get; set; }

        void Execute();
    }

    public interface IRangeWeapon : IWeapon
    {
        bool IsAutomatic { get; set; }

        float Distance { get; set; }

        float? RateOfFire { get; set; }

        int? MagazineRounds { get; set; }

        int DeviationAngle { get; set; }
    }
}
