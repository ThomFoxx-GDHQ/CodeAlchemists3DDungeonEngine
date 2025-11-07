using System.Collections;
using UnityEngine;

public class StairsBehavior : MonoBehaviour
{
    private enum StairDirection
    {
        Up,
        Down
    }

    [SerializeField] private StairDirection _direction;
    [SerializeField] private int _numberOfFloorsToChange = 1;
    [SerializeField] private bool _active = false;
    private int _floorToGoTo;

    IEnumerator DelayMoveRoutine(Vector3 pos, Collider other)
    {
        yield return new WaitForSeconds(.1f);

        other.transform.position = pos;
        Debug.Log($"Moving to {pos}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _active)
        {
            if (_direction == StairDirection.Up)
            {
                _floorToGoTo = DungeonManager.Instance.Floor - _numberOfFloorsToChange;
            }
            else if (_direction == StairDirection.Down)
            {
                _floorToGoTo = DungeonManager.Instance.Floor + _numberOfFloorsToChange;
            }

            other.GetComponent<PartyController>().StopAllRoutines();

            DungeonManager.Instance.GenerateNextFloor(_floorToGoTo);
            Debug.Log($"Going to Floor {_floorToGoTo}");

            if (_direction == StairDirection.Up && _floorToGoTo >= 0)
            {
                var pos = DungeonManager.Instance.ExitPosition(_floorToGoTo);
                pos.y = 1;
                StartCoroutine(DelayMoveRoutine(pos, other));
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = true;
        }
    }
}
