using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("TriggerEnter");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("TriggerExit");
        }
    }
}
