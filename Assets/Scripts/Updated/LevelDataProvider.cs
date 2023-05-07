using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Updated
{
    public class LevelDataProvider : MonoBehaviour
    {
        [System.Serializable]
        public class LevelData
        {
            public int width;
            public int height;
            public List<GridManager.SpecialPrefab> specialPrefabs;
            public List<GameObject> normalPrefabs;
            public float enemyPercent;
        }

        public static LevelData currentLevel { get; private set; }

        [SerializeField] private List<LevelData> levels;
        
        private static int currentLevelIndex = 0;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Awake()
        {
            if (currentLevel == null)
            {
                currentLevel = levels.First();
            }
        }

        private void SubscribeEvents()
        {
            WinConditionController.PlayerDidWin += OnPlayerDidWin;
            Grid3DPlayerController.PlayerDidDie += OnPlayerDidDie;
        }

        private void UnsubscribeEvents()
        {
            WinConditionController.PlayerDidWin -= OnPlayerDidWin;
            Grid3DPlayerController.PlayerDidDie -= OnPlayerDidDie;
        }

        private void OnPlayerDidDie()
        {
            // TODO: maybe reset levels?
        }

        private void OnPlayerDidWin()
        {
            if (currentLevelIndex < levels.Count - 1)
            {
                currentLevelIndex++;
                currentLevel = levels[currentLevelIndex];
            }
        }
    }
}