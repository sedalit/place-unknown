using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveGameTrigger : MonoBehaviour
{
    [SerializeField] private SceneSerializer sceneSerializer;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out Player player))
        {
            sceneSerializer.SaveGame();
            gameObject.SetActive(false);
        }
    }
}
