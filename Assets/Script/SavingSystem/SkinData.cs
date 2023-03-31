[System.Serializable]
public class SkinData
{
    public bool inStock = false;
    public bool isChosen = false;
    public uint index = 0;
    public SkinData(bool inStock, bool isChosen, uint index)
    {
        this.inStock = inStock;
        this.isChosen = isChosen;
        this.index = index;
    }
}
