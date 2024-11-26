using UnityEngine;

public class Collider : MonoBehaviour
{
    public GameObject PausePannel;

    [System.Obsolete]
    private void Start()
    {
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PausePannel.SetActive(true);
        }
        else
        {
            Debug.Log("fuck you");
        }
    }
}
