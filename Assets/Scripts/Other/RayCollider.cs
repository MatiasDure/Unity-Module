using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>RayCollider</c> is used to collect 
    /// information from raycasts
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class RayCollider : MonoBehaviour
    {
        [SerializeField] protected int rayDistance = 1;
        [SerializeField] protected Transform objectTransform;
        [SerializeField] protected string tagToCompare = "";
        [SerializeField] protected Vector3 directionOfRay = Vector3.down;
        [SerializeField] protected Vector3 originOfRay = new();
        [SerializeField] protected int layerMask;

        protected Ray ray;
        protected float startingY;

        protected virtual void Awake()
        {
            if (objectTransform == null) objectTransform = GetComponent<Transform>();
            startingY = originOfRay.y;
        }

        protected virtual void FixedUpdate() {}

        /// <summary>
        /// This method shoots a ray at the desired direction and
        /// checks whether or not its hitting an object with the
        /// tag passed to it
        /// </summary>
        /// <param name="tag">Tag of the object to compare (Found in the inspector)</param>
        /// <param name="hitInfo">Variable to store the information of the hit with the out keyword</param>
        /// <param name="addLayerMask">Whether or not to use layer masks to add specific objects to collided with</param>
        /// <param name="pLayerMask">layer mask of the object to check for collision (Found in the inspector)</param>
        protected bool TagMatched(string tag, out RaycastHit hitInfo, bool addLayerMask = false, int pLayerMask = 0)
        {
            return addLayerMask ? Physics.Raycast(ray, out hitInfo, rayDistance, pLayerMask) && hitInfo.collider.CompareTag(tagToCompare) :
                Physics.Raycast(ray, out hitInfo, rayDistance) && hitInfo.collider.CompareTag(tagToCompare);
        }

        /// <summary>
        /// This method updates the new position of 
        /// the ray in case the ray is on a moving object
        /// </summary>
        protected virtual void UpdateOriginPosition()
        {
            //Updating the origin point of the ray
            originOfRay = new Vector3(objectTransform.position.x, objectTransform.position.y + startingY, objectTransform.position.z);
            //A ray object contains two vector3 (origin point and direction of ray)
            ray = new(originOfRay, directionOfRay);
        }

    }
}