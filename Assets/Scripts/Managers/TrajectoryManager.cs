using CitadelShowdown.DI;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class TrajectoryManager : MonoBehaviour
    {
        [Header("Trajectory")]
        [SerializeField] private int numTrajectoryPoints = 10;
        [SerializeField] private GameObject trajectoryPointPrefab;
        [SerializeField] private GameObject[] trajectoryPoints;
        [SerializeField] private LineRenderer trajectoryLineRenderer;

        private CoreLoopFacade _coreLoopFacade;

        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade)
        {
            _coreLoopFacade = coreLoopFacade;
        }

        private void Start()
        {
            InitializeTrajectoryPoints();

            trajectoryLineRenderer = GetComponent<LineRenderer>();
            trajectoryLineRenderer.positionCount = 2;
            trajectoryLineRenderer.enabled = false;
        }

        private void InitializeTrajectoryPoints()
        {
            trajectoryPoints = new GameObject[numTrajectoryPoints];
            for (int i = 0; i < numTrajectoryPoints; i++)
            {
                trajectoryPoints[i] = Instantiate(trajectoryPointPrefab);
                trajectoryPoints[i].SetActive(false);
            }
        }

        public void UpdateTrajectory(Vector3 throwDirection, float throwForce, Transform attackerTransform)
        {
            // Calculate the number of points to show based on maxDragDistance
            int numPointsToShow = Mathf.RoundToInt(throwForce / numTrajectoryPoints);

            var lastPos = attackerTransform.position + throwDirection * 2f;

            for (int i = 0; i < numTrajectoryPoints; i++)
            {
                Vector3 pointPosition = lastPos + throwDirection * 0.5f;
                var trajectoryPoint = trajectoryPoints[i];
                trajectoryPoint.transform.position = pointPosition;

                // Calculate the rotation to face the throw direction
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, throwDirection);
                trajectoryPoint.transform.rotation = rotation;

                trajectoryPoint.SetActive(true);
                lastPos = pointPosition;
            }

            // Hide the remaining points
            for (int i = numTrajectoryPoints - 1; i > numPointsToShow; i--)
                trajectoryPoints[i].SetActive(false);
        }

        public void UpdateTrajectoryLine(Vector3 start, Vector3 current)
        {
            Vector3 dragVector = start - current;
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, _coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);
            Vector3 clampedEnd = start - dragVector.normalized * dragDistance;

            trajectoryLineRenderer.enabled = true;
            trajectoryLineRenderer.SetPosition(0, start);
            trajectoryLineRenderer.SetPosition(1, clampedEnd);
        }

        public Vector3 CalculatePointPosition(Vector3 initialPosition, Vector2 direction, float force, float time)
        {
            float gravity = Physics2D.gravity.y;
            Vector2 displacement = direction * force * time + 0.5f * Vector2.up * gravity * time * time;
            return initialPosition + new Vector3(displacement.x, displacement.y, 0);
        }

        public void HideTrajectory()
        {
            foreach (GameObject point in trajectoryPoints)
            {
                point.SetActive(false);
            }

            trajectoryLineRenderer.enabled = false;
        }
    }
}

