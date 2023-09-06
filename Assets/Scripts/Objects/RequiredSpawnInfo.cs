using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>RequiredSpawnInfo</c> retrieves the available positions, 
    /// so that objects dont spawn within a building.
    /// </summary>
    /// <remarks>
    /// Only one is needed per scene
    /// </remarks>
    public class RequiredSpawnInfo : EventListener
    {
        
        [SerializeField] private GameObject[] buildings;
        [SerializeField] private GameObject floor;

        private static RequiredSpawnInfo _spawnInfo;
        public static RequiredSpawnInfo SpawnInfo { get => _spawnInfo; }

        private List<Vector2Int> occupiedPositionsXZ;
        private static List<Vector2Int> _availablePositionsXZ;

        private static Transform _floorTransform;

        public static Transform FloorTransform { get => _floorTransform; }
        public static List<Vector2Int> AvaialablePositionsXZ { get => _availablePositionsXZ; }

        private void Awake()
        {
            if (_spawnInfo == null)
            {
                _spawnInfo = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            //Terrain bounds
            if (floor == null) floor = GameObject.Find("floor");
            _floorTransform = floor.transform;

            //Needed to avoid spawning items inside buildings
            if (buildings.Length == 0) buildings = GameObject.FindGameObjectsWithTag("Building");

            //list of the occupied and available x and z points
            occupiedPositionsXZ = new List<Vector2Int>();
            _availablePositionsXZ = new List<Vector2Int>();

            //Getting positions where objects should NOT be able to spawn
            occupiedPositionsXZ = UnavailablePositions(buildings);

            //Retrieves all the available positions on the surface 
            _availablePositionsXZ = AvailablePositions((int)_floorTransform.localScale.x/2 - 5,(int)_floorTransform.localScale.z/2 - 5,occupiedPositionsXZ);

        }

        /// <summary>
        /// This method returns a list of numbers from min (inclusive) to max (inclusive)
        /// </summary>
        /// <param name="min">The starting number of the list.</param>
        /// <param name="max">The ending number of the list.</param>
        private List<int> AddNumbersInRange(int min, int max)
        {
            //empty list
            List<int> numbers = new List<int>();

            //iterating from the min to max and adding each number to the list
            for(int i=min; i<=max; i++)
            {
                numbers.Add(i);
            }
            return numbers;
        }

        /// <summary>
        /// This method returns unvailable positions based on positions of an object
        /// or objects sent in as an argument
        /// </summary>
        /// <param name="obj">An array of objects to check their position and scale</param>
        private List<Vector2Int> UnavailablePositions(GameObject[] obj)
        {
            List<Vector2Int> unavailablePos = new List<Vector2Int>();

            //Iterating through the objects to check the positions and scales for
            for (int i = 0; i < obj.Length; i++)
            {
                Vector3 position = obj[i].transform.position;
                Vector3 scale = obj[i].transform.localScale;

                //Getting lists from boths axis to create positions based on them
                List<int> xNotAllowed = AddNumbersInRange((int)(position.x - scale.x / 2), (int)(position.x + scale.x / 2));
                List<int> zNotAllowed = AddNumbersInRange((int)(position.z - scale.z / 2), (int)(position.z + scale.z / 2));

                //Adding the positions to the unavaiable list to return
                for (int j = 0; j < xNotAllowed.Count; j++)
                {
                    for (int l = 0; l < zNotAllowed.Count; l++)
                    {
                        unavailablePos.Add(new Vector2Int(xNotAllowed[j], zNotAllowed[l]));
                    }
                }
            }

            return unavailablePos;
        }

        /// <summary>
        /// This method returns available positions based on a boundary (floor) and a list of unavailable positions
        /// or objects sent in as an argument
        /// </summary>
        /// <param name="floorScaleX">The scale of the boundary on the <c>X</c> axis</param>
        /// <param name="floorScaleZ">The scale of the boundary on the <c>Z</c>c> axis</param>
        /// <param name="comparingList">A list of positions that are already taken</param>
        private List<Vector2Int> AvailablePositions(int floorScaleX, int floorScaleZ, List<Vector2Int> comparingList)
        {
            List<Vector2Int> availablePos = new List<Vector2Int>();

            //Iterate through one point of the boundary to the other
            for(int i = -floorScaleX; i < floorScaleX; i++)
            {
                for(int j = -floorScaleZ; j < floorScaleZ; j++)
                {
                    Vector2Int v = new Vector2Int(i,j);
                    bool found = false;

                    //Comparing if the position matches one of the unavailable positions
                    for(int l = 0; l < comparingList.Count; l++)
                    {
                        if (v == comparingList[l])
                        {
                            found = true;
                            break;
                        }
                    }

                    //If not position was found, add the position to the list to return
                    if(!found) availablePos.Add(v);
                }
            }

            return availablePos;
        }


        /// <summary>
        /// This method destroys the gameobject
        /// when switching to a new scene
        /// </summary>
        /// <remarks>
        /// This is done because available positions
        /// differ between scenes
        /// </remarks>
        void DestroyInstance()
        {
            if(_spawnInfo != null) Destroy(gameObject);
        }

        protected override void SubscribeToEvent()
        {
            GameManager.OnChangeScene += DestroyInstance;
            LoadScene.OnNewSceneLoad += DestroyInstance;
        }

        protected override void UnsubscribeFromEvent()
        {
            GameManager.OnChangeScene -= DestroyInstance;
            LoadScene.OnNewSceneLoad -= DestroyInstance;
        }

    }
}
