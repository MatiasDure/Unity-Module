using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Use class <c>CameraFollow</c> when you want a camera to follow a specified target
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float cameraRotationSpeed;

        Quaternion startingOrientation;
        Quaternion currentOrientation;
        Vector3 offset;

        void Start()
        {
            if (CheckForNull()) return;
            //Retrieving the offset between the camera and the target
            offset = transform.position - target.position;

            //Storing the starting rotation of the camera
            startingOrientation = transform.rotation;
        }

        void FixedUpdate()
        {
            if (CheckForNull()) return;
            //Retrieves only the y rotation, ignoring x and z
            Quaternion targetYRotation = Quaternion.Euler(0, target.rotation.eulerAngles.y, 0);

            //rotates the camera from the current rotation to the targets rotation at a specified speed (smooths in)
            currentOrientation = Quaternion.Lerp(currentOrientation, targetYRotation, cameraRotationSpeed * Time.fixedDeltaTime);

            //Assiging the new position and rotation to the camera
            UpdateTransform();
        }

        /// <summary>
        /// This method updates the transform of the camera
        /// </summary>
        private void UpdateTransform()
        {
            transform.position = target.position + currentOrientation * offset;
            transform.rotation = currentOrientation * startingOrientation;
        }

        bool CheckForNull() => target == null;

        
    }
}
