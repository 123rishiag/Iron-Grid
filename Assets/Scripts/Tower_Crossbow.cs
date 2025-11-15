using UnityEngine;

public class Tower_Crossbow : Tower
{
    [Header("Crossbow Details")]
    [SerializeField] private Transform gunPoint;

    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            Debug.Log(hitInfo.collider.gameObject.name + " was attacked!");
            Debug.DrawLine(gunPoint.position, hitInfo.point);
        }
    }
}
