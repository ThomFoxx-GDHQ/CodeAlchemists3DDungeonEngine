using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProtoTypeEnemyBehavior : MonoBehaviour
{
    private char[,] _grid;
    private float _tileSize;
    private Vector2Int _gridPOS = Vector2Int.zero;
    private List<Vector2Int> _neighbors = new List<Vector2Int>();
    private Vector2Int _target;
    private Vector3 _moveDirection;
    private bool _isMoving = false;
    private Vector3 _startPOS;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _step = 1f;
    private PartyController _partyController;

    private void Start()
    {
        var dungeon = FindFirstObjectByType<DungeonLayoutGenerator>();
        _partyController = FindFirstObjectByType<PartyController>();
        _grid = dungeon.Grid;
        _tileSize = dungeon.TileSize;
        _gridPOS = GetGridPosition(transform.position);
        _startPOS = transform.position;
    }

    private void Update()
    {
        if (_isMoving) return;

        _gridPOS = GetGridPosition(transform.position);
        CheckNeighbors();
        PickRandomDirection();
        CalculateMovement();
        
    }

    //Get our Grid Position
    private Vector2Int GetGridPosition(Vector3 position)
    {
        Vector3 posV3 = PositionHelper.EvenOutPosition(position);
        Vector2 pos = new Vector2(posV3.x, posV3.z);
        Vector2Int newPOS = Vector2Int.zero;
        newPOS.x = (int)(pos.x / _tileSize);
        newPOS.y = (int)(pos.y / _tileSize);
        return newPOS;
    }


    //Check Neighbors
    private void CheckNeighbors()
    {
        _neighbors.Clear();

        Vector2Int checkingDirection = _gridPOS;

        if (Vector3.Distance(_partyController.transform.position, transform.position) <= 5)
        {
            _neighbors.Add(GetGridPosition(_partyController.transform.position));
        }
        else
        {
            //== North Neighbor ==//
            checkingDirection.y = _gridPOS.y + 1;
            if (checkingDirection.y < _grid.GetLength(1))
                if (_grid[checkingDirection.x, checkingDirection.y] == 'R')
                    _neighbors.Add(checkingDirection);
            //== South Neighbor ==//
            checkingDirection.y = _gridPOS.y - 1;
            if (checkingDirection.y >= 0)
                if (_grid[checkingDirection.x, checkingDirection.y] == 'R')
                    _neighbors.Add(checkingDirection);

            //== Reset for X movement ==//
            checkingDirection = _gridPOS;

            //== West Neighbor ==//
            checkingDirection.x = _gridPOS.x + 1;
            if (checkingDirection.x < _grid.GetLength(0))
                if (_grid[checkingDirection.x, checkingDirection.y] == 'R')
                    _neighbors.Add(checkingDirection);
            //== East Neighbor ==//
            checkingDirection.x = _gridPOS.x - 1;
            if (checkingDirection.x >= 0)
                if (_grid[checkingDirection.x, checkingDirection.y] == 'R')
                    _neighbors.Add(checkingDirection);
        }
    }

    //Pick Random Valid Neighbor
    private void PickRandomDirection()
    {
        if (_neighbors.Count == 0) return;

        int RNG = Random.Range(0, _neighbors.Count);
        _target = _neighbors[RNG];
        Debug.Log($"{RNG} : {_target}");
    }


    //Move to Picked Neighbor
    private void CalculateMovement()
    {
        _moveDirection = Vector3.zero;
        var direction = _target - _gridPOS;
        switch (direction)
        {
            case Vector2Int y when direction.y >= 1:
                //transform.Translate(Vector3.forward * _gridTileSize);
                _moveDirection = transform.forward * _tileSize;
                break;
            case Vector2Int y when direction.y <=-1:
                _moveDirection = (transform.forward * -1) * _tileSize;
                break;
            case Vector2Int x when direction.x >= 1:
                _moveDirection = transform.right * _tileSize;
                break;
            case Vector2Int x when direction.x <= -1:
                _moveDirection = (transform.right * -1) * _tileSize;
                break;
            default:
                _moveDirection = Vector3.zero;
                break;
        }
        Debug.Log(_moveDirection);
        StartCoroutine(MoveRoutine(_moveDirection));
    }

    IEnumerator MoveRoutine(Vector3 direction)
    {
        _isMoving = true;
        _startPOS = PositionHelper.EvenOutPosition(transform.position);
        var targetPOS = _startPOS + direction;
        float step;
        while (transform.position != targetPOS)
        {
            yield return null;
            step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPOS, step);
        }
        _startPOS = PositionHelper.EvenOutPosition(transform.position);
        yield return new WaitForSeconds(_step);
        _isMoving = false;
    }
}
