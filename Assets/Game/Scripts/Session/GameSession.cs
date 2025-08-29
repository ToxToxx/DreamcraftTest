using UnityEngine;
using Game.Gameplay.Player;


namespace Game.Gameplay.Session
{
    [AddComponentMenu("Game/Session/Game Session")]
    public sealed class GameSession : MonoBehaviour
    {
        private PlayerHealth _playerHealth;


        public void Attach(PlayerHealth health)
        {
            if (_playerHealth)
            {
                _playerHealth.Changed -= OnHpChanged;
                _playerHealth.Died -= OnDied;
            }
            _playerHealth = health;
            _playerHealth.Changed += OnHpChanged;
            _playerHealth.Died += OnDied;
        }


        private void OnHpChanged(int cur, int max) { /* hook UI later */ }
        private void OnDied()
        {
            Time.timeScale = 0f;
            Debug.Log("GAME OVER");
            // TODO: show simple UI and Restart button
        }
    }
}