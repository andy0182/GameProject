using UnityEngine;
using System.Collections;

namespace AnimFollow
{
	public partial class SimpleFootIK_AF
	{
		void PositionFeet()
		{
			// Save before PositionFeet
			leftFootRotation = leftFoot.rotation;
			rightFootRotation = rightFoot.rotation;

			// Here goes the maths			
			leftFootTargetNormal = Vector3.Lerp(Vector3.up, raycastHitLeftFoot.normal, footIKWeight);
			leftFootTargetNormal = Vector3.Lerp(lastLeftFootTargetNormal, leftFootTargetNormal, footNormalLerp * deltaTime);
			lastLeftFootTargetNormal = leftFootTargetNormal;
			rightFootTargetNormal = Vector3.Lerp(Vector3.up, raycastHitRightFoot.normal, footIKWeight);
			rightFootTargetNormal = Vector3.Lerp(lastRightFootTargetNormal, rightFootTargetNormal, footNormalLerp * deltaTime);
			lastRightFootTargetNormal = rightFootTargetNormal;

			leftFootTargetPos = raycastHitLeftFoot.point + leftFootTargetNormal * footHeight + (leftFoot.position.y - footHeight - transform.position.y) * Vector3.up;
			leftFootTargetPos = Vector3.Lerp(leftFoot.position, leftFootTargetPos, footIKWeight);
			leftFootTargetPos = Vector3.Lerp(lastLeftFootTargetPos, leftFootTargetPos, footTargetLerp * deltaTime);
			lastLeftFootTargetPos = leftFootTargetPos;
			
			rightFootTargetPos = raycastHitRightFoot.point + rightFootTargetNormal * footHeight + (rightFoot.position.y - footHeight - transform.position.y) * Vector3.up;
			rightFootTargetPos = Vector3.Lerp(rightFoot.position, rightFootTargetPos, footIKWeight);
			rightFootTargetPos = Vector3.Lerp(lastRightFootTargetPos, rightFootTargetPos, footTargetLerp * deltaTime);
			lastRightFootTargetPos = rightFootTargetPos;


			leftLegTargetLength = Mathf.Min((leftFootTargetPos - leftThigh.position).magnitude, calfLength + thighLength - .01f);
			leftLegTargetLength = Mathf.Max(leftLegTargetLength, .2f);
			leftKneeAngle = Mathf.Acos((Mathf.Pow(leftLegTargetLength, 2f) - calfLengthSquared - thighLengthSquared) * reciDenominator);
			leftKneeAngle *= Mathf.Rad2Deg;
			leftCalf.Rotate(0f, 0f, 180f - leftKneeAngle - leftCalf.localEulerAngles.z);
			leftThigh.rotation = Quaternion.FromToRotation(leftFoot.position - leftThigh.position, leftFootTargetPos - leftThigh.position) * leftThigh.rotation;

			rightLegTargetLength = Mathf.Min((rightFootTargetPos - rightThigh.position).magnitude, calfLength + thighLength - .01f);
			rightLegTargetLength = Mathf.Max(rightLegTargetLength, .2f);
			rightKneeAngle = Mathf.Acos((Mathf.Pow(rightLegTargetLength, 2f) - calfLengthSquared - thighLengthSquared) * reciDenominator);
			rightKneeAngle *= Mathf.Rad2Deg;
			rightCalf.Rotate(0f, 0f, 180f - rightKneeAngle - rightCalf.localEulerAngles.z);
			rightThigh.rotation = Quaternion.FromToRotation(rightFoot.position - rightThigh.position, rightFootTargetPos - rightThigh.position) * rightThigh.rotation;
			
			leftFoot.rotation = Quaternion.FromToRotation(Vector3.up, leftFootTargetNormal) * leftFootRotation;
			rightFoot.rotation = Quaternion.FromToRotation(Vector3.up, rightFootTargetNormal) * rightFootRotation;
		}
	}
}
