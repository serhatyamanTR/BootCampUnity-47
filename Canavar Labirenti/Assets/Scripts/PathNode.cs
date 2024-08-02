using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode SonrakiDugum; // Bir sonraki düğüme referans


    // (Optional) Visualize path nodes in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        if (SonrakiDugum != null)
        {
            Gizmos.DrawLine(transform.position, SonrakiDugum.transform.position);
        }
    }
}
