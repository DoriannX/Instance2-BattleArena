using UnityEngine;
using System.Collections.Generic;
using Managers;
using Unity.Netcode;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkinSelectionManager : MonoBehaviour
{
    public ExpManager ExpManager;

    public GameObject PanelSelectClass;
    public GameObject PanelSelectSkin;

    [SerializeField] private PlayerClassManager _playerClassManager;

    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private List<Sprite> _characterClassesShield;
    [SerializeField] private List<Sprite> _characterClassesSoldier;
    [SerializeField] private List<Sprite> _characterClassesPort;
    [SerializeField] private List<Image> _changeableSkin;

    private GameObject _playerInstance;

    public void SelectSkinCharacter()
    {
        _playerClassManager.SelectClass(_playerClassManager.SelectedClassIndex);

        if (_playerClassManager.SelectedClassIndex == 0)
        {
            _changeableSkin[0].sprite = _characterClassesShield[0];
            _changeableSkin[1].sprite = _characterClassesShield[1];
            _changeableSkin[2].sprite = _characterClassesShield[2];
            _changeableSkin[3].sprite = _characterClassesShield[3];
        }

        else if (_playerClassManager.SelectedClassIndex == 1)
        {
            _changeableSkin[0].sprite = _characterClassesSoldier[0];
            _changeableSkin[1].sprite = _characterClassesSoldier[1];
            _changeableSkin[2].sprite = _characterClassesSoldier[2];
            _changeableSkin[3].sprite = _characterClassesSoldier[3];
        }

        else if (_playerClassManager.SelectedClassIndex == 2)
        {
            _changeableSkin[0].sprite = _characterClassesPort[0];
            _changeableSkin[1].sprite = _characterClassesPort[1];
            _changeableSkin[2].sprite = _characterClassesPort[2];
            _changeableSkin[3].sprite = _characterClassesPort[3];
        }

        if (ExpManager != null)
        {
            ExpManager.Initialize(_playerClassManager.SelectedClass, _playerInstance);
        }
    }

    public void SkinBackButton()
    {
        PanelSelectClass.SetActive(true);
        PanelSelectSkin.SetActive(false);
    }

    private void SelectSkin(int index)
    {
        //TODO: refaire script entier logique changement de skin
    }
}