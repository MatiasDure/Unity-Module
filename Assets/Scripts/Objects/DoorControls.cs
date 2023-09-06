using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>DoorControls</c> is used to control 
    /// doors in the game
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class DoorControls : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private int keysNeededToOpen;

        public static event Action OnDoorOpen;

        private bool doorOpen = false;

        private void Awake()
        {
            //Gets the Animator component if one is not passed in the inspector
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //Checks whether there are enough keys to open the door
            if (!doorOpen && Inventory.Locker.KeysInLocker == keysNeededToOpen)
            {
                doorOpen = true;
                _animator.SetBool("isOpen", true);
                OnDoorOpen?.Invoke();
            }
        }
    }
}