using UnityEngine;

public class AssetSpawn1 : MonoBehaviour
{
    Vector2 room_dimensions = new Vector2(0, 0);
    public void PositionAssets(Vector3 roomCenter, Vector2 roomDimensions)
    {
        room_dimensions = roomDimensions;
        // Load assets from Resources folder
        GameObject candle1 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle1");
        GameObject candle2 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle2");
        GameObject candle3 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle_3");
     
        GameObject skull = Resources.Load<GameObject>("Prefabs/LevelAssets/skull");
        GameObject spikeTrap = Resources.Load<GameObject>("Prefabs/LevelAssets/spike_trap");

        Instantiate(spikeTrap, new Vector3(roomCenter.x, roomCenter.y, 0), Quaternion.identity);

        // Position skulls at the four corners
        Instantiate(skull, new Vector3(roomCenter.x - scale_x(24.9f), roomCenter.y +  scale_y(25.8f), 0), Quaternion.Euler(0, 180, 0));
        Instantiate(skull, new Vector3(roomCenter.x + scale_x(21.9f), roomCenter.y +  scale_y(21.9f), 0), Quaternion.identity);
        Instantiate(skull, new Vector3(roomCenter.x + scale_x(23.4f), roomCenter.y -  scale_y(22.5f), 0), Quaternion.identity);
        Instantiate(skull, new Vector3(roomCenter.x -scale_x(22.4f), roomCenter.y -  scale_y(22.7f), 0), Quaternion.Euler(0, 180, 0));

        // Place candle sets around the spike trap
        
        Instantiate(candle2, new Vector3(roomCenter.x - scale_x(6.18f), roomCenter.y -  scale_y(1.11f), 0), Quaternion.Euler(0, 180, 0));
        Instantiate(candle2, new Vector3(roomCenter.x + scale_x(6.5f), roomCenter.y +  scale_y(1.29f), 0), Quaternion.Euler(0, 180, 0));

        Instantiate(candle3, new Vector3(roomCenter.x - scale_x(7.6f), roomCenter.y +  scale_y(1.2f), 0), Quaternion.identity);
        Instantiate(candle3, new Vector3(roomCenter.x + scale_x(8.7f), roomCenter.y +  scale_y(1.9f), 0), Quaternion.identity);
        
        Instantiate(candle1, new Vector3(roomCenter.x + scale_x(7.53f), roomCenter.y +  scale_y(0.44f), 0), Quaternion.identity);
        
        // Position other candles in the upper corners of the room
        Instantiate(candle3, new Vector3(roomCenter.x - scale_x(9.7f), roomCenter.y +  scale_y(30.5f), 0), Quaternion.identity);
        Instantiate(candle1, new Vector3(roomCenter.x + scale_x(22.7f), roomCenter.y +  scale_y(21f), 0), Quaternion.identity);
        Instantiate(candle1, new Vector3(roomCenter.x + scale_x(0.9f), roomCenter.y +  scale_y(30.3f), 0), Quaternion.identity);
        Instantiate(candle2, new Vector3(roomCenter.x - scale_x(27.5f), roomCenter.y -  scale_y(9f), 0), Quaternion.identity);
        Instantiate(candle2, new Vector3(roomCenter.x - scale_x(27.5f), roomCenter.y -  scale_y(7f), 0), Quaternion.Euler(0, 180, 0));

    }

    public float scale_x(float input)
    {
        return((input/60)*room_dimensions.x);
    }

    
    public float scale_y(float input)
    {
        return((input/60)*room_dimensions.y);
    }
}
