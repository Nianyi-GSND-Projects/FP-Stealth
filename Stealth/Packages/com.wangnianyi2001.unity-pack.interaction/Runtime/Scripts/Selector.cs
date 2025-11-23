using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Nianyi.UnityPack
{
	public abstract class Selector : MonoBehaviour
	{
		#region Configurations
		public bool affectFocusables = true;
		public bool affectSelectables = true;

		public LayerMask layerMask = ~0;
		#endregion

		#region Unity life cycle
		protected void Update()
		{
			UpdateRegistry();
		}
		#endregion

		#region Registry
		readonly HashSet<GameObject> focused = new();
		readonly HashSet<GameObject> selected = new();

		public abstract IEnumerable<GameObject> GetFocusedCandidates();
		public abstract IEnumerable<GameObject> GetSelectedCandidates();

		public GameObject[] Focused => focused.ToArray();
		public GameObject[] Selected => selected.ToArray();

		void UpdateRegistry()
		{
			GameObject[] addedFocused = new GameObject[0], removedFocused = new GameObject[0];
			GameObject[] addedSelected = new GameObject[0], removedSelected = new GameObject[0];

			if(affectFocusables)
				UpdateRegistry(focused, GetFocusedCandidates(), out addedFocused, out removedFocused);
			if(affectSelectables)
				UpdateRegistry(selected, GetSelectedCandidates(), out addedSelected, out removedSelected);

			if(affectFocusables)
				foreach(var x in addedFocused) x.SendMessage(nameof(IFocusable.OnFocus), SendMessageOptions.DontRequireReceiver);
			if(affectSelectables)
				foreach(var x in addedSelected) x.SendMessage(nameof(ISelectable.OnSelect), SendMessageOptions.DontRequireReceiver);
			if(affectSelectables)
				foreach(var x in removedSelected) x.SendMessage(nameof(ISelectable.OnDeselect), SendMessageOptions.DontRequireReceiver);
			if(affectFocusables)
				foreach(var x in removedFocused) x.SendMessage(nameof(IFocusable.OnLoseFocus), SendMessageOptions.DontRequireReceiver);
		}

		void UpdateRegistry(ISet<GameObject> registry, IEnumerable<GameObject> updated, out GameObject[] added, out GameObject[] removed)
		{
			updated = new HashSet<GameObject>(updated);
			added = updated.Where(x => !registry.Contains(x)).ToArray();
			removed = registry.Where(x => !updated.Contains(x)).ToArray();
			foreach(var x in removed) registry.Remove(x);
			foreach(var x in added) registry.Add(x);
		}
		#endregion
	}
}
