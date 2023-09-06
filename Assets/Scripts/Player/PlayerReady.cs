using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>PlayerReady</c> is used to check 
    /// whether certain events can be trigger
    /// </summary>
    /// <example>
    /// Winning or losing can only be triggered after
    /// finish playing a sound effect
    /// </example>
    public class PlayerReady : EventListener
    {
        [SerializeField] private AudioManager.Sound soundToCheck;
        public static event EventHandler<bool> OnReadyToChangeScreen;

        private bool startCheck = false;
        protected override void SubscribeToEvent()
        {
            PlayerCollisionDetection.OnPlayerFall += CheckSound;
            PlayerCollisionDetection.OnPlayerWins += CheckSound;
        }

        protected override void UnsubscribeFromEvent()
        {
            PlayerCollisionDetection.OnPlayerFall -= CheckSound;
            PlayerCollisionDetection.OnPlayerWins -= CheckSound;
        }

        /// <summary>
        /// Checks if a sound has finished to trigger an event
        /// </summary>
        private void CheckSound()
        {
            if (!startCheck && AudioManager.IsPlaying(soundToCheck)) startCheck = true;
            else if(startCheck)
            {
                Debug.Log("Started checking");
                if (!AudioManager.IsPlaying(soundToCheck))
                {
                    bool won = soundToCheck == AudioManager.Sound.FlagGrabbed;
                    OnReadyToChangeScreen?.Invoke(this,won);
                }
            }
        }

        /// <summary>
        /// Checks if an animation has finished to trigger an event
        /// </summary>
        ///<remarks>
        /// Currently empty, but can be implemented similarly to the 
        /// method above
        /// </remarks>
        private void CheckAnimation() { }

    }
}
