using UnityEngine;

internal class Rotation : MonoBehaviour
{
	public Vector3 center;
	public float radius;
	public float angularSpeed;

	private float angle;

	private void Update()
	{
		angle += angularSpeed * Time.deltaTime;

		float dx = radius * Mathf.Cos(angle);
		float dy = radius * Mathf.Sin(angle);
		var position = center + new Vector3(dx, dy, 0);

		transform.position = position;
	}
}