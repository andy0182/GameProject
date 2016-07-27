using UnityEngine;
using System.Collections;

namespace AnimFollow
{
	public partial class SimpleFootIK_AF
	{
		void ShootIKRays()
		{		
			// Shoot ray to determine where the feet should be placed.
			if (!Physics.Raycast(rightFoot.position + Vector3.up * maxStepHeight, Vector3.down, out raycastHitRightFoot, raycastLength, layerMask))
			{
				if (!Physics.Raycast(rightFoot.position + Vector3.up * 2f, Vector3.down, out raycastHitRightFoot, raycastLength * 4f, layerMask))
				{
					raycastHitRightFoot.normal = Vector3.up;
					raycastHitRightFoot.point = new Vector3(rightFoot.position.x, transform.position.y - .01f, rightFoot.position.z);
				}
			}
			footForward = rightToe.position - rightFoot.position;
			footForward = new Vector3(footForward.x, 0f, footForward.z);
			footForward = Quaternion.FromToRotation(Vector3.up, raycastHitRightFoot.normal) * footForward;
			if (!Physics.Raycast(rightFoot.position + footForward + Vector3.up * maxStepHeight, Vector3.down, out raycastHitToe, maxStepHeight * 2f, layerMask))
			{
				raycastHitToe.normal = raycastHitRightFoot.normal;
				raycastHitToe.point = raycastHitRightFoot.point + footForward;
			}
			else
			{		
				if(raycastHitRightFoot.point.y < raycastHitToe.point.y - footForward.y)
					raycastHitRightFoot.point = new Vector3(raycastHitRightFoot.point.x, raycastHitToe.point.y - footForward.y, raycastHitRightFoot.point.z);
				
				// Put avgNormal in foot normal
				raycastHitRightFoot.normal = (raycastHitRightFoot.normal + raycastHitToe.normal).normalized;
			}
			
			if (!Physics.Raycast(leftFoot.position + Vector3.up * maxStepHeight, Vector3.down, out raycastHitLeftFoot, raycastLength, layerMask))
			{
				if (!Physics.Raycast(leftFoot.position + Vector3.up * 2f, Vector3.down, out raycastHitLeftFoot, raycastLength * 4f, layerMask))
				{
					raycastHitLeftFoot.normal = Vector3.up;		
					raycastHitLeftFoot.point = new Vector3(leftFoot.position.x, transform.position.y - .01f, leftFoot.position.z);
				}
			}
			footForward = leftToe.position - leftFoot.position;
			footForward = new Vector3(footForward.x, 0f, footForward.z);
			footForward = Quaternion.FromToRotation(Vector3.up, raycastHitLeftFoot.normal) * footForward;
			if (!Physics.Raycast(leftFoot.position + footForward + Vector3.up * maxStepHeight, Vector3.down, out raycastHitToe, maxStepHeight * 2f, layerMask))
			{
				raycastHitToe.normal = raycastHitLeftFoot.normal;
				raycastHitToe.point = raycastHitLeftFoot.point + footForward;
			}
			else
			{
				if(raycastHitLeftFoot.point.y < raycastHitToe.point.y - footForward.y)
					raycastHitLeftFoot.point = new Vector3(raycastHitLeftFoot.point.x, raycastHitToe.point.y - footForward.y, raycastHitLeftFoot.point.z);

				// Put avgNormal in foot normal
				raycastHitLeftFoot.normal = (raycastHitLeftFoot.normal + raycastHitToe.normal).normalized;
			}

			// Do not tilt feet if on to steep an angle
			if (Vector3.Dot(raycastHitLeftFoot.normal, Vector3.up) < 1f - maxIncline)
			{
				raycastHitLeftFoot.normal = Vector3.RotateTowards(Vector3.up, raycastHitLeftFoot.normal, Mathf.Asin(maxIncline), 0f);
			}
			if (Vector3.Dot(raycastHitRightFoot.normal, Vector3.up) < 1f - maxIncline)
			{
				raycastHitRightFoot.normal = Vector3.RotateTowards(Vector3.up, raycastHitRightFoot.normal, Mathf.Asin(maxIncline), 0f);
			}

			if (followTerrain)
			{
				transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, Mathf.Min(raycastHitLeftFoot.point.y, raycastHitRightFoot.point.y), transformYLerp * extraYLerp * deltaTime), transform.position.z);
			}
		}
	}
}
