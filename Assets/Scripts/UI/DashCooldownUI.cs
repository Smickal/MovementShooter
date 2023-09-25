using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    [SerializeField] float _fadeOutTime = 0.5f;
    float timer;

    [Header("Reference")]
    [SerializeField] Slider _dashSlider1;
    [SerializeField] Slider _dashSlider2;
    [SerializeField] CanvasGroup _cvGroup;
    [SerializeField] GameObject _dash2Container;

    bool isDash2Activated = false;
    bool isFadeOut = false;

    void Start()
    {
        if (!isDash2Activated)
        {
            _dash2Container.SetActive(false);
        }

        _cvGroup.alpha = 0f;
    }

    private void Update()
    {
        if(isFadeOut)
        {
            timer -= Time.deltaTime;
            _cvGroup.alpha = timer / _fadeOutTime;
        }
    }

    public void Activate2ndDashUI()
    {
        isDash2Activated = true;

        _dash2Container.SetActive(true);
    }

    public void ActivateDash1(float value, float maxValue)
    {
        _cvGroup.alpha = 1f;
        _dashSlider1.value = value/maxValue;
        timer = _fadeOutTime;
        isFadeOut = true;

        StopAllCoroutines();
        StartCoroutine(StopFade());
    }

    IEnumerator StopFade()
    {
        yield return new WaitForSeconds(_fadeOutTime);
        isFadeOut = false;
    }
}
