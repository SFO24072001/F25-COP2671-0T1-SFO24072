using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Constants;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference playerActionReference;
    public Scenes sceneToUnload;
    public Scenes sceneToLoad;
    private Transform returnPoint;

    private void Awake()
    {
        returnPoint = transform.GetChild(0);
    }
    private void Start()
    {   
        PlayerController.Instance.transform.position = returnPoint.position;
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            //Debug.Log("found");
            //if (playerActionReference.action.WasPressedThisFrame())
            //{

                if (sceneToUnload == Scenes.Main)
                    GridController.Instance.HideVisuals();

                if (sceneToLoad == Scenes.Main)
                    GridController.Instance.ShowVisuals();


                await SceneManager.UnloadSceneAsync($"{sceneToUnload}");
                SceneManager.LoadScene($"{sceneToLoad}", LoadSceneMode.Additive);
            //}
        }
    }
}
