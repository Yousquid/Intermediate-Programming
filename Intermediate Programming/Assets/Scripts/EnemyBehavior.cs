using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GridManager gridManager;
    public TrunManager turnManager;

    private Vector2Int moveDirction;
    private int moveDistance;

    public void SetNextMovementPlace()
    {
        moveDistance = Random.Range(1, 4);
        int directionRandomer = Random.Range(1, 4);

        if (directionRandomer == 1)
        {
            moveDirction = new Vector2Int(0, 1);
        }
        else if (directionRandomer == 2)
        {
            moveDirction = new Vector2Int(0, -1);
        }
        else if (directionRandomer == 3)
        {
            moveDirction = new Vector2Int(-1, 0);
        }
        else if (directionRandomer == 4)
        {
            moveDirction = new Vector2Int(1, 0);
        }

        
    }
}
