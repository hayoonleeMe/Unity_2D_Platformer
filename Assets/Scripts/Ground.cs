using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().IsJump = false;
            collision.gameObject.GetComponent<Animator>().SetBool("onJump", false);
        }
    }
}
