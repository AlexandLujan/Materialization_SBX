using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Materialization.Core.Input;

namespace Materialization.Features.Camera
{
    public class ThirdPersonOrbitCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private LayerMask collisionMask;

        [Header("Orbit Settings")]
        [SerializeField] private float defaultDistance = 6f;
        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float maxDistance = 100.0f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;
        [SerializeField] private float mouseSensitivity = 0.12f;
        [SerializeField] private float normalModeYawSensitivity = 0.08f;
        [SerializeField] private bool allowNormalModeHorizontalOrbit = true;

        [Header("Zoom")]
        [SerializeField] private float zoomStep = 1.0f;
        [SerializeField] private float zoomSmoothTime = 0.08f;

        [Header("Collision")]
        [SerializeField] private float collisionRadius = 0.25f;
        [SerializeField] private float collisionBuffer = 0.1f;

        [Header("Smoothing")]
        [SerializeField] private float positionSmoothTime = 0.05f;
        [SerializeField] private float rotationSmoothTime = 0.05f;

        private float targetYaw;
        private float targetPitch;
        private float currentYaw;
        private float currentPitch;

        private float targetDistance;
        private float currentDistance;
        private float zoomVelocity;

        private Vector3 cameraVelocity;
        private float yawVelocity;
        private float pitchVelocity;

        private void Start()
        {
            Vector3 euler = transform.eulerAngles;

            targetYaw = euler.y;
            targetPitch = euler.x;

            currentYaw = targetYaw;
            currentPitch = targetPitch;

            targetDistance = defaultDistance;
            currentDistance = defaultDistance;
        }

        private void LateUpdate()
        {
            if (target == null || input == null) return;

            HandleCameraRotation();
            HandleZoom();
            UpdateCameraPosition();
        }

        private void HandleCameraRotation()
        {
            if (!input.CameraModeActive) return;

            Vector2 look = input.Look;

            if (input.CameraModeActive)
            {
                targetYaw += look.x * mouseSensitivity;
                targetPitch -= look.y * mouseSensitivity;
            }
            else if (allowNormalModeHorizontalOrbit)
            {
                targetYaw += look.x * normalModeYawSensitivity;
            }

            targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);
        }

        private void HandleZoom()
        {
            float zoomInput = input.ConsumeZoom();

            if (Mathf.Abs(zoomInput) > 0.01f)
            {
                targetDistance -= zoomInput * zoomStep;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }

            currentDistance = Mathf.SmoothDamp(
                currentDistance,
                targetDistance,
                ref zoomVelocity,
                zoomSmoothTime
                );
        }

        private void UpdateCameraPosition()
        {
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
            currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);

            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

            Vector3 targetPosition = target.position;
            Vector3 desiredOffset = rotation * new Vector3(0f, 0f, -currentDistance);
            Vector3 desiredPosition = targetPosition + desiredOffset;

            Vector3 correctedPosition = ResolveCollision(targetPosition, desiredPosition);

            transform.position = Vector3.SmoothDamp(
                transform.position,
                correctedPosition,
                ref cameraVelocity,
                positionSmoothTime
            );

            Vector3 lookDirection = (targetPosition - transform.position).normalized;

            if (lookDirection.sqrMagnitude > 0.0001f)
                transform.rotation = rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        private Vector3 ResolveCollision(Vector3 origin, Vector3 desiredPosition)
        {
            Vector3 direction = desiredPosition - origin;
            float desiredDistance = direction.magnitude; // OH YEAH!!!!

            if (desiredDistance < 0.001f) return origin;

            direction.Normalize();

            if (Physics.SphereCast(
                origin,
                collisionRadius,
                direction,
                out RaycastHit hit,
                desiredDistance,
                collisionMask,
                QueryTriggerInteraction.Ignore
            ))
            {
                float safeDistance = Mathf.Max(minDistance, hit.distance - collisionBuffer);
                return origin + direction * safeDistance;
            }

            return desiredPosition;
        }

        public void ResetZoom()
        {
            targetDistance = defaultDistance;
        }
    }
}
