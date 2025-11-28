using Player;
using UnityEngine;

public class BladeAttackBehavior : IPlayerAttackBe
{
    
    public bool hasCombo => true;

    public void Execute(C_Model attacker, C_Weapon weapon)
    {
        Vector3 dir = GetMouseDirection(attacker);
        attacker.transform.forward = dir;
        Debug.Log("근접 공격 실행");

        int comboIndex = attacker.WeaponSystem.combo;
        
        attacker.Anim.SetInteger("Combo", comboIndex);
        attacker.Anim.SetTrigger("isAttack");

        attacker.cc.Move(dir * 0.4f);

        attacker.canMove = false;
        //애니메이션 진행되는 동안 이동은 못하게

    }

    private Vector3 GetMouseDirection(C_Model model)
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, model.transform.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 dir = (point - model.transform.position);
            dir.y = 0f;
            return dir.normalized;
        }
        return model.transform.forward;
    }

}
