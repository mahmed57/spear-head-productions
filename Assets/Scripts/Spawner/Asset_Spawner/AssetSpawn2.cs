using UnityEngine;
using System; // Ensure this is at the top for System.Math

public class AssetSpawn2 : MonoBehaviour
{
    Vector2 room_dimensions = new Vector2(0, 0);
    
    public void PositionAssets(Vector3 roomCenter, Vector2 roomDimensions)
    {
        room_dimensions = roomDimensions; // Assign room dimensions for scaling calculations
        
        GameObject asset_placement_3 = Resources.Load<GameObject>("Prefabs/LevelAssets/Asset_Placement_3");

        if (asset_placement_3 != null)
        {
            GameObject placed_asset_3 = Instantiate(asset_placement_3, new Vector3(roomCenter.x, roomCenter.y, 0), Quaternion.identity);

            placed_asset_3.transform.localScale = new Vector3(
                placed_asset_3.transform.localScale.x * scale_x(placed_asset_3.transform.localScale.x), 
                placed_asset_3.transform.localScale.y * scale_y(placed_asset_3.transform.localScale.y), 
                1
            );
        }
        else
        {
            Debug.LogWarning("Asset_Placement_3 prefab not found in Resources.");
        }
    }
    
    public float scale_x(float input_x)
    {
        return (float)System.Math.Round((input_x / 60.0f) * room_dimensions.x, 2);
    }

    public float scale_y(float input_y)
    {
        return (float)System.Math.Round((input_y / 60.0f) * room_dimensions.y, 2);
    }
}
