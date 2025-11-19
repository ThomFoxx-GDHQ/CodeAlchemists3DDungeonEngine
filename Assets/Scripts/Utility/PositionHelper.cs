using UnityEngine;

public static class PositionHelper
{
    public static Vector3 TruncateVector3(Vector3 value)
    {
        value.x = Mathf.RoundToInt(value.x);
        value.y = Mathf.RoundToInt(value.y);
        value.z = Mathf.RoundToInt(value.z);

        return value;
    }

    public static Vector3 EvenOutPosition(Vector3 position)
    {
        position = PositionHelper.TruncateVector3(position);
        Vector3Int newpos = Vector3Int.zero;

        if (position.x % 2 != 0)
            newpos.x = (int)position.x + 1;
        else newpos.x = (int)position.x;
        newpos.y = (int)position.y;
        if (position.z % 2 != 0)
            newpos.z = (int)position.z + 1;
        else newpos.z = (int)position.z;

        return newpos;
    }
}
