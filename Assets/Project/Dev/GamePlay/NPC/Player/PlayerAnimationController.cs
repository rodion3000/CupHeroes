using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlayerAnimationController : MonoBehaviour
{
    [SpineAnimation] public string idle;
    [SpineAnimation] public string attack_start;
    [SpineAnimation] public string attack_target;
    [SpineAnimation] public string attack_finish;

    public SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimationState;
    public bool isAttacking { get; private set; } = false; 
    private bool isFinishingAttack = false; 
    
    private PlayerAttack _playerAttack;

    [Inject]
    private void Construct( PlayerAttack playerAttack)
    {
        _playerAttack = playerAttack;
    }
    public void Initialize()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
    }

    public void Tick()
    {
        if (isFinishingAttack)
        {
            var currentAnimation = spineAnimationState.GetCurrent(0);
            if (currentAnimation != null && currentAnimation.Animation.Name == attack_finish && currentAnimation.IsComplete)
            {
                isFinishingAttack = false;
                PlayerIdleAnimation(); // Переход к idle после завершения атаки
            }
        }
        else if (!isAttacking && !IsPlayingIdle())
        {
            PlayerIdleAnimation();
        }
    }
    private void PlayStartAttackAnimation()
    {
        var currentAnimation = spineAnimationState.GetCurrent(0);

        if (currentAnimation == null || currentAnimation.Animation.Name != attack_start)
        {
            spineAnimationState.SetAnimation(0, attack_start, false);
            isAttacking = true;
        }
    }
    private void PlayerIdleAnimation()
    {
        var currentAnimation = spineAnimationState.GetCurrent(0);
        
        if (currentAnimation == null || currentAnimation.Animation.Name != idle)
        {
            spineAnimationState.SetAnimation(0, idle, true); 
            isAttacking = false; 
        }
    }
    private void PlayFinishAttack()
    {
        var currentAnimation = spineAnimationState.GetCurrent(0);
        
        if (currentAnimation == null || currentAnimation.Animation.Name != attack_finish)
        {
            spineAnimationState.SetAnimation(0, attack_finish, false);
            isFinishingAttack = true; 

            _playerAttack.ShootArrow(); // Запускаем стрелу при завершении атаки
        }
    }
    public bool IsPlayingIdle()
    {
        var currentAnimation = spineAnimationState.GetCurrent(0);
        return currentAnimation != null && currentAnimation.Animation.Name == idle;
    }

    private void OnMouseDown() 
    {
        PlayStartAttackAnimation();
    }

    private void OnMouseUp() 
    {
        isAttacking = false; 
        PlayFinishAttack(); 
    }
}
