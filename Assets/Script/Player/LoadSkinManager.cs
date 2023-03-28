using UnityEngine;

public class LoadSkinManager : MonoBehaviour
{
    [SerializeField] private Avatar[] _avatars;
    [SerializeField] private Transform _parentObject;
    private void Awake()
    {
        uint index = SaveSystem.LoadModel();
        GetComponent<Animator>().avatar = _avatars[index];
        for(int i = 0; i < _parentObject.childCount; i++)
        {
            if(i == index)
            {
                _parentObject.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
