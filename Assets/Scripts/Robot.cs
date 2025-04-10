// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/playlist?list=PLtLToKUhgzwm1rZnTeWSRAyx9tl8VbGUE

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] private int HitPoints = 100;
    private int maxHitPoints;
    private Animator animator;

    private NavMeshAgent agent;


    [SerializeField] private Slider healthBar;

    public List<Transform> robotLimbs;
    private Transform highlightedLimb1;
    private Transform highlightedLimb2;

    public Material defaultMaterial;
    public Material glowMaterial;  
    public Material matteMaterial;  
    public Material flashingMaterial;
    public Material crackedMaterial;


    public TextMeshProUGUI x2PopupText;

    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();

    public static int kills = 0;



    public bool isDead;

    private void Start()
    {
        maxHitPoints = HitPoints;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        
        foreach (Transform limb in robotLimbs)
        {
            Renderer renderer = limb.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterials[limb] = renderer.material;
            }
        }

        ApplyVisualization();
        x2PopupText.gameObject.SetActive(false);
    }

    public void TakeDamage(int damageAmount, string hitTag)
    {
        if (highlightedLimb1 == null || highlightedLimb2 == null)
        {
            Debug.LogWarning("Highlighted limbs are not set. Applying normal damage.");
        }
        else
        {
            string highlightedTag1 = highlightedLimb1.tag;
            string highlightedTag2 = highlightedLimb2.tag;

            Debug.Log($"Damage received on limb with tag: {hitTag}");
            Debug.Log($"Highlighted limb tags: {highlightedTag1}, {highlightedTag2}");

            if (hitTag == highlightedTag1 || hitTag == highlightedTag2)
            {
                damageAmount *= 2; 
                Debug.Log($"Double damage applied to: {hitTag}");

                if (MainMenu.selectedVisualization == "Popup")
                {
                    StartCoroutine(ShowX2Popup());
                }
            }
            else
            {
                Debug.Log($"Normal damage applied to: {hitTag}");
            }
        }

        
        HitPoints -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = HitPoints;
        }

        
        if (HitPoints <= 0)
        {
            if (healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }

            int randInt = Random.Range(0, 2);

            if (randInt == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            isDead = true;
            kills++;

            // Turn off robot hand when dead, otherwise still hurts player 
            GameObject robotHand = GameObject.FindWithTag("RobotHand");
            if (robotHand != null)
            {
                Collider handCollider = robotHand.GetComponent<Collider>();
                if (handCollider != null)
                {
                    handCollider.enabled = false;
                }
            }
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }


    private void ApplyVisualization()
    {
        switch (MainMenu.selectedVisualization)
        {
            case "Glow":
                HighlightRandomLimbs(glowMaterial);
                break;
            case "Matte":
                HighlightRandomLimbs(matteMaterial);
                break;
            case "Outline":
                HighlightRandomLimbsWithOutline();
                break;
            case "Flashing":
                StartCoroutine(FlashingEffectCoroutine());
                break;
            case "Cracked":
                HighlightRandomLimbs(crackedMaterial);
                break;
            default:
                ResetAllEffects();
                HighlightRandomLimbs(defaultMaterial);
                break;
        }
    }

    private void HighlightRandomLimbs(Material highlightMaterial)
    {
        if (robotLimbs.Count < 2) return;

        highlightedLimb1 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];

        while (highlightedLimb2 == highlightedLimb1)
        {
            highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        }

        ApplyMaterialToLimb(highlightedLimb1, highlightMaterial);
        ApplyMaterialToLimb(highlightedLimb2, highlightMaterial);

        Debug.Log($"{MainMenu.selectedVisualization} highlighting applied to: {highlightedLimb1.name} (Tag: {highlightedLimb1.tag}), {highlightedLimb2.name} (Tag: {highlightedLimb2.tag})");
    }

    private IEnumerator FlashingEffectCoroutine()
    {
        if (robotLimbs.Count < 2) yield break;

        highlightedLimb1 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];

        while (highlightedLimb2 == highlightedLimb1)
        {
            highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        }

        Renderer limbRenderer1 = highlightedLimb1.GetComponent<Renderer>();
        Renderer limbRenderer2 = highlightedLimb2.GetComponent<Renderer>();

        if (limbRenderer1 != null && limbRenderer2 != null)
        {
            Material originalMaterial1 = limbRenderer1.material;
            Material originalMaterial2 = limbRenderer2.material;

            bool isFlashing = false;
            float flashInterval = 0.5f;  

            while (MainMenu.selectedVisualization == "Flashing")
            {
                if (isFlashing)
                {
                    limbRenderer1.material = originalMaterial1;
                    limbRenderer2.material = originalMaterial2;
                }
                else
                {
                    limbRenderer1.material = flashingMaterial;
                    limbRenderer2.material = flashingMaterial;
                }

                isFlashing = !isFlashing;
                yield return new WaitForSeconds(flashInterval);
            }

            
            limbRenderer1.material = originalMaterial1;
            limbRenderer2.material = originalMaterial2;
        }
    }

    private IEnumerator ShowX2Popup()
    {
        x2PopupText.gameObject.SetActive(true);
        x2PopupText.alpha = 1;

        float duration = 1.5f;
        float elapsedTime = 0f;

        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            x2PopupText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        x2PopupText.gameObject.SetActive(false);
    }

    private void HighlightRandomLimbsWithOutline()
    {
        if (robotLimbs.Count < 2) return;

        ResetOutline();

        highlightedLimb1 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];

        while (highlightedLimb2 == highlightedLimb1)
        {
            highlightedLimb2 = robotLimbs[Random.Range(0, robotLimbs.Count)];
        }

        ApplyOutlineToLimb(highlightedLimb1);
        ApplyOutlineToLimb(highlightedLimb2);

        Debug.Log($"Outline highlighting applied to: {highlightedLimb1.name}, {highlightedLimb2.name}");
    }

    private void ApplyOutlineToLimb(Transform limb)
    {
        Outline outline = limb.GetComponent<Outline>();
        if (outline == null)
        {
            outline = limb.gameObject.AddComponent<Outline>();
        }

        outline.OutlineMode = Outline.Mode.OutlineAll;  
        outline.OutlineColor = Color.red;  
        outline.OutlineWidth = 5f;  
        outline.enabled = true;
    }

    private void ApplyMaterialToLimb(Transform limb, Material material)
    {
        Renderer renderer = limb.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public bool IsLimbHighlighted(string limbTag)
    {
        if (highlightedLimb1 != null && highlightedLimb2 != null)
        {
            return limbTag == highlightedLimb1.tag || limbTag == highlightedLimb2.tag;
        }
        return false;
    }


    private void ResetLimbMaterials()
    {
        foreach (var limb in originalMaterials)
        {
            limb.Key.GetComponent<Renderer>().material = limb.Value;
        }
        Debug.Log("Default visualization applied.");
    }

    private void ResetOutline()
    {
        foreach (Transform limb in robotLimbs)
        {
            Outline outline = limb.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
        Debug.Log("Outlines reset.");
    }

    private void ResetAllEffects()
    {
        
        ResetLimbMaterials();
        ResetOutline();
        
        Debug.Log("All effects reset.");
    }

}