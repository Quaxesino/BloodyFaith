using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePannel;
    PlayerController playerController;
    [SerializeField] MeleeFighter meleeFighter;

    [System.Obsolete]
    private void Start()
    {
        // Referanslarý dinamik olarak bul
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController bulunamadý! Lütfen sahnede olduðundan emin olun.");
            }
        }

        if (meleeFighter == null)
        {
            meleeFighter = FindObjectOfType<MeleeFighter>();
            if (meleeFighter == null)
            {
                Debug.LogError("MeleeFighter bulunamadý! Lütfen sahnede olduðundan emin olun.");
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        PausePannel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePannel.SetActive(false);
        Time.timeScale = 1;
    }
    public void AttackUp()
    {
        meleeFighter.DamageVal *= 2;
        PausePannel.SetActive(false);
        Time.timeScale = 1;
    }
    public void SpeedUp()
    {
        playerController.moveSpeed += 2f;
        playerController.dashingPower += 1f;
        PausePannel.SetActive(false);
        Time.timeScale = 1;
    }

    public void HealthUp()
    {
        meleeFighter.Health *= 2;
        PausePannel.SetActive(false);
        Time.timeScale = 1;
    }
}