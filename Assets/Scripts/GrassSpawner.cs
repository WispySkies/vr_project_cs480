using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grassPrefab; // Assign your grass model here
    public RenderTexture maskRenderTexture; // Assign your mask render texture
    public int grassDensity = 1000; // Number of grass objects to spawn
    public Vector2 mapSize = new Vector2(250, 250); // Size of your floor plane
    public Transform floorTransform; // The floor object for position reference

    // Floor's position in the scene
    private Vector3 floorPosition;

    void Start()
    {
        floorPosition = floorTransform.position; // Store the floor's world position
        SpawnGrass();
    }

    void SpawnGrass()
    {
        // Create a Texture2D to read the mask
        RenderTexture.active = maskRenderTexture;
        Texture2D maskTexture = new Texture2D(maskRenderTexture.width, maskRenderTexture.height, TextureFormat.RGB24, false);
        maskTexture.ReadPixels(new Rect(0, 0, maskRenderTexture.width, maskRenderTexture.height), 0, 0);
        maskTexture.Apply();
        RenderTexture.active = null;

        for (int i = 0; i < grassDensity; i++)
        {
            // Random position within map bounds, adjusted for floor position
            float xPos = Random.Range(-mapSize.x / 2, mapSize.x / 2);
            float zPos = Random.Range(-mapSize.y / 2, mapSize.y / 2);
            Vector3 position = new Vector3(xPos, 0, zPos) + floorPosition; // Add floor position offset

            // Convert world position to mask texture UV coordinates
            float u = (xPos + mapSize.x / 2) / mapSize.x;
            float v = (zPos + mapSize.y / 2) / mapSize.y;

            // Get pixel color from the mask (check if the grass can be placed here)
            Color maskColor = maskTexture.GetPixelBilinear(u, v);

            // Spawn grass only if the mask allows it (based on color threshold)
            if (maskColor.r > 0.5f) // Adjust this threshold if necessary
            {
                Instantiate(grassPrefab, position, Quaternion.identity, transform);
            }
        }

        Destroy(maskTexture); // Clean up
    }
}
