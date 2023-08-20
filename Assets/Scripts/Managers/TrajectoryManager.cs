using UnityEngine;

namespace CitadelShowdown.Managers
{
    public class TrajectoryManager : MonoBehaviour
    {
        [Header("Trajectory")]
        [SerializeField] private int numTrajectoryPoints = 10;
        [SerializeField] private GameObject trajectoryPointPrefab;
        [SerializeField] private GameObject[] trajectoryPoints;
        [SerializeField] private LineRenderer trajectoryLineRenderer;

        [SerializeField] private float maxDragDistance = 3f;

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

            var lastPos = attackerTransform.position;

            Debug.Log(numPointsToShow);

            for (int i = 0; i < numTrajectoryPoints; i++)
            {
                //float percentage = (i + 1) / (float)numPointsToShow; // Calculate percentage
                Vector3 pointPosition = lastPos + throwDirection;
                trajectoryPoints[i].transform.position = pointPosition;
                trajectoryPoints[i].SetActive(true);
                lastPos = pointPosition;
            }

            // Hide the remaining points
            for (int i = numTrajectoryPoints - 1; i > numPointsToShow; i--)
                trajectoryPoints[i].SetActive(false);
        }

        public void UpdateTrajectoryLine(Vector3 start, Vector3 current)
        {
            Vector3 dragVector = start - current;
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);
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

