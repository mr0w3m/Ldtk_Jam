using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _twitchTime = 2;

    float secondstolasttwitch = 0;

    private void Update()
    {

        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            if (direction.magnitude > 5)
            {
                Twitch();
                return;
            }


            direction = direction.normalized;
            float angleDir = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angleDir += 90;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angleDir);
        }
    }

    private void Twitch()
    {
        if (secondstolasttwitch > 0)
        {
            secondstolasttwitch -= Time.deltaTime;
        }
        else
        {
            secondstolasttwitch = Random.Range(0, _twitchTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(0, 360));
        }
    }
}
