using System.Collections.Generic;
using UnityEngine;
using Game.Core.Di;
using Game.Gameplay.Inputs;
using Game.Gameplay.Weapons;
using Game.Configs;


namespace Game.Gameplay.Player
{
    [AddComponentMenu("Game/Player/Weapon Controller")]
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;


        [Inject] private Camera _cam;


        private readonly List<IWeapon> _weapons = new();
        private int _currentIndex;
        private PlayerController _player;
        private IInputSource _input;


        void Awake()
        {
            _player = GetComponent<PlayerController>();
            _input = _player.InputSource;
        }


        public void SetupWeapons(WeaponConfig single, WeaponConfig spread)
        {
            _weapons.Clear();
            _weapons.Add(new SingleShotWeapon(single));
            _weapons.Add(new SpreadShotWeapon(spread));
        }


        void Update()
        {
            if (_weapons.Count == 0) return;
            if (_input.SwitchNext) _currentIndex = (_currentIndex + 1) % _weapons.Count;
            if (_input.SwitchPrev) _currentIndex = (_currentIndex - 1 + _weapons.Count) % _weapons.Count;
            int digit = _input.DigitHotkey;
            if (digit > 0 && digit <= _weapons.Count) _currentIndex = digit - 1;


            if (_input.FireHeld)
            {
                var w = _weapons[_currentIndex];
                var dir = transform.forward;
                w.TryFire(_muzzle.position, dir);
            }
        }
    }
}