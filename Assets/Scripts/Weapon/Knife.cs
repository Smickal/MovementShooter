using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Knife : Weapon
{
    [Header("KnifeAtrributes")]
    [Space(5)]
    [SerializeField] int _knifeDamage;
    [SerializeField] float _meleeRange;
    [SerializeField] float _knifeKnockbackForce;
    [SerializeField] float _knifeActivateTime;

    [Space(5)]
    [SerializeField] int maxComboCounter = 3;
    [SerializeField] Collider _attackCollider;
    [SerializeField] KnifeCombo[] _knifeCombos;

    string attackString;
    int comboCounter = 0;

    bool isKnifeActivated;
    bool isComboDelayActivated;
    bool isComboWindowActivated;

    float comboDelayTimer;
    float comboWindownTimer;

    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        PlayEnterAnimation();
        Invoke(nameof(Activate), _knifeActivateTime);

        InputReader.Instance.AttackAction += Attack;
        comboCounter = 0;
    }

    private void OnDisable()
    {
        Deactivate();

        InputReader.Instance.AttackAction -= Attack;
    }


    private void Update()
    {
        DelayForCombo();
        ComboWindowTimeActivation();
   
        ApplyForceAndDamage();

        //Debugs RayDamageRange
        Debug.DrawRay(transform.position, transform.up * _meleeRange, Color.red);
    }

    private void ApplyForceAndDamage()
    {
        if (isComboDelayActivated)
        {
            //Check for RaycastHit according to swords length
            RaycastHit[] hitObjs = Physics.RaycastAll(transform.position, transform.up, _meleeRange);

            foreach (RaycastHit obj in hitObjs)
            {
                Rigidbody rb = obj.collider.attachedRigidbody;

                if (rb == null) continue;
                rb.AddForce(mainCam.transform.forward * _knifeKnockbackForce, ForceMode.Impulse);

                //TODO: Add damage here
            }
        }
    }

    private void ComboWindowTimeActivation()
    {
        if (isComboWindowActivated)
        {
            comboWindownTimer += Time.deltaTime;

            if (comboWindownTimer >= _knifeCombos[comboCounter].ComboWindowOfAttack)
            {
                isComboWindowActivated = false;
                comboCounter = 0;
            }
        }
    }

    private void DelayForCombo()
    {
        if (isComboDelayActivated)
        {
            comboDelayTimer += Time.deltaTime;

            if (comboDelayTimer >= _knifeCombos[comboCounter].ComboDelayAttack)
            {
                isComboDelayActivated = false;

                comboWindownTimer = 0f;
                isComboWindowActivated = true;
            }
        }
    }

    private void Attack()
    {
        if (isKnifeActivated == false || isComboDelayActivated == true) return;

        //Add Combo Counter
        if (isComboWindowActivated == true)
        {
            comboCounter++;
            if (comboCounter >= maxComboCounter)
            {
                comboCounter = 0;
            }
        }
        

        //Add damage to enemy



        //Trigger Animation
        _gunAnimator.SetTrigger(_knifeCombos[comboCounter].NameOfAttack);


        //Reset timer
        comboDelayTimer = 0f;
        isComboDelayActivated = true;

        comboWindownTimer = 0f;
        isComboWindowActivated = false;
    }

    private void Activate()
    {
        isKnifeActivated = true;
    }

    private void Deactivate()
    {
        isKnifeActivated = false;
    }

}


[System.Serializable]
public class KnifeCombo
{
    [SerializeField] string _nameOfAttack;
    [SerializeField] int _attackDamage;
    [SerializeField] float _comboDelayOfAttack;
    [SerializeField] float _comboWindowTimeOfAttack;
    [SerializeField] float _knifeForce;

    public string NameOfAttack { get { return _nameOfAttack; } }
    public int AttackDamage { get { return _attackDamage; } }
    public float ComboDelayAttack { get { return _comboDelayOfAttack; } }
    public float ComboWindowOfAttack { get { return _comboWindowTimeOfAttack; } }
    public float KnifeForce { get { return _knifeForce; } }
}