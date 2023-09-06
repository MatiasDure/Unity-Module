using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>PlayerMoney</c> is used as a wallet 
    /// to keep track of the player's money
    /// </summary>
    public class PlayerMoney : MonoBehaviour
    {
        [SerializeField] private int startingMoney;
        
        //Triggers events
        public static event Action OnPlayerNoMoney;
        public static event Action OnMoneyCollected;
        public static event Action OnMoneyLost;
        
        //Instance of the class to get info from it
        private static PlayerMoney _money;
        public static PlayerMoney Money { get => _money; }

        //The amount of money the player has at any given time
        private int _amountMoney;
        public int AmountMoney { get => _amountMoney; }

        private int hakzMoney = 10;


        private void Awake()
        {
            if (_money == null) _money = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            RefreshWallet();
        }

        private void Update()
        {
            if (Hacks.Hak.Money()) CollectMoney(null, hakzMoney);
        }

        /// <summary>
        /// This method add money to the wallet
        /// </summary>
        public void CollectMoney(System.Object sender, int amountCollected)
        {
            _amountMoney += amountCollected;
            OnMoneyCollected?.Invoke();
        }

        /// <summary>
        /// This method removes money from the wallet
        /// </summary>
        public void GiveMoney(System.Object sender, int amountTaken)
        {
            _amountMoney -= amountTaken;
            OnMoneyLost?.Invoke();

            if (AmountMoney < 0)
            {
                OnPlayerNoMoney?.Invoke();
                RefreshWallet();
            }
        }

        /// <summary>
        /// This method resets the amount of money
        /// with the first introduced amount
        /// </summary>
        private void RefreshWallet()
        {
            _amountMoney = startingMoney;
        }

    }
}
