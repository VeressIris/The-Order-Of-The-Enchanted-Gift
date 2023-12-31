using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Customers:")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform initCustomerPos;
    private int customersServed = 0;
    private Queue<Customer> customers = new Queue<Customer>();
    private bool handlingCustomer = false;
    private Customer currentCustomer;
    private int numCustomers = 0;
    [Header("Gifts:")]
    [SerializeField] private GameObject[] giftOptions;
    public Transform objectPosition;
    public SpriteRenderer wrappingPaperSR;
    public SpriteRenderer stickerSR;
    public GameObject objectToWrap;
    [SerializeField] private Sprite closedBoxSprite;
    [Header("Game:")]
    [SerializeField] private TMP_Text timerAndWaveText;
    [HideInInspector] public bool gameOver = false;
    private int wave = 0;
    private float timeRemaining;
    public bool addedCorrectBox = false;
    public GameObject addedBox;
    [HideInInspector] public bool timerRunning = true;
    private bool countdownRunning = false;
    [HideInInspector] public bool wrappedBox = false;
    [HideInInspector] public bool closedBox = false;
    [HideInInspector] public bool addedSticker = false;
    [SerializeField] private GiftWrapping tapeController;
    [SerializeField] private Animator thinkingBubbleAnim;
    [Header("UI")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text wavesSurvivedText;
    [SerializeField] private GameObject pauseScreen;
    private bool paused = false;
    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip closeBoxSFX;
    [SerializeField] private AudioClip nextCustomerSFX;
    [SerializeField] private AudioClip gameOverSFX;

    void Awake()
    {
        gameOverScreen.SetActive(false);

        wrappingPaperSR.sprite = null;
        stickerSR.sprite = null;

        paused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);

        timeRemaining = 32f;
        timerRunning = true;
        numCustomers = 3;
        StartNewWave();
    }

    void Update()
    {
        if (!gameOver)
        {
            //pausing:
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!paused)
                {
                    Time.timeScale = 0f;
                    pauseScreen.SetActive(true);
                    timerAndWaveText.text = "";
                    paused = true;
                }
                else
                {
                    Time.timeScale = 1f;
                    pauseScreen.SetActive(false);
                    paused = false;
                }
            }

            //timer
            if (timeRemaining > 0f)
            {
                if (timerRunning && !paused)
                {
                    timeRemaining -= Time.deltaTime;
                    UpdateTimer(timeRemaining);
                }
            }
            else
            {
                Debug.Log("GAME OVER");

                audioSource.clip = gameOverSFX;
                audioSource.Play();
                
                gameOver = true;
                gameOverScreen.SetActive(true);
                wavesSurvivedText.text = $"Waves survived:\n{wave - 1}";
                timerAndWaveText.text = "";
            }

            //handle customers
            if (!handlingCustomer && customers.Count > 0)
            {
                //setup customer handling
                handlingCustomer = true;
                objectToWrap = Instantiate(giftOptions[Random.Range(0, giftOptions.Length)], objectPosition.position, Quaternion.identity);
                objectToWrap.GetComponent<Animator>().Play("FadeIn");

                currentCustomer = customers.Peek();
                //animate customer coming up
                Animator currentCustomerAnim = currentCustomer.GetComponentInChildren<Animator>();
                currentCustomerAnim.Play("Pop up", 0);

                DisplayRequirements(currentCustomer);
            }
            else
            {
                //handle customer
                if (addedCorrectBox)
                {
                    //close box
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (BoxClicked())
                        {
                            addedBox.GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
                            closedBox = true;
                            audioSource.clip = closeBoxSFX;
                            audioSource.Play();
                        }
                    }
                }

                if (FinalizedWrapping())
                {
                    audioSource.volume = 0.25f;
                    audioSource.clip = nextCustomerSFX;
                    audioSource.Play();
                    audioSource.volume = 0.7f;

                    Debug.Log("NEXT CUSTOMER!");

                    //animate customer leaving
                    customers.Dequeue().gameObject.GetComponentInChildren<Animator>().Play("Leave");
                    //animate box fading out
                    AnimateFadeOutObjects();

                    foreach (Customer customer in customers)    
                    {
                        StartCoroutine(ShiftLine(customer.gameObject, 5.2f));
                    }

                    //reset requirement checks
                    ResetBools();
                    handlingCustomer = false;
                    customersServed++;
                }
            }

            //finished wave
            if (customersServed == numCustomers)
            {
                customersServed = 0;

                if (!countdownRunning)
                {
                    StartCoroutine(Cooldown(Random.Range(3f, 12f)));
                }
            }
        }
    }

    IEnumerator Cooldown(float cooldown)
    {
        countdownRunning = true;
        timerRunning = false;

        timerAndWaveText.text = "Next wave incoming...";

        Debug.Log($"Starting {cooldown} cooldown...");

        stickerSR.GetComponent<Animator>().Play("NoDestroyFadeOut");
        wrappingPaperSR.GetComponent<Animator>().Play("NoDestroyFadeOut");
        thinkingBubbleAnim.Play("NoDestroyFadeOut");

        yield return new WaitForSeconds(cooldown);
        
        countdownRunning = false;

        numCustomers = Random.Range(2, 13);
        timeRemaining = Random.Range(30f, 120f); //reset timer
        StartNewWave();
    }

    void ResetBools()
    {
        addedCorrectBox = false;
        wrappedBox = false;
        closedBox = false;
        addedSticker = false;
        tapeController.holdingTape = false;
        GameObject[] stickerControllers = GameObject.FindGameObjectsWithTag("StickerButton");
        foreach (GameObject stickerController in stickerControllers)
        {
            stickerController.GetComponent<GiftWrapping>().holdingSticker = false;
        }
    }

    IEnumerator ShiftLine(GameObject customer, float speed)
    {
        Vector3 targetPos = customer.transform.position + new Vector3(1.825f, 0f, 0f);
        float time = 0f;
        while (time < 1.2f)
        {
            time += Time.deltaTime;

            customer.transform.position = Vector3.Lerp(customer.transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
    }

    void AnimateFadeOutObjects()
    {
        addedBox.GetComponent<Animator>().Play("FadeOut");
        GameObject.FindGameObjectWithTag("Tape").GetComponent<Animator>().Play("FadeOut");
        GameObject.FindGameObjectWithTag("Sticker").GetComponent<Animator>().Play("FadeOut");
    }

    bool FinalizedWrapping()
    {
        if (!wrappedBox) return false;
        GameObject sticker = GameObject.FindGameObjectWithTag("Sticker");
        if (sticker == null) return false;
        if (sticker.GetComponent<StickerController>().placed) return true;
        return false;
    }

    //handles box wrapping evolution
    bool BoxClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("UnwrappedBox"))
            {
                objectHit.layer = LayerMask.NameToLayer("ClosedBox");
                return true;
            }
        }

        return false;
    }

    void UpdateTimer(float time)
    {
        time++;
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60f);
        timerAndWaveText.text = string.Format($"Wave {wave}\n {minutes:00}:{seconds:00}");
    }

    void StartNewWave()
    {
        stickerSR.GetComponent<Animator>().Play("FadeIn");
        wrappingPaperSR.GetComponent<Animator>().Play("FadeIn");
        thinkingBubbleAnim.Play("FadeIn");

        wave++;
        Debug.Log($"Starting wave {wave}!");

        for (int i = 0; i < numCustomers; i++)
        {
            GameObject customer = Instantiate(customerPrefab, initCustomerPos.position - new Vector3(1.825f * i, 0f, 0f), Quaternion.identity);

            customers.Enqueue(customer.GetComponent<Customer>());
        }

        timerRunning = true;
    }

    void DisplayRequirements(Customer currentCustomer)
    {
        wrappingPaperSR.sprite = currentCustomer.wrappingPaperImg;
        stickerSR.sprite = currentCustomer.stickerImg;
    }

    public void Return()
    {
        paused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
