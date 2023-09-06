using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>PlayerMovement</c> is used to control 
    /// the movement of the player
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] _wheelColliders;
        [SerializeField] private Transform[] wheels;
        [SerializeField] private float acceleration;
        [SerializeField] private float brakingForce;
        [SerializeField] private float maxTurnAngle;

        //Creates a singleton for external use
        static Transform _playerSingleton = null;
        public static Transform PlayerSingleton { get => _playerSingleton; }

        public WheelCollider[] WheelCollider { get => _wheelColliders; }

        //limits for acceleration, braking, and turn angle

        float currentAcceleration = 0;
        float currentBrakeForce = 0;
        float currentTurnAngle = 0;

        //starting car sound
        bool hasEnteredCar, hasStartedCar = false;

        //Events 
        public static event EventHandler<AudioManager.Sound> OnStateChange;

        void Awake()
        {
            if (_playerSingleton == null) _playerSingleton = this.transform;
        }

        void Start() => OnStateChange?.Invoke(this, AudioManager.Sound.PlayerEnter);

        void Update()
        {
            if (!hasStartedCar) StartCar();
            GetInput();
            FiringEvents();
        }

        void FixedUpdate()
        {
            Move();
            Brake();
        }

        /// <summary>
        /// This method get the input from
        /// the user 
        /// </summary>
        void GetInput()
        {

            //accelerate car
            currentAcceleration = Input.GetAxis("Vertical") * acceleration  * Time.deltaTime;
            
            //brake car
            currentBrakeForce = Input.GetKey(KeyCode.Space) ? brakingForce * Time.deltaTime : 0;

            //turn car
            currentTurnAngle = Input.GetAxis("Horizontal") * maxTurnAngle;
        }

        /// <summary>
        /// This method moves the car
        /// </summary>
        void Move()
        {
            //acceleration force and turn added to the FRONT wheels
            for (int i = 0; i < _wheelColliders.Length / 2; i++)
            {
                _wheelColliders[i].motorTorque = currentAcceleration;
                Turn(_wheelColliders[i], wheels[i]);
            }
        }

        /// <summary>
        /// This method brakes the car
        /// </summary>
        void Brake()
        {
            //brake force added to each wheel collider
            for (int i = 0; i < _wheelColliders.Length; i++)
            {
                _wheelColliders[i].brakeTorque = currentBrakeForce;
            }
        }

        /// <summary>
        /// This method turns the car
        /// </summary>
        void Turn(WheelCollider collider, Transform wheel)
        {
            //turns the car
            collider.steerAngle = currentTurnAngle;
            //Rotates the wheels the amount of degrees we are turning            
            wheel.localEulerAngles = new Vector3(0, currentTurnAngle, 0);
        }

        /// <summary>
        /// This method plays the sound a car makes when starting
        /// </summary>
        void StartCar()
        {
            if (!hasEnteredCar && !AudioManager.IsPlaying(AudioManager.Sound.PlayerEnter))
            {
                OnStateChange?.Invoke(this, AudioManager.Sound.PlayerStartCar);
                hasEnteredCar = true;
                return;
            }
            else if (hasEnteredCar && !AudioManager.IsPlaying(AudioManager.Sound.PlayerStartCar))
            {
                OnStateChange?.Invoke(this, AudioManager.Sound.GameSoundtrack);
                hasStartedCar = true;
            }
        }

        /// <summary>
        /// This method is in charge of triggering 
        /// events to update the sounds
        /// </summary>
        void FiringEvents()
        {
            if(hasStartedCar)
            {
                if (currentAcceleration == 0) OnStateChange?.Invoke(this, AudioManager.Sound.PlayerStopped);
                else OnStateChange?.Invoke(this, AudioManager.Sound.PlayerMoving);
                if (Math.Abs(currentTurnAngle) > maxTurnAngle - 3) OnStateChange?.Invoke(this, AudioManager.Sound.PlayerTurn);
            }
        }

        void OnDestroy()
        {
            _playerSingleton = null;
        }

    }
}
