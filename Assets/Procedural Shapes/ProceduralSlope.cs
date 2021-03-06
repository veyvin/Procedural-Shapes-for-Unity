﻿using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSlope : ProceduralShape
{
	public float sizeX = 1;
	public float sizeY = 1;
	public float sizeZ = 1;

	protected override Mesh CreateMesh(Mesh mesh = null)
	{
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh.Clear();
		}

		var meshBuilder = MeshBuilder.CreateSlope(sizeX, sizeY, sizeZ, false);
		if (meshBuilder.vertices.Count <= 65535)
		{
			mesh = meshBuilder.ToMesh(mesh);
			if (mesh.name == string.Empty)
			{
				mesh.name = "Stairs";
			}
		}

		return mesh;
	}
}