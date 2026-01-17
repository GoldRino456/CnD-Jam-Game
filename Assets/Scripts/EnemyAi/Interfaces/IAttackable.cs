using UnityEngine;

public interface IAttackable
{
    bool IsAttacking {get; set;}

    void SetAttackStatus(bool isAttacking);
}
