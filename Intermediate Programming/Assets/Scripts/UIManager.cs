using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public int food = 0;
    public TextMeshProUGUI foodNumberText;
    public GridManager gridManager;
    public TrunManager trunManager;
    public ClickEventer clickEventer;
    public TextMeshProUGUI animalSpecialAbilityButtonText;
    public GameObject animalSpecialAbilityButton;
    public TextMeshProUGUI actionText;
    public float timer = 0;
    public bool isHiding = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodNumberText.text = "Food: " + food;

        actionText.text = "Action:" + gridManager.grid[clickEventer.firstObjectPos.x, clickEventer.firstObjectPos.y].action + "/1";

        if (isHiding)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 0.2f)
        {
            animalSpecialAbilityButton.SetActive(false);
            timer = 0;
            isHiding = false;
        }
    }

    public void ShowAnimalUI(string animalType)
    {
        animalSpecialAbilityButton.SetActive(true);
        
        if (animalType == "rabbit")
        {
            animalSpecialAbilityButtonText.text = "Dig Hole";
        }
        else if (animalType == "")
        { 
        
        }
    }

    public void HideAnimalUI()
    {
        isHiding = true;
    }
}
