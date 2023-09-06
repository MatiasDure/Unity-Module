using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>PlayerCollisionDetection</c> is used to retrieve 
    /// information about the objects colliding with the player
    /// </summary>
    public class PlayerCollisionDetection : MonoBehaviour, ICanDrop
    {
        [SerializeField] private int _dropLimit;

        public static event Action OnPlayerWins;
        public static event Action OnPlayerFall;
        public static event Action OnKeyCollected;
        public static event Action OnPlayerHit;
        public static event EventHandler<bool> OnNpcInteraction;

        public int DropLimit { get => _dropLimit; }

        private bool immortal = false;

        private void Update()
        {
            if(Hacks.Hak.Immortal()) ToggleImmortal();
            //Checks whether the player has fallen from the floor
            ICanDrop drop = this;
            if(drop.DropBoundary(PlayerMovement.PlayerSingleton.localPosition.y)) OnPlayerFall?.Invoke();
        }

        private void OnTriggerEnter(Collider other) => Behavior(other.gameObject);
        private void OnTriggerExit(Collider other) => Behavior(other.gameObject,true);

        private void OnCollisionEnter(Collision other) => Behavior(other.gameObject);

        /// <summary>
        /// This method is used to react accordingly 
        /// to collisions detected 
        /// </summary>
        /// <remarks>
        /// Mainly used to trigger events
        /// </remarks>
        private void Behavior(GameObject objectCollided, bool onExit = false)
        {
            string tag = objectCollided.tag;
            switch(tag)
            {
                //Player loses money when colliding with cars
                case "Car":
                    if (immortal)
                    {
                        Destroy(objectCollided);
                        break;
                    }
                    OnPlayerHit?.Invoke();
                    PlayerMoney.Money.GiveMoney(this,objectCollided.GetComponent<EnemyData>().AmountMoneyGrab);

                    //Stops the movement of the enemy
                    objectCollided.GetComponent<EnemyMovement>().EnemyRest();
                    break;
                //Player gets money when colliding with cash
                case "Money":
                    PlayerMoney.Money.CollectMoney(this,objectCollided.GetComponent<MoneyData>().AmountWorth);
                    Destroy(objectCollided);
                    break;
                //Interacts with Npc
                case "Npc":
                    OnNpcInteraction?.Invoke(this, onExit);
                    break;
                //Key gets added to inventory
                case "Key":
                    OnKeyCollected?.Invoke();
                    Destroy(objectCollided);
                    break;
                //Player reaches finish line (wins)
                case "Flag":
                    OnPlayerWins?.Invoke();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This class toggles the immortality of the player ;)
        /// </summary>
        private void ToggleImmortal() => immortal = !immortal;

    }
}
