using System;
using UnityEngine;


namespace Game.Gameplay.Player
{
    [AddComponentMenu("Game/Player/Player Health")]
    public sealed class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _max = 5;
        private int _current;
        public event Action<int, int> Changed;
        public event Action Died;
        public int Current => _current;
        public int Max => _max;


        void Awake() => _current = _max;


        public void TakeDamage(int dmg)
        {
            if (_current <= 0) return;
            _current = Mathf.Max(0, _current - Mathf.Abs(dmg));
            Changed?.Invoke(_current, _max);
            if (_current == 0) Died?.Invoke();
        }
    }
}