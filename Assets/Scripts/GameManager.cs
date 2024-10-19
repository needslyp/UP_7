using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private Text resultText;
    [SerializeField] private Image backImage;
    [SerializeField] private Text statisticsText;

    [SerializeField] private ImageTimer harvestTimer;
    [SerializeField] private ImageTimer eatingTimer;

    [SerializeField] private Image raidTimerImg;
    [SerializeField] private Image peasantTimerImg;
    [SerializeField] private Image warriorTimerImg;

    [SerializeField] private Button peasantButton;
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private Text peasantCountText;
    [SerializeField] private Text warriorCountText;
    [SerializeField] private Text wheatCountText;

    [SerializeField] private Text harvestTimerText;
    [SerializeField] private Text eatingTimerText;
    [SerializeField] private Text raidTimerText;

    [SerializeField] private Text peasantCostText;
    [SerializeField] private Text warriorCostText;
    [SerializeField] private Text upgradeCostText;

    [SerializeField] private AudioSource mainSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource raidSound;
    [SerializeField] private AudioSource timerSound;


    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float raidTimer;

    private int raidCounter = 0;

    private int allAmountOfWheats = 0;
    private int allAmountOfPeasants = 0;
    private int allAmountOfWarriors = 0;

    public int peasantCount = 3;
    public int warriorCount = 0;
    public int wheatCount = 10;

    public int upgradeCount = 1;

    public int wheatPerPeasant = 2;
    public int wheatToWarriors = 5;

    public int peasantCost = 5;
    public int warriorCost = 9;
    public int upgradeCost = 50;

    public float peasantCreateTime = 6;
    public float warriorCreateTime = 9;
    public float raidMaxTime = 20;
    public int raidIncrease = 2;
    public int nextRaid = 0;

    public int winAmountOfPeasants = 100;
    public int winAmountOfRaids = 15;
    
    void Start()
    {
        mainSound = GetComponent<AudioSource>();

        UpdateResourceText();
        Time.timeScale = 1;
        mainSound.Play();
        raidTimer = raidMaxTime;

    }

    void Update()
    {   
        raidTimer -= Time.deltaTime;
        raidTimerImg.fillAmount = raidTimer / raidMaxTime;

        if (raidTimer <= 0){
            raidSound.Play();
            raidTimer = raidMaxTime;
            warriorCount -= nextRaid;
            raidCounter += 1;

            if (raidCounter > 2){
                nextRaid += raidIncrease;
            }

            if (raidCounter > 0 && raidCounter % 5 == 0 ){
                if (peasantCreateTime > 2.5f) peasantCreateTime -= 0.05f * raidCounter;
                if (warriorCreateTime > 3.5f) warriorCreateTime -= 0.05f * raidCounter;
                raidMaxTime += 0.1f * raidCounter;
            }
            
        }

        if (harvestTimer.tick){
            timerSound.Play();
            wheatCount += peasantCount * wheatPerPeasant;
            allAmountOfWheats += peasantCount * wheatPerPeasant;
        }

        if (eatingTimer.tick){
            timerSound.Play();
            wheatCount -= warriorCount * wheatToWarriors;
        }

        if (peasantTimer > 0){
            peasantTimer -= Time.deltaTime;
            peasantTimerImg.fillAmount = peasantTimer / peasantCreateTime;
        }
        else if (peasantTimer > -1){
            peasantTimerImg.fillAmount = 0;
            allAmountOfPeasants += upgradeCount;
            peasantCount += upgradeCount;
            peasantButton.interactable = true;
            peasantTimer = -2;
        }
        else {
            peasantButton.interactable = wheatCount >= peasantCost;
        }

        if (warriorTimer > 0){
            warriorTimer -= Time.deltaTime;
            warriorTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1){
            warriorTimerImg.fillAmount = 0;
            warriorCount += upgradeCount;
            allAmountOfWarriors += upgradeCount;
            warriorButton.interactable = true;
            warriorTimer = -2;
        }
        else {
            warriorButton.interactable = wheatCount >= warriorCost;
        }

        upgradeButton.interactable = wheatCount >= upgradeCost;

        UpdateResourceText();

        if (warriorCount < 0 || wheatCount < 0){
            EndGame(false);
        }
        else if (peasantCount >= winAmountOfPeasants || raidCounter >= winAmountOfRaids){
            EndGame(true);
        }
    }

    public void CreatePeasant(){
        wheatCount -= peasantCost;
        peasantTimer = peasantCreateTime - Random.Range(0, 5) * 0.5f;
        peasantButton.interactable = false;

        clickSound.Play();
    }

    public void CreateWarrior(){
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime - Random.Range(0, 5) * 0.5f;
        warriorButton.interactable = false;

        clickSound.Play();
    }

    public void Upgrade(){
        wheatCount -= upgradeCost;
        upgradeCount += 1;
        upgradeCost += 10 * upgradeCount;
        
        clickSound.Play();
    }

    private void UpdateResourceText(){
        peasantCountText.text = $"Крестьяне: {peasantCount}К";
        warriorCountText.text = $"Воины: {warriorCount}В";
        wheatCountText.text = $"Пшеничка: {wheatCount}П";

        peasantCostText.text = $"-{peasantCost}П";
        warriorCostText.text = $"-{warriorCost}П";
        upgradeCostText.text = $"-{upgradeCost}П";

        harvestTimerText.text = $"+{peasantCount * wheatPerPeasant}П";
        eatingTimerText.text = $"-{warriorCount * wheatToWarriors}П";
        
        raidTimerText.text = raidCounter < 3 ? $" << {3 - raidCounter}" : $"-{nextRaid}В";
    }

    private void EndGame(bool isWin){
        mainCanvas.SetActive(false);
        endCanvas.SetActive(true);

        Time.timeScale = 0;
        mainSound.Pause();

        backImage.color = isWin ? new Color(0.03f, 0.3f, 0.07f, 0.5f) : new Color(0.3f, 0.03f, 0.07f, 0.5f);
        resultText.text = isWin ? "ПОБЕДА" : "ПОРАЖЕНИЕ";

        statisticsText.text = $"Нанято крестьян: {allAmountOfPeasants}К \nПройдено волн: {raidCounter} \nНанято воинов: {allAmountOfWarriors}В \nПшеницы собрано: {allAmountOfWheats}П";
    }

    public void RestartGame(){
        peasantTimer = -2;
        warriorTimer = -2;

        raidCounter = 0;

        allAmountOfWheats = 0;
        allAmountOfPeasants = 0;
        allAmountOfWarriors = 0;

        peasantCount = 3;
        warriorCount = 0;
        wheatCount = 10;

        upgradeCount = 1;

        wheatPerPeasant = 2;
        wheatToWarriors = 5;

        peasantCost = 5;
        warriorCost = 9;
        upgradeCost = 50;

        peasantCreateTime = 6;
        warriorCreateTime = 9;
        raidMaxTime = 20;
        raidIncrease = 2;
        nextRaid = 0;

        winAmountOfPeasants = 100;
        winAmountOfRaids = 15;

        endCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        Start();
    }
}
