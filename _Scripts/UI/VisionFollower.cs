using UnityEngine;

namespace TerrariumXR.UI
{
    // from https://www.youtube.com/watch?v=ler5ffTJnrk
    public class VisionFollower : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool continuous = true;

        private void Update()
        {
            if (continuous)
            {
                LookAtPlayer();
            }
        }


        private void LookAtPlayer()
        {
            // Rotate towards the player
            // var lookAtPos = new Vector3(cameraTransform.transform.position.x, transform.position.y, cameraTransform.transform.position.z);
            // transform.LookAt(cameraTransform);

            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.transform.position);
        }



    // ================== Archive ==================
        //
        // [SerializeField] private int maxTimesMoved = 50;
        // [SerializeField] private float xDistance = 0.0f;
        // [SerializeField] private float yDistance = -0.3f;
        // [SerializeField] private float zDistance = 0.5f;

        // private bool isCentered = false;
        // private bool isFacing = false;
        // private int timesMoved = 0;

        // // Built-in Unity *Magic*
        // private void OnBecameInvisible()
        // {
        //     isCentered = false;
        // }

        // private void Update()
        // {
        //     if (continuous || timesMoved < maxTimesMoved)
        //     {
        //         if (!isCentered)
        //         {
        //             // Find the position we need to be at
        //             Vector3 targetPosition = FindTargetPosition();

        //             // Move just a little bit at a time
        //             MoveTowards(targetPosition);

        //             // // If we've reached the position, don't do anymore work
        //             // if (ReachedPosition(targetPosition))
        //             //     isCentered = true;
        //         }
        //         if (!isFacing)
        //         {
        //             LookAtPlayer();
        //         }
                
        //         timesMoved++;
        //     }
        // }

        // private Vector3 FindTargetPosition()
        // {
        //     // Let's get a position infront of the player's camera
        //     Vector3 xComponent = cameraTransform.right * xDistance;
        //     Vector3 yComponent = cameraTransform.up * yDistance;
        //     Vector3 zComponent = cameraTransform.forward * zDistance;
        //     return cameraTransform.position + xComponent + yComponent + zComponent;
        // }

        // private void MoveTowards(Vector3 targetPosition)
        // {
        //     // Instead of a tween, that would need to be constantly restarted
        //     transform.position += (targetPosition - transform.position) * 0.025f;
        // }

        // private bool ReachedPosition(Vector3 targetPosition)
        // {
        //     // Simple distance check, can be replaced if you wish
        //     return Vector3.Distance(targetPosition, transform.position) < 0.1f;
        // }

        // private void LookAtPlayer()
        // {
        //     // Rotate towards the player
        //     // var lookAtPos = new Vector3(cameraTransform.transform.position.x, transform.position.y, cameraTransform.transform.position.z);
        //     // transform.LookAt(cameraTransform);

        //     transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.transform.position);
        // }

        // public void ResetTimer()
        // {
        //     timesMoved = 0;
        // }
    }
}