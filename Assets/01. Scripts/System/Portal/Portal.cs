using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool isOpen = false;
    private int portalIndex = 0;
    public bool bossPortal = false;
    public int bossIndex = 0;

    //assigned in Inspector
    [Header("Applied to the effects at start")]
    [SerializeField] private Color portalEffectColor;

    [Header("Changing these might `break` the effects")]
    [Space(20)]
    [SerializeField] private Renderer portalRenderer;
    [SerializeField] private ParticleSystem[] effectsParticles;
    [SerializeField] private Light portalLight;
    [SerializeField] private AudioSource orbAudio, flashAudio, portalAudio;

    private float maxVolOrb = 0.08f, maxVolportal = 0.8f, maxIntPortalLight = 4;
    private float transitionSpeed = 0.3f;

    //assigned when Awake
    private bool inTransition, activated;
    private Material portalMat, portalEffectMat;
    private float fadeFloat;

    private Coroutine transitionCor;


    private void Awake()
    {
        Setup();
    }

    //Call this function to activate or deactivate the effects
    public void TogglePortal(bool _activate)
    {
        if (inTransition || _activate == activated)
            return;
    
        if (_activate)//toggle on
        {
            activated = true;
            Debug.Log("포탈 오픈");
            transitionCor = StartCoroutine(PreActivate());
        }
        else if (!_activate)//toggle off
        {
            activated = false;

            effectsParticles[2].Stop();

            transitionCor = StartCoroutine(TransitionSequence());
        }
    }

    private IEnumerator PreActivate()
    {
        inTransition = true;
        isOpen = true;

        orbAudio.volume = maxVolOrb;
        orbAudio.Play();

        effectsParticles[0].Play();

        yield return new WaitForSeconds(1.2f);

        flashAudio.Play();
        portalAudio.Play();

        yield return new WaitForSeconds(0.3f);

        transitionCor = StartCoroutine(TransitionSequence());
        effectsParticles[2].Play();
    }

    private IEnumerator TransitionSequence()
    {
        inTransition = true;

        while (inTransition)
        {
            if (activated)//transition to on
            {
                fadeFloat = Mathf.MoveTowards(fadeFloat, 1f, Time.deltaTime * transitionSpeed);

                orbAudio.volume -= Time.deltaTime * 0.1f;

                if (fadeFloat >= 1f)//transition finished
                {
                    inTransition = false;
                    orbAudio.Stop();
                }
            }
            else //transition to off
            {
                fadeFloat = Mathf.MoveTowards(fadeFloat, 0f, Time.deltaTime * transitionSpeed);

                if (fadeFloat <= 0f)//transition finished
                {
                    inTransition = false;
    
                    portalAudio.Stop();
                    effectsParticles[2].Stop();
                }
            }
    
            //fade in/out
            portalAudio.volume = maxVolportal * fadeFloat;

            portalEffectMat.SetFloat("_PortalFade", fadeFloat);
            portalMat.SetFloat("_EmissionStrength", fadeFloat);

            portalLight.intensity = maxIntPortalLight * fadeFloat; 
    
            yield return null;
        }
    }

    private void Setup()
    {
        RandomStageSet();

        //Getting/creating material instance
        Material[] mats = portalRenderer.materials.ToArray();
        portalMat = mats[0];
        portalEffectMat = mats[1];

        //Deactivate effects on Start
        portalMat.SetColor("_EmissionColor", portalEffectColor);
        portalMat.SetFloat("_EmissionStrength", 0);
        portalEffectMat.SetColor("_ColorMain", portalEffectColor);
        portalEffectMat.SetFloat("_PortalFade", 0f);

        foreach (ParticleSystem part in effectsParticles)
        {
            ParticleSystem.MainModule mod = part.main;
            mod.startColor = portalEffectColor;
        }
        
        portalAudio.volume = 0f;
        portalLight.intensity = 0f;
    }

    private void RandomStageSet()
    {
        if (bossPortal)
        {
            // 보스 스테이지
            portalIndex = 3;
            portalEffectColor = Color.red;
            return;
        }
        
        int index = Random.Range(0, 10);

        if (index < 5)
        {
            // 일반 몬스터 스테이지
            portalIndex = 0;
            portalEffectColor = Color.gray;
        }
        else if (index >= 5 && index < 8)
        {
            // 엘리트 스테이지
            portalIndex = 1;
            portalEffectColor = Color.cyan;
        }
        else
        {
            // 상점 스테이지
            portalIndex = 2;
            portalEffectColor = Color.yellow;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen && other.CompareTag("Player"))
        {
            Debug.Log("포탈 진입");
            switch (portalIndex)
            {
                case 0:
                    EnterNormalStage();
                    break;
                case 1:
                    EnterEliteStage();
                    break;
                case 2:
                    EnterShopStage();
                    break;
                case 3:
                    EnterBossStage();
                    break;
            }
        }
    }

    private void EnterNormalStage()
    {
        int index = Random.Range(1, 4);
        SceneManager.LoadScene("NormalStage" + index);
    }

    private void EnterEliteStage()
    {
        int index = Random.Range(1, 4);
        SceneManager.LoadScene("EliteStage" + index);
    }
    private void EnterShopStage()
    {
        SceneManager.LoadScene("ShopStage");
    }
    private void EnterBossStage()
    {
        SceneManager.LoadScene("BossStage" + bossIndex);
    }
}
