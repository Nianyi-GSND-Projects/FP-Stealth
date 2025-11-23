using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Nianyi.UnityPack
{
	public class RaySelector : Selector
	{
		public bool useMaxDistance = true;
		[HideWhen(nameof(useMaxDistance), false), Min(0)] public float distance = 10f;
		public Vector3 direction = Vector3.forward;
		public bool passThrough = false;

		public override IEnumerable<GameObject> GetFocusedCandidates()
		{
			foreach(var hit in GetRaycastHits())
			{
				if(hit.collider.gameObject.TryGetComponent<IFocusable>(out _))
					yield return hit.collider.gameObject;
			}
		}

		public override IEnumerable<GameObject> GetSelectedCandidates()
		{
			foreach(var hit in GetRaycastHits())
			{
				if(hit.collider.gameObject.TryGetComponent<ISelectable>(out _))
					yield return hit.collider.gameObject;
			}
		}

		RaycastHit[] GetRaycastHits()
		{
			Vector3 direction = transform.TransformDirection(this.direction).normalized;

			RaycastHit[] hits;
			if(passThrough)
			{
				hits = Physics.RaycastAll(
					transform.position,
					direction,
					useMaxDistance ? distance : Mathf.Infinity,
					layerMask
				);
			}
			else
			{
				bool hasHit = Physics.Raycast(
					transform.position,
					direction,
					out var hit,
					useMaxDistance ? distance : Mathf.Infinity,
					layerMask
				);
				if(hasHit)
					hits = new RaycastHit[] { hit };
				else
					hits = new RaycastHit[0];
			}
			return hits;
		}
	}
}
