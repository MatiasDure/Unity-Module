using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>PurchaseButtonBehavior</c> is used for buttons 
    /// that are going to be used to buy objects
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PurchaseButtonBehavior : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private int costOfObject;

        public static event Action OnHousePurchased;
        public static event Action OnCollectEnoughMoney;

        private void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        void Update()
        {
            bool buttonEnabled = _animator.GetBool("isEnabled");
            bool collectedEnoughMoney = CollectedEnoughMoney();

            //Enables the button to be pressed when the player collected the necessary amount of money
            if (!buttonEnabled && collectedEnoughMoney)
            {
                //sets enabled animation
                _animator.SetBool("isEnabled", true);
                OnCollectEnoughMoney?.Invoke();
            }
            else if (buttonEnabled && !_animator.GetBool("isPressed") && !collectedEnoughMoney) _animator.SetBool("isEnabled", false);

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_animator.GetBool("isPressed") || 
                collision.gameObject.name != "Player" || 
                !_animator.GetBool("isEnabled")) return;

            //Sets pressed animation
            _animator.SetBool("isPressed", true);

            //Triggers events
            OnHousePurchased?.Invoke();
            PlayerMoney.Money.GiveMoney(this, costOfObject);
        }

        /// <summary>
        /// This method checks whether the player
        /// has collected enough money to buy a
        /// certain object
        /// </summary>
        bool CollectedEnoughMoney() => PlayerMoney.Money.AmountMoney >= costOfObject;
    }
}