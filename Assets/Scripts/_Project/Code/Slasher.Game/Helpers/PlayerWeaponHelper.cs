using Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers;
using Assets.Scripts._Project.Code.Slasher.Game.Weapons;
using UnityEngine;

namespace Assets.Scripts._Project.Code.Slasher.Game.Helpers
{
    public static class PlayerWeaponHelper
    {
        public static void ChangeWeapon(
            GameObject aimPart,
            RangeWeapon rangeWeaponPrefab,
            PlayerShootController shootController,
            PlayerWeaponAimController aimController)
        {
            var currentWeapon = aimPart.GetComponentInChildren<RangeWeapon>();
            currentWeapon.gameObject.SetActive(false);
            GameObject.Destroy(currentWeapon.gameObject);

            var newWeapon = GameObject.Instantiate(rangeWeaponPrefab, aimPart.transform);
            newWeapon.transform.localPosition = Vector3.zero;

            shootController.RefreshWeapons();
            aimController.RefreshSprite();
        }
    }
}
