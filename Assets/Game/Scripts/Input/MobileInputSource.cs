using UnityEngine;


namespace Game.Gameplay.Inputs
{
    public sealed class MobileInputSource : IInputSource
    {
        public Vector2 Move { get; private set; }
        public bool FireHeld { get; private set; }
        public bool SwitchNext => false;
        public bool SwitchPrev => false;
        public int DigitHotkey => 0;


        public Vector3 AimPoint(Camera cam, LayerMask groundMask, float maxDist = 500f)
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                var t = UnityEngine.Input.GetTouch(0);
                var ray = cam.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out var hit, maxDist, groundMask))
                {
                    FireHeld = true;
                    return hit.point;
                }
            }
            FireHeld = false;
            return cam.transform.position + cam.transform.forward * 10f;
        }


        public void SetMove(Vector2 move) => Move = move;
        public void SetFire(bool down) => FireHeld = down;
    }
}