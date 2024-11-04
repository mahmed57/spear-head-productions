using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public int number_of_enemies_killed = 0;

    public int crystal_count = 0;

    private GameObject[] rooms;


    public Vector3 start_pos = new Vector3(0, 0, 0);

    public void increment_enemy_counter(GameObject enemy)
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");

        foreach(GameObject room in rooms)
        {
            RoomController room_controller = room.GetComponent<RoomController>();
            
            if((room_controller.hasSpawnedEnemies == true) && (room_controller.enemies.Count > 0))
            {
                room_controller.enemies.Remove(enemy);
                
                if(room_controller.enemies.Count == 0)
                {
                    crystal_count += 4;
                }

                break;
            }
        }


        number_of_enemies_killed++;

    }



}
