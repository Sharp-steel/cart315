using UnityEngine;

public class teleport : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public controlZoneAndPoints[] controlZones;
    public Transform player;
    
    public Transform[] enemies;
    public Transform[] teammates;
    public Transform[] arenas;

    public float spacing = 1.5f;
    private int currentIndex = -1;

    // Update is called once per frame
    void Update()
    {
        if (dayNightCycle == null) return;

        float mins = dayNightCycle.mins;
        int newIndex = Mathf.FloorToInt(mins/2.5f);
        newIndex = Mathf.Clamp(newIndex, 0, arenas.Length - 1);

        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            TeleportAll(currentIndex);
        }

        if (mins == 0f && currentIndex != 0)
        {
            currentIndex = 0;
            TeleportAll(0);
        }
    }

    void TeleportAll(int index)
    {
        for (int i = 0; i < controlZones.Length; i++)
        {
            if (controlZones[i] != null)
                controlZones[i].isActive = (i == index);
        }
        
        Transform arena = arenas[index];
        Transform allyAnchor = arena.Find("AllySpawn");
        Transform enemyAnchor = arena.Find("EnemySpawn");
        
        int totalAllies = teammates.Length + 1;
        
        if (player != null)
            player.position = GetPosition(allyAnchor.position, 0, totalAllies);
        
        for (int i = 0; i < teammates.Length; i++)
        {
            if (teammates[i] != null)
                teammates[i].position = GetPosition(allyAnchor.position, i + 1, totalAllies);
        }
        
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                enemies[i].position = GetPosition(enemyAnchor.position, i, enemies.Length);
        }
    }
    
    Vector3 GetPosition(Vector3 start, int index, int total)
    {
        float startY = (total - 1) * spacing / 2f;
        float yOffset = startY - (index * spacing);

        return start + new Vector3(0, yOffset, 0);
    }
}
