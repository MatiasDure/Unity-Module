using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>GameManager</c> is used to preserve 
    /// information that one wants to carry throughout scenes.
    /// Also used to load scenes on game over.
    /// </summary>
    public class GameManager : EventListener
    {

        [SerializeField] private int _lives;
        [SerializeField] private int _moneyToWin;

        public int MoneyToWin { get => _moneyToWin; }
        public int Lives { get => _lives; }

        static GameManager _manager = null;
        public static GameManager Manager { get => _manager; }

        private int startingLives;

        public static event Action OnChangeScene;

        private void Awake()
        {
            if (Manager == null)
            {
                //game manager is assinged once
                _manager = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //There can only be one game manager, so we destroy the others
                Destroy(gameObject);
            }

            AudioManager.Init();
        }

        protected override void Start()
        {
            base.Start();
            startingLives = Lives;
        }

        private void Update()
        {
            if (Hacks.Hak.Win()) PlayerVictory();
            else if (Hacks.Hak.Lose()) PlayerDefeat();
        }

        /// <summary>
        /// This method checks whether the player won or died when an 
        /// event is triggered
        /// </summary>
        private void DecidedWinLoseOutcome(System.Object sender, bool won)
        {
            if (won) PlayerVictory();
            else OnPlayerDeath();
        }

        /// <summary>
        /// This method reacts accordingly when the played has died
        /// </summary>
        private void OnPlayerDeath()
        {
            _lives--;

            if (_lives > 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else PlayerDefeat();
            
        }

        /// <summary>
        /// This method resets values
        /// </summary>
        /// <remarks>
        /// Currently only lives, but can be expanded
        /// </remarks>
        public void ResetStats() => _lives = startingLives;

        /// <summary>
        /// This method loads the victory screen
        /// </summary>
        private void PlayerVictory()
        {
            OnChangeScene?.Invoke();
            LoadScreen("VictoryScreen");
        }

        private void PlayerDefeat()
        {
            ResetStats();
            LoadScreen("DefeatScreen");
            OnChangeScene?.Invoke();
        }

        /// <summary>
        /// This method loads the scene passed 
        /// </summary>
        /// <param name="sceneToLoad">
        /// The name of the scene to load
        /// </param>
        void LoadScreen(string sceneToLoad) => SceneManager.LoadScene(sceneToLoad);

        protected override void SubscribeToEvent()
        {
            PlayerReady.OnReadyToChangeScreen += DecidedWinLoseOutcome;
            PlayerMoney.OnPlayerNoMoney += OnPlayerDeath;
        }

        protected override void UnsubscribeFromEvent()
        {
            PlayerReady.OnReadyToChangeScreen -= DecidedWinLoseOutcome;
            PlayerMoney.OnPlayerNoMoney -= OnPlayerDeath;
        }

    }
}