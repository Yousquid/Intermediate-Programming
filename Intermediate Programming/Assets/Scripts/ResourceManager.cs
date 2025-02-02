using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourceManager : MonoBehaviour
{
    public int food = 0;
    public TextMeshProUGUI foodNumberText;
    public GridManager gridManager;
    public TrunManager trunManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodNumberText.text = "Food: " + food;
    }
}
