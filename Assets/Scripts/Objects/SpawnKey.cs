using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>SpawnKey</c> is used to spawn keys around
    /// the designated area
    /// </summary>
    /// <remarks>
    /// Inherits from class <c>SpawnObjects</c>
    /// </remarks>
    public class SpawnKey : SpawnObjects
    {
        private bool timeToSpawn = false;

        protected override void Update()
        {
            if (NotAbleToSpawn()) return;
            Spawn();
        }

        /// <summary>
        /// This method is used to allow the keys to start spawning
        /// </summary>
        private void AllowSpawn() => timeToSpawn = true;

        /// <summary>
        /// This method checks whether keys can spawn or not
        /// </summary>
        protected override bool NotAbleToSpawn() => !timeToSpawn || base.NotAbleToSpawn();

        protected override void SubscribeToEvent()
        {
            base.SubscribeToEvent();
            PurchaseButtonBehavior.OnHousePurchased += AllowSpawn;
        }

        protected override void UnsubscribeFromEvent()
        {
            base.UnsubscribeFromEvent();
            PurchaseButtonBehavior.OnHousePurchased -= AllowSpawn;
        }
    }
}
