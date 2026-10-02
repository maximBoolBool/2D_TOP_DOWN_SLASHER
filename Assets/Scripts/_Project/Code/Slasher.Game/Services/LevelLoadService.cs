using System.Collections.Generic;
using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using UnityEngine;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    public interface ILevelLoadService
    {
        int LevelsCount { get; }
        int CurrentLevelIndex { get; }
        GameObject CurrentLevel { get; }

        GameObject LoadLevel(int levelIndex);
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
        public GameObject CurrentLevel { get; private set; }

        public LevelLoadService(
            DiContainer container,
            [Inject(Id = GameObjectInjectConstants.LEVEL_PREFAB_IDS)] List<GameObject> levelPrefabs,
            [Inject(Id = GameObjectInjectConstants.LEVEL_ROOT_ID)] Transform levelRoot)
        {
            _container = container;
            _levelPrefabs = levelPrefabs;
            _levelRoot = levelRoot;
        }

        public GameObject LoadLevel(int levelIndex)
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

            CurrentLevel = _container.InstantiatePrefab(levelPrefab, _levelRoot);
            CurrentLevel.name = levelPrefab.name;
            CurrentLevelIndex = levelIndex;

            return CurrentLevel;
        }

        public void UnloadCurrentLevel()
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.SetActive(false);
                Object.Destroy(CurrentLevel);
            }

            CurrentLevel = null;
            CurrentLevelIndex = NoLevelIndex;
        }
    }
}
