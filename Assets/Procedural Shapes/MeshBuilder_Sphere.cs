public partial class MeshBuilder
{
	public static MeshBuilder CreateSphericalCube(float radius, int segments, bool collisionMesh)
	{
		var meshBuilder = CreateBox(1, 1, 1, segments, segments, segments, collisionMesh);

		var vertices = meshBuilder.vertices;
		var normals = meshBuilder.normals;

		for (int i = 0; i < vertices.Count; i++)
		{
			var normal = vertices[i].normalized;
			vertices[i] = normal * radius * .5f;
			normals[i] = normal;
		}

		return meshBuilder;
	}
}