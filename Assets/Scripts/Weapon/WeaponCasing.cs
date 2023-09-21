using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCasing : MonoBehaviour
{
    [SerializeField] float _maxActivatedTime = 5f;
    [SerializeField] Rigidbody _rigidbody;

    float activateTimer = 0f;
    bool isActivated = false;

    public bool IsActivated { get { return isActivated; } }
    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            activateTimer -= Time.deltaTime;

            if (activateTimer < 0f)
            {
                isActivated = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateGameObject(Vector3 Force)
    {
        isActivated = true;
        activateTimer = _maxActivatedTime;
        gameObject.SetActive(true);

        _rigidbody.AddForce(Force, ForceMode.Impulse);
    }

}
