using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]

public class SceneInfo : ScriptableObject
{
    public Vector2 playerPoint;
    public Vector2 skele1Point;
    public Vector2 skele2Point;

    public bool skele1Dead;
    public bool skele2Dead;

    private void OnEnable()
    {
        playerPoint.x = 0;
        playerPoint.y = 0;

        skele1Point.x = 5;
        skele1Point.y = 0;

        skele2Point.x = 0;
        skele2Point.y = -3;

        skele1Dead = false;
        skele2Dead = false;
    }
}


