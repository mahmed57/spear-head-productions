using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSpawner : MonoBehaviour
{
   public void spawn_assets(Vector3 room_center, Vector2 room_dimensions, int room_design)
   {
      
         if(room_design == 1)
         {
            GetComponent<AssetSpawn1>().PositionAssets(room_center, room_dimensions);
         }

         
         else if(room_design == 2)
         {
            GetComponent<AssetSpawn2>().PositionAssets(room_center, room_dimensions);
         }

         else if(room_design == 3)
         {
            GetComponent<AssetSpawn3>().PositionAssets(room_center, room_dimensions);
         }

         else
         {
               GetComponent<AssetSpawn4>().PositionAssets(room_center, room_dimensions);
         }
        
   }
}
