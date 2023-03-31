using UnityEngine;
using UnityEngine.UI;
namespace DS
{
    public class SkinChanger : MonoBehaviour
    {
        [SerializeField] private static SkinData[] info = new SkinData[]
        {
            new SkinData(true, true, 0),
            new SkinData (false, false, 1)
        };
        
        [SerializeField] private Transform parentObject;

        public Button buyButton;
        public int index;
        private void Awake()
        {
            
            info = SaveSystem.LoadSkin();
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].isChosen == true)
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
            if (index < parentObject.childCount - 1)
            {
                SwitchCurrentToNextImg(+1);
                if (info[index].inStock)
                {
                    info[index].isChosen = true;
                    info[index - 1].isChosen = false;
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
                SwitchCurrentToNextImg(-1);
                if (info[index].inStock)
                {
                    info[index].isChosen = true;
                    info[index + 1].isChosen = false;
                    buyButton.gameObject.SetActive(false);
                }
                else
                {
                    buyButton.gameObject.SetActive(true);
                }
            }
        }

        private void SwitchCurrentToNextImg(short dirX)
        {
            parentObject.GetChild(index).gameObject.SetActive(false);
            index += dirX;
            parentObject.GetChild(index).gameObject.SetActive(true);
        }

        public void BuyButtonAction()
        {
            //add adds
            info[index].inStock = true;
            info[index].isChosen = true;
            for (int i = 0; i < info.Length; i++)
            {
                if (i != index)
                {
                    info[i].isChosen = false;
                }
            }
            buyButton.gameObject.SetActive(false);
            SaveSystem.SaveSkin(info);
        }
        public void SaveChooseSkinBtn()
        {
            SaveSystem.SaveSkin(info);
        }
        public static SkinData[] FirstLoadInvetory()
        {
            info[0].isChosen = true;
            info[0].inStock = true;
            info[0].index = 0;
            return info;
        }
    }
}