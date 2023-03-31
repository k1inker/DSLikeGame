using UnityEngine;

namespace DS
{
    public class LoadSkinManager : MonoBehaviour
    {
        [SerializeField] private Avatar[] _avatars;
        [SerializeField] private Transform _parentObject;
        private void Awake()
        {
            SkinData[] models = SaveSystem.LoadSkin();
            uint index = 0;
            FindChosenModel(models, ref index);

            GetComponent<Animator>().avatar = _avatars[index];
            SwitchOnIndexModel(index);
        }

        private void SwitchOnIndexModel(uint index)
        {
            for (int i = 0; i < _parentObject.childCount; i++)
            {
                if (i == index)
                {
                    _parentObject.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        private void FindChosenModel(SkinData[] models, ref uint index)
        {
            foreach (var model in models)
                if (model.isChosen)
                    index = model.index;
        }
    }
}