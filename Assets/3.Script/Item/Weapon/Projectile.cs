using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[Serializable]
public class Projectile : Weapon
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float lifeTime = 3f;
    [SerializeField]
    protected FactionType targetFaction = FactionType.None;

    [SerializeField]
    protected static GameObject prefab;

    #region properties
    public float Speed => speed;
    public float LifeTime => lifeTime;
    #endregion

    [SerializeField]
    private Transform target;
    [SerializeField]
    Vector3 dirVector = Vector3.right;

    #region object pooling
    private static Dictionary<string, List<Projectile>> pool = new Dictionary<string, List<Projectile>>();

    public static void ReturnToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);

        if (pool.ContainsKey(projectile.itemCode))
        {
            pool[projectile.itemCode].Add(projectile);
        }
        else
        {
            List<Projectile> projectileList = new List<Projectile>();
            projectileList.Add(projectile);

            pool.Add(projectile.itemCode, projectileList);
        }
    }
    public static Projectile GetProjectileFromPool(string itemCode)
    {
        Projectile projectile = null;

        if (pool.ContainsKey(itemCode) && pool[itemCode].Count > 0)
        {
            projectile = pool[itemCode][0];
            pool[itemCode].RemoveAt(0);
        }
        else
        {
            if (prefab == null)
            {
                prefab = Addressables.LoadAssetAsync<GameObject>(itemCode).WaitForCompletion();
            }

            projectile = Instantiate(prefab).GetComponent<Projectile>();
        }

        projectile.gameObject.SetActive(true);

        return projectile;
    }
    #endregion

    public void Spawn(Character owner, Transform targetTransform, Vector3 spawnPosition, float damage)
    {
        Equip(owner);
        this.damage = damage <= 0 ? 1 : damage;
        target = targetTransform;
        transform.position = spawnPosition;

        if(target == null)
        {
            dirVector = new Vector3(owner.GetFrontDirX(), 0);

            StartCoroutine(Delete_co());
        }
    }

    private void Update()
    {
        MoveToTarget();


        if (dirVector == Vector3.zero)
            ReturnToPool(this);
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            dirVector = (target.position - transform.position).normalized;
        }

        Flip((int)dirVector.x);
        transform.position = GetNextPosition(dirVector);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Combat combat))
        {
            if(combat.FactionType == targetFaction)
            {
                combat.TakeDamage(damage, Owner.Combat);
                ReturnToPool(this);
            }
        }
    }

    private Vector3 GetNextPosition(Vector3 dirVector)
    {
        Vector3 newPos = transform.position + speed * dirVector * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;

        return newPos;
    }

    private void Flip(int dir)
    {
        if(dir >= 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    private IEnumerator Delete_co()
    {
        yield return new WaitForSeconds(lifeTime);

        ReturnToPool(this);
    }
}
