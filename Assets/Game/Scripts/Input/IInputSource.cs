using UnityEngine;


namespace Game.Gameplay.Inputs
{
    public interface IInputSource
    {
        Vector2 Move { get; } 
        Vector3 AimPoint(Camera cam, LayerMask groundMask, float maxDist = 500f);
        bool FireHeld { get; }
        bool SwitchNext { get; }
        bool SwitchPrev { get; }
        int DigitHotkey { get; } 
    }
}