using UnityEngine;

internal class UserInput : MonoBehaviour
{
	public float sensitivity = 1;

	private void Update()
	{
		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		transform.Translate(new Vector3(dx, dy, 0) * sensitivity);
	}
}