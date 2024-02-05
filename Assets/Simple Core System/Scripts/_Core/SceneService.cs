using System.Collections;
using UnityEngine;

namespace Core
{
    public interface ISceneService
    {
        public EnumId ServiceId { get; }
        public SceneCore Core { get; set; }
        public IEnumerator StartService();
    }

    [DefaultExecutionOrder(-5)]
    public abstract class SceneService : MonoBehaviour, ISceneService
    {
        public EnumId ServiceId => serviceId;
        public SceneCore Core { get; set; }

        [Header("SERVICE PARAMETER")]
        [SerializeField]
        private EnumId serviceId;

        public virtual IEnumerator StartService() { yield return null; }
    }
}