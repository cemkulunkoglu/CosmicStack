using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [Tooltip("Bu objenin verdi�i hasar")]
    public int damage = 1;

    [Tooltip("�arpt�ktan sonra kendini yok et")]
    public bool destroyOnHit = true;
}
