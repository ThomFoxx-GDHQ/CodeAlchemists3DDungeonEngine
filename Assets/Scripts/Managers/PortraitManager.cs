using UnityEngine;

public class PortraitManager : MonoSingleton<PortraitManager>
{
    [SerializeField] Sprite[] _portraitSprites;

    public Sprite GetPortrait(int index)
    {
        return _portraitSprites[index];
    }
}
