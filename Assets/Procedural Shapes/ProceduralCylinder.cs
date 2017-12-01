using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCylinder : ProceduralShape
{
	public PivotPosition pivotPosition;
	public bool smoothNormals;
	public float height = 1;
	public float radius = 1;
	public int segments = 2;
	public int sectors = 8;

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

		var meshBuilder = smoothNormals
			? MeshBuilder.CreateConeSmooth(height, radius, radius, segments, sectors, false)
			: MeshBuilder.CreateConeFlat(height, radius, radius, segments, sectors, false);

		if (meshBuilder.vertices.Count <= 65535)
		{
			switch (pivotPosition)
			{
				case PivotPosition.Buttom:
					break;

				case PivotPosition.Center:
					meshBuilder.Transform(new Vector3(0, -height / 2, 0), Quaternion.identity);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			mesh = meshBuilder.ToMesh(mesh);
			if (mesh.name == string.Empty)
			{
				mesh.name = "Cylinder";
			}
		}

		return mesh;
	}
}