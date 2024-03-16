using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityServiceLocator;

public class ChangeScene : MonoBehaviour
{
    public SceneReference scene;

    public GameObject craftingManager;
    public GameObject inventoryManager;

    private void Start()
    {
        DontDestroyOnLoad(craftingManager);
        DontDestroyOnLoad(inventoryManager);
    }

    [Button]
    public void GoToScene()
    {
        SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Single);
    }
}
