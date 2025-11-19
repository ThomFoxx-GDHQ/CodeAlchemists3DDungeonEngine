using UnityEngine;

public class ProtoTypeEnemyBehavior : MonoBehaviour
{
    private char[,] _grid;
    private float _tileSize;
    private Vector2Int _gridPOS = Vector2Int.zero;

    private void Start()
    {
        var dungeon = FindFirstObjectByType<DungeonLayoutGenerator>();
        _grid = dungeon.Grid;
        _tileSize = dungeon.TileSize;
        GetGridPosition();
    }

    //Get our Grid Position
    private void GetGridPosition()
    {
        Vector3 posV3 = PositionHelper.EvenOutPosition(transform.position);
        Vector2 pos = new Vector2(posV3.x, posV3.z);
        _gridPOS.x = (int)(pos.x / _tileSize);
        _gridPOS.y = (int)(pos.y / _tileSize);
        Debug.Log($"GridPosition for {transform.position} is {_gridPOS}.");
    }


    //Check Neighbors
    //Pick Random Valid Neighbor
    //Move to Picked Neighbor

}
