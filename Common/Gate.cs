using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private bool isOpen;

    private Collider collider;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        animator.enabled = false;
    }

    public void PlayAnimationClip()
    {
        if (isOpen)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }
        SetAnimatorProperty();
    }

    private void SetAnimatorProperty()
    {
        var prop = isOpen ? "Open" : "Close";
        if (isOpen) SoundManager.Instanse.PlayOneShot(SoundManager.Instanse.DoorClosed);
        else SoundManager.Instanse.PlayOneShot(SoundManager.Instanse.DoorOpened);
        animator.enabled = true;
        animator.CrossFade(prop, 1f);
    }
}
