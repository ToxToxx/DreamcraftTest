using UnityEngine;


namespace Game.Gameplay.Inputs
{
    public sealed class PcInputSource : IInputSource
    {
        public Vector2 Move => new Vector2(
        UnityEngine.Input.GetAxisRaw("Horizontal"),
        UnityEngine.Input.GetAxisRaw("Vertical"))
        .normalized;


        public Vector3 AimPoint(Camera cam, LayerMask groundMask, float maxDist = 500f)
        {
            var ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
            return Physics.Raycast(ray, out var hit, maxDist, groundMask) ? hit.point : ray.GetPoint(20f);
        }


        public bool FireHeld => UnityEngine.Input.GetMouseButton(0);
        public bool SwitchNext => UnityEngine.Input.mouseScrollDelta.y < 0f;
        public bool SwitchPrev => UnityEngine.Input.mouseScrollDelta.y > 0f;
        public int DigitHotkey
        {
            get
            {
                for (int i = 1; i <= 9; i++)
                    if (UnityEngine.Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)))
                        return i;
                return 0;
            }
        }
    }
}