using UnityEngine;
using UnityEngine.UI;
namespace DS {
    public class WeaponChanger : MonoBehaviour
    {
        [SerializeField] private Button _heavyWeaponBtn;
        [SerializeField] private Button _katanaWeaponBtn;
        [SerializeField] private Button _easyWeaponBtn;

        private WeaponType _currentType;
        private void Awake()
        {
            _currentType = SaveSystem.LoadWeapon();
            ChangeStateButtons();
        }
        private void ChangeStateButtons()
        {
            if (_currentType == WeaponType.EasySword)
            {
                _easyWeaponBtn.interactable = false;
                _katanaWeaponBtn.interactable = true;
                _heavyWeaponBtn.interactable = true;
            }
            else if (_currentType == WeaponType.OneHandHeavySword)
            {
                _easyWeaponBtn.interactable = true;
                _katanaWeaponBtn.interactable = true;
                _heavyWeaponBtn.interactable = false;
            }
            else if (_currentType == WeaponType.TwoHandHeavySword)
            {
                _easyWeaponBtn.interactable = true;
                _katanaWeaponBtn.interactable = false;
                _heavyWeaponBtn.interactable = true;
            }
        }
        public void SetTypeBtn(int type)
        {
            _currentType = (WeaponType)type;
            ChangeStateButtons();
            SaveSystem.SaveWeapon(_currentType);
        }
    }
}