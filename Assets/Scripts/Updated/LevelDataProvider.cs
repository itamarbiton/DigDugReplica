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
            public GridManager.GridConfiguration gridConfig;
            public float enemyPercent;
        }

        public static LevelData CurrentLevel { get; private set; }

        [SerializeField] private List<LevelData> levels;
        
        private static int _currentLevelIndex = 0;

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
            if (CurrentLevel == null)
            {
                CurrentLevel = levels.First();
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
            if (_currentLevelIndex < levels.Count - 1)
            {
                _currentLevelIndex++;
                CurrentLevel = levels[_currentLevelIndex];
            }
        }
    }
}