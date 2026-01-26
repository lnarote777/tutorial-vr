using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Calls functionality when a collision occurs
/// </summary>
public class OnCollision : MonoBehaviour
{
    [Serializable] public class CollisionEvent : UnityEvent<Collision> { }

    // When the object enters a collision
    public CollisionEvent OnEnter = new CollisionEvent();

    // When the object exits a collision
    public CollisionEvent OnExit = new CollisionEvent();

    [Header("Audio Settings")]
    public AudioClip bounceClip;
    [Range(0, 1)] public float volumeScale = 1.0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnEnter.Invoke(collision);
        // --- LÓGICA DE AUDIO ---
        if (bounceClip != null)
        {
            // Calculamos la fuerza del impacto para que el sonido sea realista
            float impactForce = collision.relativeVelocity.magnitude;
            float volume = Mathf.Clamp01(impactForce / 10f) * volumeScale;

            // Variamos el tono un poco para que no canse el oído
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(bounceClip, volume);
        }

        Debug.Log("Choque detectado con: " + collision.gameObject.name + " a una fuerza de: " + collision.relativeVelocity.magnitude);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnExit.Invoke(collision);
    }

    private void OnValidate()
    {
        if (TryGetComponent(out Collider collider))
            collider.isTrigger = false;
    }
}
