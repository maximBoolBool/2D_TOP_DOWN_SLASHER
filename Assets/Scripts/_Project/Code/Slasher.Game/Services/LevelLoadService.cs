using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Levels;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    public interface ILevelLoadService
    {
        int LevelsCount { get; }
        int CurrentLevelIndex { get; }
        LevelView CurrentLevel { get; }

        LevelView LoadLevel(string levelName);
        LevelView LoadLevel(int levelIndex);
        void UnloadCurrentLevel();
    }

    public class LevelLoadService : ILevelLoadService
    {
        private const int NoLevelIndex = -1;

        private readonly DiContainer _container;
        private readonly List<GameObject> _levelPrefabs;
        private readonly Transform _levelRoot;

        public int LevelsCount => _levelPrefabs.Count;
        public int CurrentLevelIndex { get; private set; } = NoLevelIndex;
        public LevelView CurrentLevel { get; private set; }

        public LevelLoadService(
            DiContainer container,
            [Inject(Id = GameObjectInjectConstants.LEVEL_PREFAB_IDS)] List<GameObject> levelPrefabs,
            [Inject(Id = GameObjectInjectConstants.LEVEL_ROOT_ID)] Transform levelRoot)
        {
            _container = container;
            _levelPrefabs = levelPrefabs;
            _levelRoot = levelRoot;
        }

        /// <summary>Загружает уровень по имени префаба (например "Level1").</summary>
        public LevelView LoadLevel(string levelName)
        {
            int index = _levelPrefabs.FindIndex(prefab => prefab != null && prefab.name == levelName);
            if (index < 0)
            {
                Debug.LogError($"{nameof(LevelLoadService)}: level '{levelName}' not found in GameInstaller level prefabs");
                return null;
            }

            return LoadLevel(index);
        }

        public LevelView LoadLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= _levelPrefabs.Count)
            {
                Debug.LogError($"{nameof(LevelLoadService)}: level index {levelIndex} is out of range (0..{_levelPrefabs.Count - 1})");
                return null;
            }

            var levelPrefab = _levelPrefabs[levelIndex];
            if (levelPrefab == null)
            {
                Debug.LogError($"{nameof(LevelLoadService)}: level prefab with index {levelIndex} is not assigned in GameInstaller");
                return null;
            }

            UnloadCurrentLevel();

            // InstantiatePrefab через контейнер — чтобы объекты внутри уровня получили [Inject]
            var levelObject = _container.InstantiatePrefab(levelPrefab, _levelRoot);
            levelObject.name = levelPrefab.name;

            CurrentLevel = levelObject.GetComponent<LevelView>();
            if (CurrentLevel == null)
            {
                Debug.LogError($"{nameof(LevelLoadService)}: level prefab '{levelPrefab.name}' has no {nameof(LevelView)} on its root", levelPrefab);
                // чтобы уровень всё равно корректно выгружался
                CurrentLevel = levelObject.AddComponent<LevelView>();
            }

            CurrentLevelIndex = levelIndex;
            return CurrentLevel;
        }

        public void UnloadCurrentLevel()
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.gameObject.SetActive(false);
                Object.Destroy(CurrentLevel.gameObject);
            }

            CurrentLevel = null;
            CurrentLevelIndex = NoLevelIndex;
        }
    }
}
