using NUnit.Framework;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npc_prefab;

    [SerializeField]
    private int num_npcs = 5;

    private GameObject[] npcs;

    private Vector3 position = new Vector3(-9, 0.5f, 0 );


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcs = new GameObject[num_npcs];

        for (int i = 0; i < num_npcs; i++)
        {
            npcs[i] = Instantiate(npc_prefab, position, Quaternion.identity);

            position.x += 2;
        }
    }
}
