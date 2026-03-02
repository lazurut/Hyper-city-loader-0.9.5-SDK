using UnityEngine;

namespace Fragsurf.Movement
{
	public class MoveData
	{
		public Transform playerTransform;

		public Transform viewTransform;

		public Vector3 viewTransformDefaultLocalPos;

		public Vector3 origin;

		public Vector3 viewAngles;

		public Vector3 velocity;

		public float forwardMove;

		public float sideMove;

		public float upMove;

		public float surfaceFriction = 1f;

		public float gravityFactor = 1f;

		public float walkFactor = 1f;

		public float verticalAxis;

		public float horizontalAxis;

		public bool wishJump;

		public bool crouching;

		public bool sprinting;

		public float slopeLimit = 45f;

		public float rigidbodyPushForce = 1f;

		public float defaultHeight = 2f;

		public float crouchingHeight = 1f;

		public float crouchingSpeed = 10f;

		public bool toggleCrouch;

		public bool slidingEnabled;

		public bool laddersEnabled;

		public bool angledLaddersEnabled;

		public bool climbingLadder;

		public Vector3 ladderNormal = Vector3.zero;

		public Vector3 ladderDirection = Vector3.forward;

		public Vector3 ladderClimbDir = Vector3.up;

		public Vector3 ladderVelocity = Vector3.zero;

		public bool underwater;

		public bool cameraUnderwater;

		public bool grounded;

		public bool groundedTemp;

		public float fallingVelocity;

		public bool useStepOffset;

		public float stepOffset;
	}
}
