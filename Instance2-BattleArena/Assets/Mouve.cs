using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float inertia = 0.1f; // Inertie après avoir relâché la touche
    private Vector2 moveDirection;
    private Vector2 velocity;

    void Update()
    {
        // Récupérer la direction du mouvement basée sur les entrées de l'utilisateur
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        // Appliquer l'inertie
        if (moveDirection.magnitude > 0.1f)
        {
            velocity = Vector2.Lerp(velocity, moveDirection.normalized * moveSpeed, inertia);
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, inertia); // Inertie pour s'arrêter
        }

        // Déplacer le joueur
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
}