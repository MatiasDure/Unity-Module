using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    
    [RequireComponent(typeof(CooldownSystem))]
    public class SpawnObjects : EventListener, IHasCooldown
    {
        /// <summary>
        /// Class <c>SpawnObject</c> is used to spawn objects
        /// around the area assigned
        /// </summary>
        [SerializeField] protected GameObject[] spawnableObjects;
        [SerializeField] protected Transform floor;
        [SerializeField] protected float _cooldownDuration;
        [SerializeField] protected int amountToSpawn;
        [SerializeField] protected int _id;
        [SerializeField] protected CooldownSystem cooldownSystem;
        [SerializeField] protected float heightToSpan;

        public int Id { get => _id; }
        public float CooldownDuration { get => _cooldownDuration; }        

        protected int currentSpawned = 0;

        //Used to spawn items within the floor's dimension
        protected float boundaryX;
        protected float boundaryZ;

        private bool playerTalkingToNpc = false;

        protected void Awake()
        {
            if (cooldownSystem == null) cooldownSystem = GetComponent<CooldownSystem>();
            if (floor == null) floor = GameObject.Find("Floor").transform;  
        }

        protected override void Start()
        {
            //we call the base class in case we need to subscribe to events
            base.Start();

            //we get the floor dimension
            EstablishBoundary();
        }

        protected virtual void Update()
        {
            if (NotAbleToSpawn()) return;
            Spawn();
        }

        /// <summary>
        /// This method Spawns the items on a randomize position
        /// </summary>
        /// <remarks>
        /// Uses the available position list from the <c>RequiredSpawnInfo</c> class
        /// </remarks>
        protected void Spawn()
        {
            cooldownSystem.PutOnCooldown(this);
            currentSpawned++;
            int randomIndex = Random.Range(0, spawnableObjects.Length);
            GameObject spawn = spawnableObjects[randomIndex];

            //Getting a random position from the available positions' list
            randomIndex = Random.Range(0,RequiredSpawnInfo.AvaialablePositionsXZ.Count);
            Vector2 positionToSpawn = RequiredSpawnInfo.AvaialablePositionsXZ[randomIndex];
            spawn.transform.position = new Vector3(positionToSpawn[0], heightToSpan, positionToSpawn[1]);
            Instantiate(spawn);
        }

        /// <summary>
        /// This method gets the bounds in which objects can spawn
        /// </summary>
        /// <remarks>
        /// Usually the floor of the game
        /// </remarks>
        protected void EstablishBoundary()
        {
            floor = RequiredSpawnInfo.FloorTransform == null ? floor = GameObject.Find("Floor").transform : RequiredSpawnInfo.FloorTransform;
            boundaryX = floor.localScale.x / 2 - 5;
            boundaryZ = floor.localScale.z / 2 - 5;
        }

        /// <summary>
        /// This method checks whether the player is 
        /// interacting with the Npc to avoid 
        /// spawning objects during that time
        /// </summary>
        private void PauseSpawning(System.Object sender, bool onExit) => playerTalkingToNpc = !onExit;

        /// <summary>
        /// This method checks whether the object is able to spawn
        /// </summary>
        virtual protected bool NotAbleToSpawn() => currentSpawned >= amountToSpawn || playerTalkingToNpc || cooldownSystem.IsOnCoolDown(Id);

        protected override void SubscribeToEvent() => PlayerCollisionDetection.OnNpcInteraction += PauseSpawning;

        protected override void UnsubscribeFromEvent() => PlayerCollisionDetection.OnNpcInteraction -= PauseSpawning;
    }
}
