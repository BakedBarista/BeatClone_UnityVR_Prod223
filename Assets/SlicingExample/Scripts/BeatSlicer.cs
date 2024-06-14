using NobleMuffins.TurboSlicer;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Assumptions: 
/// Targets have one convex mesh.
/// Target has a single meshrenderer (No skinned meshes)
/// The slicer "blade" is omnidirectional, it cuts in any direction (think lightsaber rather than metal sword blade). The axis of the blade matches the local Y axis and passes through the center of local space (no x/z offset)
/// Targets pivots are at their center.
/// </summary>
public class BeatSlicer : MonoBehaviour
{
    private new Rigidbody rigidbody;

    // Center of mass (mid blade if collider properly setup to match blade) is a reasonable approximation of the blades motion for impacts
    private Vector3 lastBladeCenterPositionWorld;
    private Vector3 currentBladeCenterPositionWorld;
    private Vector3 bladeMovementDirection = Vector3.zero;

    // Angle threshold for determining if slicing is allowed
    private const float thresholdAngle = 45f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        // Unity (2018.3.3)(bug?) won't calculate (or RecalculateCenterofMass) center of mass if isKinematic is true. Ensure isKinematic is set false so that CoM is recalculated then restore whatever the original value was.
        bool isKinematic = rigidbody.isKinematic;
        rigidbody.isKinematic = false;
        rigidbody.isKinematic = isKinematic;

        currentBladeCenterPositionWorld = transform.position + transform.TransformDirection(rigidbody.centerOfMass);
        lastBladeCenterPositionWorld = currentBladeCenterPositionWorld;
    }

    private void LateUpdate()
    {
        // Update blade motion and position vectors
        currentBladeCenterPositionWorld = transform.position + transform.TransformDirection(rigidbody.centerOfMass);
        bladeMovementDirection = currentBladeCenterPositionWorld - lastBladeCenterPositionWorld;
        bladeMovementDirection.Normalize();
        lastBladeCenterPositionWorld = currentBladeCenterPositionWorld;
    }

    private void OnTriggerEnter(Collider other)
    {
        BeatTarget target = other.GetComponentInParent<BeatTarget>();
        if (target != null)
        {
            if (other is MeshCollider)
            {
                Assert.IsTrue(((MeshCollider)other).convex, "Target collider is not convex. Please use only convex colliders on targets");
            }

            if ((gameObject.layer == LayerMask.NameToLayer("BlueSword") && other.gameObject.layer == LayerMask.NameToLayer("BlueBeat")) ||
                (gameObject.layer == LayerMask.NameToLayer("RedSword") && other.gameObject.layer == LayerMask.NameToLayer("RedBeat")))
            {
                // Get the cube's local Y direction in world space
                Vector3 cubeUpDirection = target.transform.up;

                // Check if the blade movement direction is against the cube's local Y direction
                float angle = Vector3.Angle(-cubeUpDirection, bladeMovementDirection);
                if (angle <= thresholdAngle)
                {
                    Vector3 thirdSlicingPlanePoint = transform.position + bladeMovementDirection;
                    if (bladeMovementDirection == Vector3.zero)
                    {
                        // If the blade isn't moving then the third point would be on the line of the other two points and a slicing plane can not be created.
                        // this is very rough, it produces a slice but quite possibly not oriented very well
                        thirdSlicingPlanePoint = other.ClosestPoint(currentBladeCenterPositionWorld);
                    }

                    // Perform the slice
                    SliceTarget(target, transform.position, transform.position + transform.up, thirdSlicingPlanePoint);

                    if (ScoreManager.Instance != null)
                    {
                        ScoreManager.Instance.AddScore();
                    }
                    else
                    {
                        Debug.LogError("ScoreManager instance is not set!");
                    }

                }
                else
                {
                    Debug.Log("Cube not sliced from the allowed direction!");
                }
            }
        }
    }

    /// <summary>
    /// Perform the slice using the plane defined by three points (A,B,C) on that plane
    /// </summary>
    /// <param name="target">To slice</param>
    /// <param name="planePointA"></param>
    /// <param name="planePointB"></param>
    /// <param name="planePointC"></param>
    private void SliceTarget(BeatTarget target, Vector3 planePointA, Vector3 planePointB, Vector3 planePointC)
    {
        TurboSlicer turboSlicer = TurboSlicerSingleton.Instance;
        bool destroyOriginal = true;
        TurboSlicerSingleton.Instance.SliceByTriangle(target.gameObject, new Vector3[] { planePointA, planePointB, planePointC }, destroyOriginal);
    }
}
