using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>RayCollisionWithPlatform</c> is used exclusively to
    /// deal with hits that happen with moving platform
    /// </summary>
    public class RayCollisionWithPlatform : RayCollider
    {
        protected override void Awake()
        {
            base.Awake();
            originOfRay = new Vector3(objectTransform.position.x, objectTransform.position.y + startingY,objectTransform.position.z);
        }

        private void Start()
        {
            //Checking a hit with the specified layer mask (in inspector) with the bitwise operator left shift <<
            layerMask = 1 << layerMask; //shifting 1 n times to the left. ex: 1 << 3 = 1000
        }

        protected override void FixedUpdate()
        {
            if (objectTransform == null) return;
            UpdateOriginPosition();
            RaycastHit hit;
            if(TagMatched(tagToCompare,out hit, true, layerMask)) Ride(hit);
            else StopRide();
        }

        /// <summary>
        /// This method makes the platform become the 
        /// parent of the target's transform
        /// </summary>
        /// <remarks>
        /// This is how an object would move along
        /// with a movable platform
        /// </remarks>
        private void Ride(RaycastHit hitInfo)
        {
            objectTransform.parent = hitInfo.transform;
        }

        /// <summary>
        /// This method removes the parent of the object
        /// </summary>
        private void StopRide() => objectTransform.parent = null;

        private void OnDestroy()
        {
            objectTransform.parent = null;
        }
    }

}
