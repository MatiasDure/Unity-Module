using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>LoadScene</c> is used to load 
    /// scenes
    /// </summary>
    /// <remarks>
    /// Used mostly with buttons
    /// </remarks>
    public class LoadScene : MonoBehaviour
    {

        public static event Action OnNewSceneLoad;

        /// <summary>
        /// This method loads a scene
        /// </summary>
        /// <param name="sceneName">
        /// The name of a scene to load
        /// </param>
        public void LoadScreen(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            if(GameManager.Manager != null) GameManager.Manager.ResetStats();
        }

        /// <summary>
        /// This method fires an event to let
        /// others know that a new screen is being loaded
        /// </summary>

        public void NewScreen() => OnNewSceneLoad?.Invoke();

        /// <summary>
        /// This method quits the application
        /// </summary>
        public void ExitGame() => Application.Quit();
    }
}
