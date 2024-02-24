using Core;
using TigerForge;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class PlayerDisableListener : MonoBehaviour
    {
        public UnityEvent OnEnablePlayer = new();
        public UnityEvent OnDisablePlayer = new();

        private void OnEnable()
        {
            EventManager.StartListening(EConst.ENABLE_PLAYER, EnableCharacter);
            EventManager.StartListening(EConst.DISABLE_PLAYER, DisableCharacter);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EConst.ENABLE_PLAYER, EnableCharacter);
            EventManager.StopListening(EConst.DISABLE_PLAYER, DisableCharacter);
        }

        private void EnableCharacter()
        {
            OnEnablePlayer?.Invoke();
        }

        private void DisableCharacter()
        {
            OnDisablePlayer?.Invoke();
        }
    }
}