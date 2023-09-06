using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>Inventory</c> is used to keep track
    /// of the player's objects
    /// </summary>
    /// <remarks>
    /// Currently only for keys, but can expanded
    /// </remarks>
    public class Inventory : EventListener
    {
        private static Inventory _locker;
        public static Inventory Locker { get => _locker; }

        private int _keysInLocker = 0;
        public int KeysInLocker { get => _keysInLocker; }

        private void Awake()
        {
            if (_locker == null) _locker = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// This meethods adds keys to the locker
        /// </summary>
        private void AddKey() => _keysInLocker++;

        protected override void SubscribeToEvent() => PlayerCollisionDetection.OnKeyCollected += AddKey;

        protected override void UnsubscribeFromEvent() => PlayerCollisionDetection.OnKeyCollected -= AddKey;

    }
}
