using UnityEngine;

public partial class MeshBuilder
{
	public static MeshBuilder CreateBox(float sizeX, float sizeY, float sizeZ, int segmentsX, int segmentsY, int segmentsZ, bool collisionMesh)
	{
		var lower = CreatePlane(sizeX, sizeZ, segmentsX, segmentsZ, collisionMesh);
		lower.Transform(new Vector3(0, -sizeY / 2, 0), Quaternion.Euler(180, 0, 0));

		var upper = CreatePlane(sizeX, sizeZ, segmentsX, segmentsZ, collisionMesh);
		upper.Transform(new Vector3(0, sizeY / 2, 0), Quaternion.identity);

		var left = CreatePlane(sizeY, sizeZ, segmentsY, segmentsZ, collisionMesh);
		left.Transform(new Vector3(-sizeX / 2, 0, 0), Quaternion.Euler(0, 0, 90));

		var right = CreatePlane(sizeY, sizeZ, segmentsY, segmentsZ, collisionMesh);
		right.Transform(new Vector3(sizeX / 2, 0, 0), Quaternion.Euler(0, 0, -90));

		var back = CreatePlane(sizeX, sizeY, segmentsX, segmentsY, collisionMesh);
		back.Transform(new Vector3(0, 0, -sizeZ / 2), Quaternion.Euler(-90, 0, 0));

		var forward = CreatePlane(sizeX, sizeY, segmentsX, segmentsY, collisionMesh);
		forward.Transform(new Vector3(0, 0, sizeZ / 2), Quaternion.Euler(90, 0, 0));

		return Combine(lower, upper, left, right, back, forward);
	}
}