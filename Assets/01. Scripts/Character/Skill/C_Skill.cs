using Player;
using System.Collections;
using UnityEngine;

public class C_Skill
{
    private readonly C_Model _model;
    public bool isSkill1Cool = false;
    public bool isSkill2Cool = false;


    public C_Skill(C_Model model)
    {
        this._model = model;
        return;
    }

    public void UseSkill1()
    {
        _model.StartCoroutine(Skill1Cooldown(5f));
    }

    public void UseSkill2()
    {
        _model.StartCoroutine(Skill2Cooldown(5f));
    }

    IEnumerator Skill1Cooldown(float cool)
    {
        isSkill1Cool = true;
        Debug.Log("Skill 1 used, cooldown started.");
        yield return new WaitForSeconds(cool);
        Debug.Log("Skill 1 used, cooldown End.");
        isSkill1Cool = false;
    }

    IEnumerator Skill2Cooldown(float cool)
    {
        isSkill2Cool = true;
        Debug.Log("Skill 2 used, cooldown started.");
        yield return new WaitForSeconds(cool);
        Debug.Log("Skill 2 used, cooldown End.");
        isSkill2Cool = false;
    }
}
