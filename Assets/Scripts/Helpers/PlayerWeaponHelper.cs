using Assets.Scripts.PlayerControllers;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class PlayerWeaponHelper
    {
        public static void ChangeWeapon(GameObject aimPart, RangeWeapon _rangeWeaponPrefab)
        {
            var currentWeapon = aimPart.GetComponentInChildren<RangeWeapon>();
            currentWeapon.gameObject.SetActive(false);
            GameObject.Destroy(currentWeapon.gameObject);

            var newWeapon = GameObject.Instantiate(_rangeWeaponPrefab, aimPart.transform);
            newWeapon.transform.localPosition = Vector3.zero;

            aimPart.GetComponent<PlayerShootController>().RefreshWeapons();
            aimPart.GetComponent<PlayerWeaponAimController>().RefreshSprite();
        }
    }
}
