using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticleSplash : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _maxActivatedTime = 5f;
    [SerializeField] ParticleSystem _particleSystem;

    float activateTimer = 0f;
    bool isActivated = false;

    public bool IsActivated { get { return isActivated; } }
    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            activateTimer -= Time.deltaTime;

            if(activateTimer < 0f)
            {
                isActivated = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateGameObject()
    {
        isActivated = true;
        activateTimer = _maxActivatedTime;
        gameObject.SetActive(true);

        _particleSystem.Play();
    }
}
