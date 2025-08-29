using UnityEngine;


namespace Game.Core.Utils
{
    public static class Extensions
    {
        public static Vector2 WithY(this Vector2 v, float y) { v.y = y; return v; }
    }
}