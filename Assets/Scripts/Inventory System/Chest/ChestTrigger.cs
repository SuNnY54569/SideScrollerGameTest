using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;
    
    private bool playerInRange = false;
    private Inventory chestInventory;
    
    private CanvasGroup canvasGroup;
    private Tween promptTween;

    private void Awake()
    {
        chestInventory = GetComponent<Inventory>();
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (ChestUI.Instance.gameObject.activeSelf)
                ChestUI.Instance.Close();
            else
                ChestUI.Instance.Open(chestInventory);
            
            HidePrompt();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && ChestUI.Instance.gameObject.activeSelf)
        {
            ChestUI.Instance.Close();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HidePrompt();
        }
    }
    
    private void ShowPrompt()
    {
        if (promptUI == null) return;

        if (canvasGroup == null)
            canvasGroup = promptUI.GetComponent<CanvasGroup>();

        promptUI.transform.localScale = Vector3.zero;
        promptUI.SetActive(true);
        
        promptTween?.Kill();
        
        promptTween = promptUI?.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        if (canvasGroup != null)
            canvasGroup.DOFade(1f, 0.25f);
    }

    private void HidePrompt()
    {
        if (promptUI == null) return;

        promptTween?.Kill();
        
        promptTween = promptUI?.transform.DOScale(0f, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => promptUI.SetActive(false));

        if (canvasGroup != null)
            canvasGroup.DOFade(0f, 0.2f);
    }
}
