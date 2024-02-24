using TigerForge;
using UnityEngine;

namespace Waroeng
{
    public class PlayerDisableEmitter : MonoBehaviour
    {
        public void EmitEnablePlayer()
        {
            EventManager.EmitEvent(EConst.ENABLE_PLAYER);        
        }

        public void EmitDisablePlayer()
        {
            EventManager.EmitEvent(EConst.DISABLE_PLAYER);
        }
    }
}