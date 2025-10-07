using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [Tooltip("Bu objenin verdiði hasar")]
    public int damage = 1;

    [Tooltip("Çarptýktan sonra kendini yok et")]
    public bool destroyOnHit = true;
}
