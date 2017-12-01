using UnityEngine;

public partial class MeshBuilder
{
	public static MeshBuilder CreateSlope(float sizeX, float sizeY, float sizeZ, bool collisionMesh)
	{
		var leftSide = CreateSlopeSide(true, collisionMesh);
		leftSide.Transform(new Vector3(-sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		var rightSide = CreateSlopeSide(false, collisionMesh);
		rightSide.Transform(new Vector3(sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		var farSide = CreatePlane(sizeX, sizeY, 1, 1, collisionMesh);
		farSide.Transform(new Vector3(0, sizeY / 2, sizeZ), Quaternion.Euler(90, 0, 0));

		var bottom = CreatePlane(sizeX, sizeZ, 1, 1, collisionMesh);
		bottom.Transform(new Vector3(0, 0, sizeZ / 2), Quaternion.Euler(180, 0, 0));

		var slope = CreateSlope(collisionMesh);
		slope.Transform(new Vector3(-sizeX / 2, 0, 0), Quaternion.identity, new Vector3(sizeX, sizeY, sizeZ));

		return Combine(leftSide, rightSide, farSide, bottom, slope);
	}

	private static MeshBuilder CreateSlopeSide(bool leftSide, bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;
		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;
		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(0, 0, 1));
		vertices.Add(new Vector3(0, 1, 1));

		if (collisionMesh == false)
		{
			if (leftSide)
			{
				normals.Add(Vector3.left);
				normals.Add(Vector3.left);
				normals.Add(Vector3.left);
			}
			else
			{
				normals.Add(Vector3.right);
				normals.Add(Vector3.right);
				normals.Add(Vector3.right);
			}

			uvs.Add(new Vector2(0, 0));
			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(1, 1));

			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
		}

		vertices.Add(new Vector3(0, 0, 1));
		if (collisionMesh == false)
		{
			if (leftSide)
			{
				normals.Add(Vector3.left);
			}
			else
			{
				normals.Add(Vector3.right);
			}
			uvs.Add(Vector2.zero);
			colors.Add(color);
		}

		if (leftSide)
		{
			indices.Add(0);
			indices.Add(1);
			indices.Add(2);
		}
		else
		{
			indices.Add(0);
			indices.Add(2);
			indices.Add(1);
		}

		return meshBuilder;
	}

	private static MeshBuilder CreateSlope(bool collisionMesh)
	{
		var meshBuilder = new MeshBuilder(collisionMesh);
		var vertices = meshBuilder.vertices;
		var indices = meshBuilder.indices;
		var normals = meshBuilder.normals;
		var uvs = meshBuilder.uvs;
		var colors = meshBuilder.colors;

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(1, 0, 0));
		vertices.Add(new Vector3(1, 1, 1));
		vertices.Add(new Vector3(0, 1, 1));

		if (collisionMesh == false)
		{
			normals.Add(Vector3.up);
			normals.Add(Vector3.up);
			normals.Add(Vector3.up);
			normals.Add(Vector3.up);

			uvs.Add(new Vector2(0, 0));
			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(1, 1));
			uvs.Add(new Vector2(0, 1));

			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
		}

		indices.Add(0);
		indices.Add(2);
		indices.Add(1);
		indices.Add(0);
		indices.Add(3);
		indices.Add(2);

		return meshBuilder;
	}
}
