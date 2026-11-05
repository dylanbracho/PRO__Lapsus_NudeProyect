using UnityEngine;

public class espejoActivste : MonoBehaviour
{
    [SerializeField] private GameObject colleccionable;
    [SerializeField] private GameObject espejo;
    void Update()
    {
        if (colleccionable == null)
        {
            espejo.SetActive(true);
            gameObject.SetActive(false);
        }
    }


}
