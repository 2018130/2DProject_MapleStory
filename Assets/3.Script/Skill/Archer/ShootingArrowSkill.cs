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

    public override bool StartSkill()
    {
        bool useSkill = base.StartSkill();

        if(useSkill)
        {
            Shoot();
        }

        return useSkill;
    }

    private void Shoot()
    {
        ownedWeapon.Owner.SetAnimation("Attack");
    }

    public void ShootArrow()
    {
        Projectile arrow = Projectile.GetProjectileFromPool(arrowItemCode);

        int sign = ownedWeapon.Owner.GetFrontDirX();
        Vector3 boxSize = new Vector3(arrow.Speed * arrow.LifeTime * 3, ownedWeapon.Owner.Model.bounds.size.y);
        Vector3 offset = boxSize * sign / 2f;
        offset.y = 0;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(arrowSpawnPoint.position + offset, boxSize, 0, skillHitTargetLayer);
        
        foreach(var enemy in enemies)
        {
            if (enemy.transform.CompareTag("Enemy"))
            {
                PlayerCharacterData playerCharacterData = ((PlayerCharacter)ownedWeapon.Owner).PlayerCharacterData;

                arrow.Spawn(ownedWeapon.Owner, enemy.transform, arrowSpawnPoint.position, CalculateDamage() * atkRate * (10 + lv));
                return;
            }
        }

        arrow.Spawn(ownedWeapon.Owner, null, arrowSpawnPoint.position, 1);
    }

    protected override float CalculateDamage()
    {
        PlayerCharacterData playerCharacterData = ((PlayerCharacter)ownedWeapon.Owner).PlayerCharacterData;

        float atkWeight = playerCharacterData.statusData.ATK;
        atkWeight += playerCharacterData.statusData.DEX * 0.4f;
        atkWeight += UnityEngine.Random.Range(playerCharacterData.statusData.LUK, 0.4f * playerCharacterData.statusData.LUK);

        return atkWeight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(ownedWeapon != null && ownedWeapon.Owner != null)
        {
            int sign = ownedWeapon.Owner.GetFrontDirX();
            Vector3 boxSize = new Vector3(1.5f * 3, ownedWeapon.Owner.Model.bounds.size.y);
            Vector3 offset = boxSize * sign / 2f;
            offset.y = 0;
            Gizmos.DrawCube(arrowSpawnPoint.position + offset, boxSize);
        }
    }
}
