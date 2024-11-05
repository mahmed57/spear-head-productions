using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSpawn4 : MonoBehaviour
{
    Vector2 room_dimensions = new Vector2(0, 0);
    public void PositionAssets(Vector3 roomCenter, Vector2 roomDimensions)
    {
        room_dimensions = roomDimensions;
        // Load assets from Resources folder
        GameObject candle1 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle1");
        GameObject candle2 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle2");
        GameObject candle3 = Resources.Load<GameObject>("Prefabs/LevelAssets/candle_3");
        GameObject graveStone = Resources.Load<GameObject>("Prefabs/LevelAssets/grave_stone");
        GameObject skull = Resources.Load<GameObject>("Prefabs/LevelAssets/skull");
        GameObject spikeTrap = Resources.Load<GameObject>("Prefabs/LevelAssets/spike_trap");
        GameObject statue = Resources.Load<GameObject>("Prefabs/LevelAssets/statue");
        GameObject statueKnight = Resources.Load<GameObject>("Prefabs/LevelAssets/statue_knight");

        //
        Instantiate(graveStone, new Vector3(roomCenter.x + scale_x(23.9f), roomCenter.y - scale_y(16.89f), 0), Quaternion.identity);

        // Place the spike trap 
        Instantiate(spikeTrap, new Vector3(roomCenter.x, roomCenter.y + scale_y(21.9f), 0), Quaternion.identity);

        // Position statues in the center left and right sides
        Instantiate(statueKnight, new Vector3(roomCenter.x + scale_x(15.2f), roomCenter.y + scale_y(24f), 0), Quaternion.identity);
        Instantiate(statue, new Vector3(roomCenter.x - scale_x(15.2f), roomCenter.y + scale_y(24f), 0), Quaternion.Euler(0, 180, 0));

        // Position skulls clockwise
        Instantiate(skull, new Vector3(roomCenter.x - scale_x(25.79f), roomCenter.y +  scale_y(26.99f), 0), Quaternion.Euler(0, 180, 0));
        Instantiate(skull, new Vector3(roomCenter.x - scale_x(12.59f), roomCenter.y -  scale_y(26.3f), 0), Quaternion.identity);
        Instantiate(skull, new Vector3(roomCenter.x + scale_x(25.29f), roomCenter.y -  scale_y(18.9f), 0), Quaternion.identity);
        Instantiate(skull, new Vector3(roomCenter.x -scale_x(12.59f), roomCenter.y -  scale_y(26.3f), 0), Quaternion.Euler(0, 180, 0));

        // Place candles clockwise
        
        Instantiate(candle2, new Vector3(roomCenter.x - scale_x(25.5f), roomCenter.y + scale_y(29f), 0), Quaternion.identity);
        Instantiate(candle2, new Vector3(roomCenter.x + scale_x(12.9f), roomCenter.y -  scale_y(25.2f), 0), Quaternion.Euler(0, 180, 0));
        Instantiate(candle2, new Vector3(roomCenter.x - scale_x(27.5f), roomCenter.y -  scale_y(26f), 0), Quaternion.Euler(0, 180, 0));

        Instantiate(candle3, new Vector3(roomCenter.x - scale_x(8.59f), roomCenter.y + scale_y(30.5f), 0), Quaternion.identity);
        Instantiate(candle3, new Vector3(roomCenter.x + scale_x(15.4f), roomCenter.y - scale_y(22.9f), 0), Quaternion.identity);
        Instantiate(candle3, new Vector3(roomCenter.x - scale_x(14.29f), roomCenter.y - scale_y(24.7f), 0), Quaternion.identity);
        Instantiate(candle1, new Vector3(roomCenter.x + scale_x(26.5f), roomCenter.y +  scale_y(30.1f), 0), Quaternion.identity);
        
    

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
