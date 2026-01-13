using UnityEngine;

public interface IMovebale
{
    Rigidbody2D enemyRb {get; set;}

    bool isFacingRight {get; set;}

    void MoveEnemy(Vector2 velocity);

    void CheckIsFacingRight(Vector2 velocity);

}
