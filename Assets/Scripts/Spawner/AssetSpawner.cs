using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSpawner : MonoBehaviour
{
   public void spawn_assets(Vector3 room_center, Vector2 room_dimensions)
   {
        GetComponent<AssetSpawn1>().PositionAssets(room_center, room_dimensions);
   }
}
