using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArrowSkill : ActiveSkill
{
    [SerializeField]
    private string arrowItemCode;

    [SerializeField]
    private Transform arrowSpawnPoint;


    public override void StartSkill()
    {
        bool canSkill = canUseSkillWhileJumping ||
            (!canUseSkillWhileJumping && ownedWeapon.Owner.StateMuchine.CurrentState.GetType() != new JumpState().GetType());
        if (!skillController.IsPlayingAnySkill && canSkill)
        {
            skillController.IsPlayingAnySkill = true;


            if (!canMoveWhileUsingSkill)
            {
                ownedWeapon.Owner.IsStuned = true;
            }

            if (ownedWeapon.Owner.TryGetComponent(out Combat combat))
            {
                if (!combat.CheckMP(requireMPAmount))
                    return;

                combat.AddMP(-requireMPAmount);
            }

            Shoot();
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();

        if (!canMoveWhileUsingSkill)
        {
            ownedWeapon.Owner.IsStuned = false;
        }
    }
    private void Shoot()
    {
        ownedWeapon.Owner.SetAnimation("Attack");
    }

    // animNotify
    public void ShootArrow()
    {
        Projectile arrow = Projectile.GetProjectileFromPool(arrowItemCode);

        int sign = ownedWeapon.Owner.GetFrontDirX();
        Vector3 boxSize = new Vector3(arrow.Speed * arrow.LifeTime * 3, ownedWeapon.Owner.Model.bounds.size.y);
        Vector3 offset = boxSize * sign / 2f;
        offset.y = 0;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(arrowSpawnPoint.position + offset, boxSize, 0, skillHitTargetLayer);

        foreach (var enemy in enemies)
        {
            if (enemy.transform.CompareTag("Enemy"))
            {
                PlayerCharacterData playerCharacterData = ((PlayerCharacter)ownedWeapon.Owner).PlayerCharacterData;

                arrow.Spawn(ownedWeapon.Owner, enemy.transform, arrowSpawnPoint.position, CalculateDamage());
                return;
            }
        }

        arrow.Spawn(ownedWeapon.Owner, null, arrowSpawnPoint.position, 1);
    }

    public override float CalculateDamage()
    {
        float atkWeight = StatusController.Instance.GetTotalValueByType(StatusType.Atk);
        atkWeight += StatusController.Instance.GetTotalValueByType(StatusType.DEX) * 0.4f;
        atkWeight += UnityEngine.Random.Range(StatusController.Instance.GetTotalValueByType(StatusType.LUK),
            0.4f * StatusController.Instance.GetTotalValueByType(StatusType.LUK));

        return atkWeight * (GetSkillDamagePercent() / 100);
    }

    public override float GetSkillDamagePercent()
    {
        return 110 + lv;
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.red;
        if (ownedWeapon != null && ownedWeapon.Owner != null)
        {
            int sign = ownedWeapon.Owner.GetFrontDirX();
            Vector3 boxSize = new Vector3(1.5f * 3, ownedWeapon.Owner.Model.bounds.size.y);
            Vector3 offset = boxSize * sign / 2f;
            offset.y = 0;
            Gizmos.DrawCube(arrowSpawnPoint.position + offset, boxSize);
        }
        */
    }
}
