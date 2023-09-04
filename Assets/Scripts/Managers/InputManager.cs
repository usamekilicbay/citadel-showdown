using UnityEngine;
using Zenject;

namespace CitadelShowdown.Managers
{
    public class InputManager
    {
        public Vector3 DragStartPosition { get; private set; }
        private Camera _mainCamera;

        private ConfigurationManager _configurationManager;

        [Inject]
        public void Construct(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;

            _mainCamera = Camera.main;
        }

        public void SetDragStartPosition() 
        {
            DragStartPosition = GetMousePosition();
        }

        public float GetForceByDragPercentage()
        {
            var dragDistance = GetDragPercentage();

            var force = dragDistance * (_configurationManager.MaxThrowForce / 100f);

            return force;
        }

        public float GetDragPercentage()
        {
            var dragDistance = GetDragDistance();

            var percentage = dragDistance / _configurationManager.MaxDragDistance * 100f;

            return percentage;
        }

        public float GetDragDistance()
        {
            var dragVector = GetDragVector();
            var dragDistance = Mathf.Clamp(dragVector.magnitude, 0, _configurationManager.MaxDragDistance);

            return dragDistance >= _configurationManager.MinDragDistance
                ? dragDistance
                : -1f;
        }

        public Vector3 GetDragVector()
            => DragStartPosition - GetMousePosition();

        public Vector3 GetMousePosition()
        {
            var currentPosition = Input.mousePosition;
            currentPosition.z = 10; // Depth of the Camera
            currentPosition = _mainCamera.ScreenToWorldPoint(currentPosition);

            return currentPosition;
        }
    }
}
