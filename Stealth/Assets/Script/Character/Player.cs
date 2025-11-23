using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	[
		RequireComponent(typeof(CharacterController)),
		RequireComponent(typeof(PlayerInput))
	]
	public class Player : Character
	{
		protected void OnMove(InputValue value)
		{
			var raw = value.Get<Vector2>();
			bufferMovementInput = new Vector3(raw.x, 0, raw.y);
		}

		protected void OnLook(InputValue value)
		{
			var raw = value.Get<Vector2>();
			raw *= 360f * orientSpeed / Screen.width;

			Azimuth = Azimuth + raw.x;
			float zenith = Zenith + raw.y;
			if(zenith < 0)
				zenith += 360;
			if(zenith < 180)
				zenith = Mathf.Clamp(zenith, 0, 90);
			else
				zenith = Mathf.Clamp(zenith, 270, 360);
			Zenith = zenith;
		}
	}
}
