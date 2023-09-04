using CitadelShowdown.Common.Abstract;
using CitadelShowdown.DI;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class TrajectoryManager : MonoBehaviour, IRenewable
    {
        [Header("Trajectory")]
        [SerializeField] private int numTrajectoryPoints = 10;
        [Range(0.1f, 3f)]
        [SerializeField] private float indicatorSpace;
        [SerializeField] private GameObject trajectoryPointPrefab;
        [SerializeField] private GameObject[] trajectoryPoints;
        [SerializeField] private LineRenderer trajectoryLineRenderer;

        private CoreLoopFacade _coreLoopFacade;

        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade)
        {
            _coreLoopFacade = coreLoopFacade;
        }

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            trajectoryLineRenderer = GetComponent<LineRenderer>();
            trajectoryLineRenderer.positionCount = 2;
            trajectoryLineRenderer.enabled = false;
            InitializeTrajectoryPoints();
            Renew();
        }

        private void InitializeTrajectoryPoints()
        {
            trajectoryPoints = new GameObject[numTrajectoryPoints];
            for (var i = 0; i < numTrajectoryPoints; i++)
            {
                trajectoryPoints[i] = Instantiate(trajectoryPointPrefab);
                trajectoryPoints[i].SetActive(false);
            }
        }

        public void UpdateTrajectory(Vector3 throwDirection, float forcePercentage, Transform attackerTransform)
        {
            foreach (var trajectoryPoint in trajectoryPoints)
                trajectoryPoint.SetActive(false);

            // Calculate the number of points to show based on force
            var numPointsToShow = Mathf.Floor(forcePercentage / numTrajectoryPoints);

            var lastPos = attackerTransform.position + throwDirection * 2f;

            for (var i = 0; i < numPointsToShow; i++)
            {
                var pointPosition = lastPos + throwDirection * indicatorSpace;
                var trajectoryPoint = trajectoryPoints[i];
                trajectoryPoint.transform.position = pointPosition;

                // Calculate the rotation to face the throw direction
                var rotation = Quaternion.LookRotation(Vector3.forward, throwDirection);
                trajectoryPoint.transform.rotation = rotation;

                trajectoryPoint.SetActive(true);
                lastPos = pointPosition;
            }
        }

        public void UpdateTrajectoryLine(Vector3 start, Vector3 current)
        {
            var dragVector = start - current;
            var dragDistance = Mathf.Clamp(dragVector.magnitude, 0, _coreLoopFacade.ConfigurationManager.MovementConfigs.MaxDragDistance);
            var clampedEnd = start - dragVector.normalized * dragDistance;

            trajectoryLineRenderer.enabled = true;
            trajectoryLineRenderer.SetPosition(0, start);
            trajectoryLineRenderer.SetPosition(1, clampedEnd);
        }

        public Vector3 CalculatePointPosition(Vector3 initialPosition, Vector2 direction, float force, float time)
        {
            var gravity = Physics2D.gravity.y;
            var displacement = force * time * direction + 0.5f * gravity * time * time * Vector2.up;

            return initialPosition + (Vector3)displacement;
        }

        public void HideTrajectory()
        {
            var firstPosition = trajectoryPoints[0].transform.position;

            foreach (var point in trajectoryPoints)
            {
                point.SetActive(false);
                point.transform.position = firstPosition;
            }

            trajectoryLineRenderer.enabled = false;
        }

        public void Renew()
        {
            HideTrajectory();
        }
    }
}

