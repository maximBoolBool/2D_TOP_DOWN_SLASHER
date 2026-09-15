using Assets.Scripts.Constants;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class RangeWeaponChangeItem : BaseInteractiveItem
    {
        [SerializeField]
        private RangeWeapon _rangeWeaponPrefab;
        private SpriteRenderer _weponSpriteRenderer;

        private void Awake()
        {
            _weponSpriteRenderer = GetComponent<SpriteRenderer>();
            _weponSpriteRenderer.sprite = _rangeWeaponPrefab.gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        public override void Interact()
        {
            var player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
            var aimPart = player.transform.Find("AimShootUnitPart");         
            var newWeapon = Instantiate(_rangeWeaponPrefab, aimPart);
            newWeapon.transform.localPosition = Vector3.zero;
        }
    }
}
