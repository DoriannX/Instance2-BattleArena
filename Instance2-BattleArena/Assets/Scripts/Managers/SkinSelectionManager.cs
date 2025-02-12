using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkinSelectionManager : MonoBehaviour
{
    public ExpManager ExpManager;

    public GameObject PanelSelectClass;
    public GameObject PanelSelectSkin;

    [SerializeField] private PlayerClassManager _playerClassManager;

    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private List<Sprite> CharacterClassesShield;
    [SerializeField] private List<Sprite> CharacterClassesSoldier;
    [SerializeField] private List<Sprite> CharacterClassesPort;
    [SerializeField] private List<Image> ChangeableSkin;

    private GameObject _playerInstance;

    public void SelectSkinCharacter()
    {
        _playerClassManager.SelectClass(_playerClassManager.selectedClassIndex);

        if (_playerClassManager.selectedClassIndex == 0)
        {
            ChangeableSkin[0].sprite = CharacterClassesShield[0];
            ChangeableSkin[1].sprite = CharacterClassesShield[1];
            ChangeableSkin[2].sprite = CharacterClassesShield[2];
            ChangeableSkin[3].sprite = CharacterClassesShield[3];
        }

        else if (_playerClassManager.selectedClassIndex == 1)
        {
            ChangeableSkin[0].sprite = CharacterClassesSoldier[0];
            ChangeableSkin[1].sprite = CharacterClassesSoldier[1];
            ChangeableSkin[2].sprite = CharacterClassesSoldier[2];
            ChangeableSkin[3].sprite = CharacterClassesSoldier[3];
        }

        else if (_playerClassManager.selectedClassIndex == 2)
        {
            ChangeableSkin[0].sprite = CharacterClassesPort[0];
            ChangeableSkin[1].sprite = CharacterClassesPort[1];
            ChangeableSkin[2].sprite = CharacterClassesPort[2];
            ChangeableSkin[3].sprite = CharacterClassesPort[3];
        }

        if (ExpManager != null)
        {
            ExpManager.Initialize(_playerClassManager.selectedClass, _playerInstance);
        }
    }

    public void SkinBackButton() 
    { 
        PanelSelectClass.SetActive(true);
        PanelSelectSkin.SetActive(false);
    }

    public void SkinSelectedButton()
    {
        PanelSelectSkin.SetActive(false);

        if (_playerClassManager.selectedClassIndex == 0)
        {
            _playerInstance = Instantiate(_playerClassManager.PlayerPrefabShield, _playerSpawn.position, Quaternion.identity);
        }
        else if (_playerClassManager.selectedClassIndex == 1)
        {
            _playerInstance = Instantiate(_playerClassManager.PlayerPrefabSoldier, _playerSpawn.position, Quaternion.identity);
        }
        else if (_playerClassManager.selectedClassIndex == 2)
        {
            _playerInstance = Instantiate(_playerClassManager.PlayerPrefabCarrier, _playerSpawn.position, Quaternion.identity);
        }
    }
}