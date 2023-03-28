using UnityEngine;
using UnityEngine.UI;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private SkinData[] info;
    [SerializeField] private Transform parentObject;

    public Button buyButton;
    public int index;
    private void Awake()
    {
        info = SaveSystem.LoadInvetory(this);
        for(int i = 0; i < info.Length; i++)
        {
            if(info[i].isChosen == true)
            {
                parentObject.GetChild(i).gameObject.SetActive(true);
                index = i;
            }
            else
            {
                parentObject.GetChild(i).gameObject.SetActive(false);
            }
                
        }

        buyButton.gameObject.SetActive(false);
    }
    public void ScrollRight()
    {
        if(index < parentObject.childCount - 1)
        {
            parentObject.GetChild(index).gameObject.SetActive(false);
            index++;
            parentObject.GetChild(index).gameObject.SetActive(true);
            if (info[index].inStock)
            {
                info[index].isChosen = true;
                info[index-1].isChosen = false;
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);
            }
        }
    }
    public void ScrollLeft()
    {
        if (index > 0)
        {
            parentObject.GetChild(index).gameObject.SetActive(false);
            index--;
            parentObject.GetChild(index).gameObject.SetActive(true);
            if (info[index].inStock)
            {
                info[index].isChosen = true;
                info[index+1].isChosen = false;
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);
            }
        }
    }
    public void BuyButtonAction()
    {
        //add adds
        info[index].inStock = true;
        info[index].isChosen = true;
        for(int i = 0; i < info.Length; i++)    
        {
            if(i != index)
            {
                info[i].isChosen = false;
            }
        }
        buyButton.gameObject.SetActive(false);
        SaveSystem.SaveInvetory(info);
    }
    public void SaveChooseSkinBtn()
    {
        SaveSystem.SaveInvetory(info);
    }
    public SkinData[] FirstLoadInvetory()
    {
        info[0].isChosen = true;
        info[0].inStock = true;
        info[0].index = 0;
        return info;
    }
}
