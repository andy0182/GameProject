using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace AnimFollow
{
	public class ragdollHitByBullet_AF : UnityEngine.MonoBehaviour
	{
		// This script should be added to the root containing the ragdoll

		RagdollControl_AF ragdollControl;

		public ParticleSystem blood;
		public ParticleSystem bloodClone;
		bool userNeedsToFixStuff = false;
		void Awake ()
		{
			if (!blood)
			{
				Debug.LogWarning("You need to assign blood prefab in the ragdollHitByBullet script on " + this.name);
				userNeedsToFixStuff = true;
			}
			if (!(ragdollControl = GetComponentInChildren<RagdollControl_AF>()))
			{
				Debug.LogWarning("The ragdollHitByBullet script on " + this.name + " requires a RagdollControl script to work");
				userNeedsToFixStuff = true;
			}
		}
		void OnApplicationQuit()
		{
			ServerObject.Close ();
		}
		void Update()
		{
		}
		void HitByBullet (BulletHitInfo_AF bulletHitInfo)
		{
//			if (userNeedsToFixStuff)
//				return;

			bloodClone = Instantiate(blood, bulletHitInfo.hitPoint, Quaternion.LookRotation(bulletHitInfo.hitNormal)) as ParticleSystem;
			//StartCoroutine (ragdollControl.Falling ());
			//ragdollControl.shotByBullet = true;
			bloodClone.transform.parent = bulletHitInfo.hitTransform;
			bloodClone.Play();
			Destroy(bloodClone.gameObject, 1f);

			StartCoroutine(AddForceToLimb(bulletHitInfo));
		}

		IEnumerator AddForceToLimb (BulletHitInfo_AF bulletHitInfo)
		{
			yield return new WaitForFixedUpdate();
			Rigidbody rigidbody=GetComponentInChildren<Rigidbody> ();
			rigidbody.AddForce(bulletHitInfo.bulletForce);
			//bulletHitInfo.hitTransform.rigidbody.AddForceAtPosition(bulletHitInfo.bulletForce, bulletHitInfo.hitPoint);
			//bulletHitInfo.hitTransform.rigidbody.AddRelativeForce (bulletHitInfo.bulletForce);

		}
	}
}
