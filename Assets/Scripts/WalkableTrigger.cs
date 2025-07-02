using UnityEngine;

[RequireComponent (typeof(Animator))]
public class WalkableTrigger : MonoBehaviour
{
    private bool _canBeActivated;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _canBeActivated = true;
            _anim.SetBool("PlateDepressed", _canBeActivated);
            Debug.Log("Player has entered the Trigger zone", this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canBeActivated = false;
            _anim.SetBool("PlateDepressed", _canBeActivated);
            Debug.Log("Player has exited the Trigger zone", this.gameObject);
        }
    }
}
