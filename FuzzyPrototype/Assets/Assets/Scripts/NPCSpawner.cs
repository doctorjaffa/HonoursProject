using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npc_prefab;

    [SerializeField]
    private int num_npcs = 5;

    private GameObject[] npcs;

    private Vector3 position = new Vector3(-9, 0.5f, 0 );

    // Create a list to store all possible NPC spawn points
    private List<SpawnPoint> spawn_points = new List<SpawnPoint>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add all of the spawn points to the list
        spawn_points.AddRange(FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None));
    
        npcs = new GameObject[num_npcs];

        // For each NPC, find a random free spot and spawn the NPC 
        for (int i = 0; i < num_npcs; i++)
        {
            SpawnPoint spot = GetFreeSpot();
            if (spot != null)
            {
                // The spot is now occupied
                spot.is_occupied = true;
                npcs[i] = Instantiate(npc_prefab, spot.transform.position, Quaternion.identity);
            }
        }
    }

    // Find all spawn points that are not occupied, then pick a random one 
    public SpawnPoint GetFreeSpot()
    {
        List<SpawnPoint> free_spots = spawn_points.FindAll(s => !s.is_occupied);

        // If there are no free spots left, return null
        if (free_spots.Count == 0)
            return null;

        return free_spots[Random.Range(0, free_spots.Count)];
    }
}
