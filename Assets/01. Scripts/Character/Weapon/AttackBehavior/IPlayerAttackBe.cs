using Player;
using UnityEngine;

public interface IPlayerAttackBe
{
    bool hasCombo { get; }
    void Execute(C_Model attacker,C_Weapon weapon);
}
