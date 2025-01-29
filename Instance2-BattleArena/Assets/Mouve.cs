using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float inertia = 0.1f; // Inertie apr�s avoir rel�ch� la touche
    private Vector2 moveDirection;
    private Vector2 velocity;

    void Update()
    {
        // R�cup�rer la direction du mouvement bas�e sur les entr�es de l'utilisateur
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        // Appliquer l'inertie
        if (moveDirection.magnitude > 0.1f)
        {
            velocity = Vector2.Lerp(velocity, moveDirection.normalized * moveSpeed, inertia);
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, inertia); // Inertie pour s'arr�ter
        }

        // D�placer le joueur
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
}