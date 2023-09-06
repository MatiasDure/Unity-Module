using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>WaypointFollower</c> is used when you want an object 
    /// to follow a waypoint system
    /// </summary>
    [RequireComponent(typeof(CooldownSystem))]
    public class WaypointFollower : MonoBehaviour, IHasCooldown
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private GameObject movingObject;
        [SerializeField] private float speed;
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private int _id;
        [SerializeField] private CooldownSystem cooldownSystem; 

        private Transform currentWaypoint;
        private int currentIndex = 0;

        private const float DistanceToSwitchWaypoints = .15f;

        public int Id { get => _id; }
        public float CooldownDuration { get => _cooldownDuration; }

        private void Awake()
        {
            if (cooldownSystem == null) cooldownSystem = GetComponent<CooldownSystem>();
        }

        void Start() => currentWaypoint = waypoints[currentIndex];


        private void FixedUpdate() => MoveTowardsWaypoint();


        /// <summary>
        /// This method moves the object towards the current waypoint
        /// </summary>
        void MoveTowardsWaypoint()
        {
            //Checking whether it is currently on cooldown
            if (cooldownSystem.IsOnCoolDown(_id)) return;

            //finding the distance between the positions 
            Vector3 difference = CalculateDirection();//currentWaypoint.localPosition - movingObject.transform.localPosition;

            //moving the object with the declared speed
            movingObject.transform.localPosition += difference.normalized * speed * Time.deltaTime;

            if (difference.magnitude <= DistanceToSwitchWaypoints) SwitchWayPoint();
        }

        /// <summary>
        /// This method calculates the difference between the current waypoint's
        /// position and the position of the object that moves to determine the direction
        /// </summary>
        Vector3 CalculateDirection() => currentWaypoint.localPosition - movingObject.transform.localPosition;


        /// <summary>
        /// This method updates the waypoint once the object has
        /// move towards a new one
        /// </summary>
        void SwitchWayPoint()
        {
            cooldownSystem.PutOnCooldown(this);
            currentIndex++;

            //Not going over the amount of waypoints
            if (currentIndex == waypoints.Length) currentIndex = 0;

            currentWaypoint = waypoints[currentIndex];
        }
    }
}