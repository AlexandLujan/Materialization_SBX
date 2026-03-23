using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public float SlopeAngle { get; private set; }
        public bool JustLanded { get; private set; }
        public bool JustLeftGround { get; private set; }

        [SerializeField] private Transform groundCheckOrigin;
        [SerializeField] private float checkRadius = 0.25f;
        [SerializeField] private float checkDistance = 0.3f;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float maxWalkableSlopeAngle = 45f;

        private bool wasGrounded;

        private void Update()
        {
            // Debug.Log("Ground checker.");
            wasGrounded = IsGrounded;

            PerformGroundCheck();

            JustLanded = !wasGrounded && IsGrounded;
            JustLeftGround = wasGrounded && !IsGrounded;

            /*
            if (JustLanded)
                Debug.Log("[GroundChecker] JustLanded");

            if (JustLeftGround)
                Debug.Log("[GroundChecker] JustLeftGround");

            if (IsGrounded)
                Debug.Log($"[GroundChecker] Grounded | SlopeAngle: {SlopeAngle:F2}");
            */
        }

        private void PerformGroundCheck()
        {
            if (groundCheckOrigin == null)
            {
                // Debug.LogWarning("GroundCheckerOrigin is null.");
                IsGrounded = false;
                GroundNormal = Vector3.up;
                SlopeAngle = 0f;
                return;
            }

            if (Physics.SphereCast(
                groundCheckOrigin.position,
                checkRadius,
                Vector3.down,
                out RaycastHit hit,
                checkDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore))
            {
                GroundNormal = hit.normal;
                SlopeAngle = Vector3.Angle(Vector3.up, GroundNormal);
                IsGrounded = SlopeAngle <= maxWalkableSlopeAngle;
                // Debug.Log("HIT!");
            }
            else
            {
                IsGrounded = false;
                GroundNormal = Vector3.up;
                SlopeAngle = 0f;
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (groundCheckOrigin == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckOrigin.position, checkRadius);
            Gizmos.DrawWireSphere(groundCheckOrigin.position + Vector3.down * checkDistance, checkRadius);
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * checkDistance);
        }
    }
}
