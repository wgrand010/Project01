using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField ] private float knockbackstrength;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController cc = hit.collider.GetComponent<CharacterController>();

        if (cc != null )
        {
            Vector3 direction = hit.transform.position - transform.position;
            direction.y = 0;
           

            cc.Move(direction.normalized * knockbackstrength);
        }
    }
}
