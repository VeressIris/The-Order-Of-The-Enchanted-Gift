using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Transactions;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    [SerializeField] private Transform objectPosition;
    [SerializeField] private TMP_Text timerAndWaveText;
    private int wave = 0;
    private float timeRemaining;
    private int customersServed = 0;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform initCustomerPos;
    private Queue<Customer> customers = new Queue<Customer>();
    private Vector3[] customersPos;
    [SerializeField] private Image wrappingPaperImg;
    [SerializeField] private Image stickerImg;
    bool handlingCustomer = false;
    [SerializeField] private GameObject[] giftOptions;

    void Start()
    {
        timeRemaining = 10f;
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
                handlingCustomer = true;
                GameObject objectToWrap = Instantiate(giftOptions[Random.Range(0, giftOptions.Length)], objectPosition.position, Quaternion.identity);
                Customer currentCustomer = customers.Peek();
                DisplayRequirements(currentCustomer);
            }
        }
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
        customersPos = new Vector3[numCustomers];

        for (int i = 0; i < numCustomers; i++)
        {
            Customer customer;
            if (customers.Count == 0)
            {
                customer = Instantiate(customerPrefab, initCustomerPos.position, Quaternion.identity).GetComponent<Customer>();
                customersPos[0] = initCustomerPos.position;
            }
            else
            {
                //instantiate customers behind each other
                customersPos[i] = customersPos[i - 1] - new Vector3(1.5f, 0f, 0f);
                customer = Instantiate(customerPrefab, customersPos[i], Quaternion.identity).GetComponent<Customer>(); 
            }
            customers.Enqueue(customer);
        }
    }

    void DisplayRequirements(Customer currentCustomer)
    {
        wrappingPaperImg.sprite = currentCustomer.wrappingPaperImg;
        stickerImg.sprite = currentCustomer.stickerImg;
    }
}
