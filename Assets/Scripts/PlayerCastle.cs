using UnityEngine;

public class PlayerCastle : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Enemy"))
        {
            Destroy(_other.gameObject);
        }
    }
}
