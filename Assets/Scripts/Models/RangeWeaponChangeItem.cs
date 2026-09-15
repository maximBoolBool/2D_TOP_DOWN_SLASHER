using Assets.Scripts.Constants;
using Assets.Scripts.Helpers;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class RangeWeaponChangeItem : BaseInteractiveItem
    {
        [SerializeField]
        private RangeWeapon _rangeWeaponPrefab;
        private SpriteRenderer _weaponSpriteRenderer;

        private void Awake()
        {
            _weaponSpriteRenderer = GetComponent<SpriteRenderer>();

            if (_rangeWeaponPrefab == null)
            {
                Debug.LogError("_rangeWeaponPrefab не назначен!", this);
                return;
            }

            var prefabSprite = _rangeWeaponPrefab.GetComponent<SpriteRenderer>();
            if (prefabSprite == null)
            {
                Debug.LogError($"У {_rangeWeaponPrefab.name} нет SpriteRenderer на корне!", this);
                return;
            }

            Debug.Log($"Найден спрайт: {prefabSprite.sprite?.name}, назначаю его на {gameObject.name}");
            _weaponSpriteRenderer.sprite = prefabSprite.sprite;
        }

        public override void Interact()
        {
            var player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
            var aimPart = player.transform.Find("AimShootUnitPart");         

            PlayerWeaponHelper.ChangeWeapon(aimPart.gameObject, _rangeWeaponPrefab);

            Destroy(gameObject);
        }
    }
}
