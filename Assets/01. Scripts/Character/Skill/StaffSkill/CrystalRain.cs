using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CrystalRain
{
    private C_Model owner;
    private float skillDamage;
    public CrystalRain(C_Model owner, float skillDamage)
    {
        this.owner = owner;
        this.skillDamage = skillDamage;
    }

    public void UseSkill(GameObject skillEffect)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int groundMask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 targetPos = hit.point;

            PoolableMono effect = PoolManager.Instance.Pop(skillEffect.name);
            effect.transform.position = targetPos;
            owner.StartCoroutine(SkillDamageActive(effect));
        }
    }

    IEnumerator SkillDamageActive(PoolableMono effect)
    {
        Vector3 centor = effect.transform.position;
        Vector3 highPoint = centor + Vector3.up * 5f;

        owner.previewCenter = centor;
        owner.previewRadius = 6f;
        owner.previewHeight = 5f;
        owner.showPreview = true;

        
        for (int i = 0; i < 5; i++)
        {
            Collider[] cols = Physics.OverlapCapsule(centor, highPoint, 6);
            foreach (var col in cols)
            {
                EnemyBase enemy = col.GetComponentInParent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Mathf.RoundToInt(owner.GetStat().damage * skillDamage));
                }
            }
            yield return new WaitForSeconds(0.2f);
        }

        owner.showPreview = false;
    }

}
