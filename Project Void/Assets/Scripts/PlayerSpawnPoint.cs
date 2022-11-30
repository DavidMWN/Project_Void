using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject player;

    public SceneInfo sceneInfo;

    void Start()
    {
        SetSpawnPoint(sceneInfo.playerPoint);
                
        SpawnPlayer();
    }

    public void SetSpawnPoint(Vector2 point)
    {
        transform.position = point;
    }

    private void SpawnPlayer()
    {
        Instantiate(player, transform.position, Quaternion.identity);
    }
}
