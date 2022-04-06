using UnityEngine;

public class EntityActionAnimations : EntityAction
{
    [SerializeField] private Animator m_CharacterAnimator;
    [SerializeField] private string m_ActionAnimationName;
    [SerializeField] private float m_TimeDuration;
    [SerializeField] private bool withoutAnimation;
    [SerializeField] private bool isLoopAnimation;

    private Timer m_AnimationTimer;
    protected bool m_IsPlayingAnimation;

    public override void StartAction()
    {
        if (withoutAnimation == false) m_CharacterAnimator.CrossFade(m_ActionAnimationName, m_TimeDuration);
        m_AnimationTimer = Timer.CreateTimer(m_TimeDuration, isLoopAnimation);
        m_AnimationTimer.OnTick += OnTimerTick;
        base.StartAction();
    }

    public override void EndAction()
    {
        m_AnimationTimer.OnTick -= OnTimerTick;
        base.EndAction();
    }
    private void OnDestroy()
    {
        if (m_AnimationTimer != null) m_AnimationTimer.OnTick -= OnTimerTick;
    }
    private void OnTimerTick()
    {
        if (m_CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName))
        {
            m_IsPlayingAnimation = true;
        }

        if (m_IsPlayingAnimation == true)
        {
            if (m_CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName) == false || withoutAnimation)
            {
                m_IsPlayingAnimation = false;
                EndAction();
            }
        }
    }
}
