using UnityEngine;

public class FindAttackTarget : MonoBehaviour
{   
    public Enemy enemy;

    void OnEnable()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Debug.Log("find player");
        }
    }

    
}
