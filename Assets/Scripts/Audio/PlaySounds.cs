using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    /// <summary>
    /// Class <c>PlaySounds</c> is used to 
    /// switch between and play most sound effects
    /// </summary>
    public class PlaySounds : EventListener
    {
        AudioManager.Sound currentSound = AudioManager.Sound.NoSound;

        protected override void SubscribeToEvent()
        {
            PlayerMovement.OnStateChange += PlaySoundOnMovementChange;
            PlayerCollisionDetection.OnPlayerHit += PlayGotHit;
            PlayerMoney.OnMoneyCollected += PlayGainMoney;
            PlayerCollisionDetection.OnNpcInteraction += PlayerExitNPC;
            PlayerCollisionDetection.OnPlayerFall += PlayFalling;
            PlayerCollisionDetection.OnPlayerWins += PlayReachedFlag;
            NpcInteraction.OnNewMessage += PlayNewMessage;
            DoorControls.OnDoorOpen += PlayDoorOpen;
            PurchaseButtonBehavior.OnHousePurchased += PlayPurchased;
            PlayerCollisionDetection.OnKeyCollected += PlayGotKey;
        }
        protected override void UnsubscribeFromEvent()
        {
            PlayerMovement.OnStateChange -= PlaySoundOnMovementChange;
            PlayerCollisionDetection.OnPlayerHit -= PlayGotHit;
            PlayerMoney.OnMoneyCollected -= PlayGainMoney;
            PlayerCollisionDetection.OnNpcInteraction -= PlayerExitNPC;
            PlayerCollisionDetection.OnPlayerFall -= PlayFalling;
            PlayerCollisionDetection.OnPlayerWins -= PlayReachedFlag;
            NpcInteraction.OnNewMessage -= PlayNewMessage;
            DoorControls.OnDoorOpen -= PlayDoorOpen;
            PurchaseButtonBehavior.OnHousePurchased -= PlayPurchased;
            PlayerCollisionDetection.OnKeyCollected -= PlayGotKey;
        }

        /// <summary>
        /// This method plays a sound after the player changes its movement state
        /// </summary>
        /// <param name="sender"> object that triggered the event</param>
        /// <param name="sound"> Sound to play</param>
        void PlaySoundOnMovementChange(System.Object sender, AudioManager.Sound sound)
        {
            switch (sound)
            {
                default:
                    AudioManager.PlaySound(sound);
                    break;
                case AudioManager.Sound.PlayerStopped:
                    AudioManager.StopSound(AudioManager.Sound.PlayerMoving);
                    goto default;
            }
        }

        void PlayGainMoney() => currentSound = AudioManager.Sound.MoneyPicked;
        void PlayGotHit() => currentSound = AudioManager.Sound.PlayerHit;
        void PlayEnterNPC() => currentSound = AudioManager.Sound.EnterNpc;
        void PlayLeaveNPC() => currentSound = AudioManager.Sound.LeaveNpc;
        void PlayFalling() => currentSound = AudioManager.Sound.PlayerFalling;
        void PlayReachedFlag() => currentSound = AudioManager.Sound.FlagGrabbed;
        void PlayNewMessage() => currentSound = AudioManager.Sound.NewNpcMessage;
        void PlayDoorOpen() => currentSound = AudioManager.Sound.DoorOpened;
        void PlayPurchased() => currentSound = AudioManager.Sound.MoneyPicked;
        void PlayGotKey() => currentSound = AudioManager.Sound.KeyPicked;
        void PlayerExitNPC(System.Object sender, bool exit)
        {
            if (exit) PlayLeaveNPC();
            else PlayEnterNPC();
        }

        //sounds that need to be implemented

        private void Update()
        {
            //Skips the iteration until the current sound is changed to a playable sound
            if (currentSound == AudioManager.Sound.NoSound) return;

            //Plays sound
            AudioManager.PlaySound(currentSound);

            //Resets current sound
            currentSound = AudioManager.Sound.NoSound;
        }

    }
}
