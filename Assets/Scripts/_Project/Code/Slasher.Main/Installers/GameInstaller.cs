using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Main.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _weaponLoadGO;

        public override void InstallBindings()
        {
            Container.Bind<GameObject>()
                .WithId(GameObjectInjectConstants.WEAPON_LOAD_GO)
                .FromInstance(_weaponLoadGO)
                .AsSingle();
        }
    }
}
