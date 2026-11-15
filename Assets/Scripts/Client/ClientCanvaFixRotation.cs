namespace Client
{
	using UnityEngine;

	public class ClientCanvaFixRotation : MonoBehaviour
	{
		private Quaternion initialRotation;

		void Start()
		{
			initialRotation = transform.rotation;
		}

		void LateUpdate()
		{
			transform.rotation = initialRotation;
		}
	}
}