using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>EnemyMovement</c> is in charge of the movement of the 
    /// enemies in the game 
    /// </summary>
    public class EnemyMovement : EventListener, ICanDrop
    {
        [SerializeField] private float speed;
        [SerializeField] private int distanceToStartPersue;
        [SerializeField] private int _dropLimit;
        [SerializeField] private Transform target;

        public int DropLimit { get => _dropLimit; }

        private Vector3 direction;
        private bool stopMoving = false;
        private bool playerTalkingToNPC = false;

        protected override void Start()
        {
            base.Start();
            target = PlayerMovement.PlayerSingleton;
        }

        private void Update()
        {
            ICanDrop drop = this;
            if (drop.DropBoundary(transform.position.y)) Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (playerTalkingToNPC) return;
            if(target != null)
            LookAtTarget();
            CalculateDirection();
            Move();
        }

        /// <summary>
        /// This method calculates the distance between the target's 
        /// positions and it's position
        /// </summary>
        private void CalculateDirection()
        {
            direction = target.position - transform.position;
            if(stopMoving && direction.magnitude > distanceToStartPersue) stopMoving = false;
            direction.Normalize();
        }

        /// <summary>
        /// This method updates the position of the 
        /// enemy with the new calculated direction
        /// </summary>
        private void Move()
        {
            if (stopMoving) return;
            transform.position += direction * speed * Time.deltaTime;
        }

        /// <summary>
        /// This method rotates to look 
        /// towards the target
        /// </summary>
        private void LookAtTarget() => transform.LookAt(target);

        /// <summary>
        /// This method stops the enemy from moving
        /// </summary>
        public void EnemyRest() => stopMoving = true;

        /// <summary>
        /// This method pause all updates in the enemy
        /// </summary>
        private void PauseEnemies(System.Object sender, bool onExit) => playerTalkingToNPC = !onExit;

        /// <summary>
        /// This method checks the current <c>Y</c> position of the enemy,
        /// and destroys it if this is lower than the argument passed
        /// </summary>
        public void DropBoundary(float objPositionY)
        {
            if (objPositionY <= DropLimit) Destroy(gameObject);
        }

        protected override void SubscribeToEvent() => PlayerCollisionDetection.OnNpcInteraction += PauseEnemies;

        protected override void UnsubscribeFromEvent() => PlayerCollisionDetection.OnNpcInteraction -= PauseEnemies;

    }
}