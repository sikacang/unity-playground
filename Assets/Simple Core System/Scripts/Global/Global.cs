using UnityEngine;

public static class Global
{
    public static GlobalConfig GlobalSettings   { get; private set; }    
    private static bool _isInitialized;

    #region UNITY ATTRIBUTE METHOD
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitializeSubSystem()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        Application.quitting -= OnApplicationQuit;
        Application.quitting += OnApplicationQuit;

        _isInitialized = true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeAfterSceneLoad()
    {
        // You can unpause network services here
    }
    #endregion

    private static void Initialize()
    {
        if (!_isInitialized) return;

        GlobalSettings = Resources.Load<GlobalConfig>(GlobalConfig.GLOBAL_CONFIG_PATH);
        

        _isInitialized = true;
    }

    private static void DeInitialize()
    {
        if (!_isInitialized) return;
        _isInitialized = false;
    }

    private static void OnApplicationQuit()
    {
        DeInitialize();
    }

    private static T CreateStaticObject<T>() where T : Component
    {
        var gameObject = new GameObject(typeof(T).Name);
        Object.DontDestroyOnLoad(gameObject);

        return gameObject.AddComponent<T>();
    }
}