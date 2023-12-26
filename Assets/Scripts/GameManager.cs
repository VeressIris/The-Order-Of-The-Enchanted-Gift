using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private GameObject customerLine;
    [Header("Gifts:")]
    [SerializeField] private GameObject[] giftOptions;
    public Transform objectPosition;
    public Image wrappingPaperImg;
    public Image stickerImg;
    public GameObject objectToWrap;
    [SerializeField] private Sprite closedBoxSprite;
    [Header("Game:")]
    [SerializeField] private TMP_Text timerAndWaveText;
    private bool gameOver = false;
    private int wave = 0;
    private float timeRemaining;
    public bool addedCorrectBox = false;
    public GameObject addedBox;
    [HideInInspector] public bool wrappedBox = false;
    [HideInInspector] public bool closedBox = false;
    [HideInInspector] public bool addedSticker = false;

    void Start()
    {
        wrappingPaperImg.sprite = null;
        stickerImg.sprite = null;

        timeRemaining = 35f;
        StartNewWave(3);
    }

    void Update()
    {
        if (!gameOver)
        {
            //timer
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
            }
            else
            {
                Debug.Log($"Wave {wave} over!");
                timeRemaining = Random.Range(30f, 120f); //reset timer
                StartNewWave(Random.Range(2, 13));
            }

            //handle customers
            if (!handlingCustomer)
            {
                //setup customer handling
                handlingCustomer = true;
                objectToWrap = Instantiate(giftOptions[Random.Range(0, giftOptions.Length)], objectPosition.position, Quaternion.identity);
                
                currentCustomer = customers.Peek();
                //animate customer coming up
                Animator currentCustomerAnim = currentCustomer.GetComponent<Animator>();
                currentCustomerAnim.enabled = true;
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
                        }
                    }
                }

                if (FinalizedWrapping())
                {
                    Debug.Log("NEXT CUSTOMER!");

                    //animate customer leaving
                    customers.Dequeue().gameObject.GetComponent<Animator>().Play("Leave");
                    //animate box fading out
                    AnimateFadeOutObjects();

                    //move customers forward
                    StartCoroutine(ShiftLine(customerLine, 5.2f));

                    //reset requirement checks
                    addedCorrectBox = false;
                    wrappedBox = false; 
                    closedBox = false;
                    addedSticker = false;
                }
            }
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

    void StartNewWave(int numCustomers)
    {
        wave++;
        Debug.Log($"Starting wave {wave}!");
      
        for (int i = 0; i < numCustomers; i++)
        {
            GameObject customer = Instantiate(customerPrefab, customerLine.transform);

            Vector3 position = initCustomerPos.position - new Vector3(1.825f * i, 0f, 0f);
            customer.transform.position = position;

            customers.Enqueue(customer.GetComponent<Customer>());
        }
    }

    void DisplayRequirements(Customer currentCustomer)
    {
        wrappingPaperImg.sprite = currentCustomer.wrappingPaperImg;
        stickerImg.sprite = currentCustomer.stickerImg;
    }
}
