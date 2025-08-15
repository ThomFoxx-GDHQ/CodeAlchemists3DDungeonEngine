using UnityEngine;

public class PortraitManager : MonoSingleton<PortraitManager>
{
    [SerializeField] Sprite[] _portraitSprites;

    public int PortraitCount => _portraitSprites.Length;

    public Sprite GetPortrait(int index)
    {
        if (index < _portraitSprites.Length)
        {

            return _portraitSprites[index];
        }
        return _portraitSprites[0];
    }
}
