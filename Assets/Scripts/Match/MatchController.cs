using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = System.Random;


namespace Main
{
public enum ElementType
{
    Fire = 0, //0
    Earth = 1, //1
    Metal = 2, //2
    Water = 3, //3
    Wood = 4 //4
}

public class MatchController : MonoBehaviour
{
    [Header("Legendaries")] 
    [FormerlySerializedAs("leg_Fire")] [SerializeField] private GameObject legFire;
    [FormerlySerializedAs("leg_Water")] [SerializeField] private GameObject legWater;
    [FormerlySerializedAs("leg_Metal")] [SerializeField] private GameObject legMetal;
    [FormerlySerializedAs("leg_Earth")] [SerializeField] private GameObject legEarth;
    [FormerlySerializedAs("leg_Wood")] [SerializeField] private GameObject legWood;
    
    [Header("Parameters")]
    [FormerlySerializedAs("_warningsPannel")] [SerializeField]private WarningsPannel warningsPannel;
    [FormerlySerializedAs("HexagramCounter")] [SerializeField] private int hexagramCounter;
    [FormerlySerializedAs("HexagramParts")]
    [SerializeField] private GameObject[] hexagramParts;
    [FormerlySerializedAs("HUD")]
    [SerializeField] private GameObject hud;
    [FormerlySerializedAs("ChooseYourElementButtonsHUD")]
    [SerializeField] private GameObject chooseYourElementButtonsHUD;
    [SerializeField] private GameObject[] buttons;
    [FormerlySerializedAs("Terrain")] [SerializeField] private GameObject[] terrain;
    [FormerlySerializedAs("HexagramNumberArray")] [SerializeField] private List<int> hexagramNumberArray;
    [SerializeField] private GameObject terrainParent;
    [SerializeField] private GameObject terrainPosition;
    [SerializeField] private GameObject CanvasCameraChange;
    private bool _hexagramChangeColor;
    private bool _conversionHasBegun;
    private bool _matchHasStarted;
    private bool _playerOneTurn ;
    private List<int> _hexBeingBuilt = new List<int>();
    //variable made to keep the element the first player picks
    private List<int> _elements = new List<int>();
    private float _startTime;
    private static readonly int Activate = Animator.StringToHash("Activate");
    private float _colorChangeSpeed;
    private Material _hexMaterial;
    private Color _color1;
    private Color _color2;
    private Color _targetColor;
    private static readonly int Color3 = Shader.PropertyToID("_Color");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int F = Shader.PropertyToID("_Float");
    private static readonly int HolderSpin = Animator.StringToHash("HolderSpin");
    private static readonly int Levitate = Animator.StringToHash("Levitate");
    private static readonly int red = Animator.StringToHash("Red");
    private static readonly int brown = Animator.StringToHash("Brown");
    private static readonly int black = Animator.StringToHash("Black");
    private static readonly int blue = Animator.StringToHash("Blue");
    private static readonly int green = Animator.StringToHash("Green");
    private static readonly int release = Animator.StringToHash("release");
    
    //generate new tokenID
    //public static GameObject objectNFT;
    //activate claimNFT object
    private static GameManager gameManager;
    


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(hexagramParts[hexagramCounter].GetComponent<MeshRenderer>().material.color);
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (hexagramCounter == 6)
        { 
            CanvasCameraChange.transform.GetChild(0).gameObject.SetActive(true);
            hexagramCounter = 10;
            var position = terrainPosition.transform.position;
            terrainParent.GetComponent<Animator>().SetTrigger(Levitate);
            gameManager.objectNFT = (hexagramNumberArray[0], hexagramNumberArray[1], hexagramNumberArray[2]) switch
            {
                (0, 0, 0) => Instantiate(terrain[0].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (1, 0, 0) => Instantiate(terrain[1].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (0, 1, 0) => Instantiate(terrain[2].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (0, 0, 1) => Instantiate(terrain[3].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (1, 1, 0) => Instantiate(terrain[4].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (0, 1, 1) => Instantiate(terrain[5].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (1, 0, 1) => Instantiate(terrain[6].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                (1, 1, 1) => Instantiate(terrain[7].gameObject, position, Quaternion.identity,
                    terrainParent.transform),
                _ => gameManager.objectNFT
            };

            //hexagramParts[HexagramCounter - 1].transform.parent.GetComponent<Animator>().SetTrigger(HolderSpin);
           
            
            
        }

        if (!_hexagramChangeColor) return;
        if (_conversionHasBegun)
        {
            ReleaseLegAnim(_elements[_elements.Count-1]);
            ReleaseLegAnim(_elements[_elements.Count-2]);
            _startTime = Time.deltaTime;
            _conversionHasBegun = false;
            _hexMaterial = hexagramParts[hexagramCounter].GetComponent<MeshRenderer>().material;
            _conversionHasBegun = !_conversionHasBegun;
            _color1 = _hexMaterial.GetColor(Color3);
            _color2 = _hexMaterial.GetColor(BaseColor);
            _colorChangeSpeed = 0.0025f;

        }
        Debug.Log(_targetColor);
        var t = (Time.time - _startTime) * _colorChangeSpeed;
        _hexMaterial.SetColor(Color3, Color.Lerp(_color1, _targetColor, t));
        _hexMaterial.SetColor(BaseColor,Color.Lerp(_color2, _targetColor, t));
        _hexMaterial.SetFloat(F, Mathf.Round(Mathf.Lerp(_hexMaterial.GetFloat(F), 1, t * 0.2f)));
        //Debug.Log(hexMaterial.GetFloat(F).ToString());
        
        if (_hexMaterial.GetColor(Color3) != _targetColor || hexagramCounter >= 6) return;
        if (hexagramCounter + 1 != 6)
        {
            MatchStart();
        
            for (var i = 0; i < 5; i++)
            {
                buttons[i].GetComponent<Button>().enabled = true;
            }    
            _hexagramChangeColor = false;
        }
        
        hexagramCounter++;
        if (hexagramCounter < 6)
        {
            hexagramParts[hexagramCounter].GetComponent<Animator>().SetTrigger(Activate);
        }
    }

    private IEnumerator Release()
    {
        legEarth.GetComponent<Animator>().SetTrigger(release);
        legWater.GetComponent<Animator>().SetTrigger(release);
        legMetal.GetComponent<Animator>().SetTrigger(release);
        legWood.GetComponent<Animator>().SetTrigger(release);
        legFire.GetComponent<Animator>().SetTrigger(release);
        yield return new  WaitForSeconds(0);
    }
    private void ReleaseLegAnim(int leg)
    {
        switch (leg)
        {
            case 0:
                legFire.GetComponent<Animator>().SetTrigger(release);
                break;
            case 1:
                legEarth.GetComponent<Animator>().SetTrigger(release);
                break;
            case 2:
                legMetal.GetComponent<Animator>().SetTrigger(release);
                break;
            case 3:
                legWater.GetComponent<Animator>().SetTrigger(release);
                break;
            case 4:
                legWood.GetComponent<Animator>().SetTrigger(release);
                break;
        }

    }
    
    //Used in the button to start the match
    public void MatchStart()
    {
        hexagramParts[hexagramCounter].GetComponent<Animator>().SetTrigger(Activate);
        hud.SetActive(false);
        _matchHasStarted = true;
        chooseYourElementButtonsHUD.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    public void ElementPicker(int element)
    { 
        _elements.Add(element);

        switch (element)
        {
            case 0 :
                legFire.GetComponent<Animator>().SetTrigger(red);
                break;    
            case 1 :
                legEarth.GetComponent<Animator>().SetTrigger(brown);
                break;            
            case 2 :
                legMetal.GetComponent<Animator>().SetTrigger(black);
                break;           
            case 3 :
                legWater.GetComponent<Animator>().SetTrigger(blue);
                break;           
            case 4 :
                legWood.GetComponent<Animator>().SetTrigger(green);
                break;
        }
        
        Debug.Log(System.String.Join("", _elements.ConvertAll(i => i.ToString()).ToArray()));
        for (var i = 0; i < 5; i++)
        {
            buttons[i].GetComponent<Button>().enabled = false;
        }
        
        StartCoroutine(WaitForNpc());
     
    }

    private IEnumerator WaitForNpc()
    {
        yield return new WaitForSeconds(0.3f);
        chooseYourElementButtonsHUD.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        
        //NPC Playing v
        //chainlink
        var randomNpcChoice = UnityEngine.Random.Range(0, 5);
        if (randomNpcChoice != _elements[_elements.Count - 1])
        {
            switch (randomNpcChoice)
            {
                case 0:
                    legFire.GetComponent<Animator>().SetTrigger(red);
                    break;
                case 1:
                    legEarth.GetComponent<Animator>().SetTrigger(brown);
                    break;
                case 2:
                    legMetal.GetComponent<Animator>().SetTrigger(black);
                    break;
                case 3:
                    legWater.GetComponent<Animator>().SetTrigger(blue);
                    break;
                case 4:
                    legWood.GetComponent<Animator>().SetTrigger(green);
                    break;
            }
        }

        _elements.Add(randomNpcChoice);
        Debug.Log("Npc choose :" + Enum.GetName(typeof(ElementType), randomNpcChoice));
        warningsPannel.StartBlinkForSeconds("Npc choose :" + Enum.GetName(typeof(ElementType), randomNpcChoice));
        yield return new WaitForSeconds(2);
        if (randomNpcChoice != _elements[_elements.Count-2])
        {
            ChangeHexagram();
        }
        else
        {
            ReleaseLegAnim(randomNpcChoice);
            warningsPannel.StartBlinkForSeconds("The elements tied");
            _elements.RemoveAt(_elements.Count-1);
            _elements.RemoveAt(_elements.Count-1);
            MatchStart();
            for (var i = 0; i < 5; i++)
            {
                buttons[i].GetComponent<Button>().enabled = true;
            } 
        }
    }

    void ChangeHexagram()
    {
        
        var hex = hexagramParts[hexagramCounter];
        var hexMaterial =  hexagramParts[hexagramCounter].GetComponent<MeshRenderer>().material;
        //hex.GetComponent<Animator>().SetTrigger(Activate);
        hexagramNumberArray.Add(CheckMatchResult(_elements[_elements.Count - 2], _elements[_elements.Count - 1]) ? 1 : 0);
        Debug.Log("AAAAAAAAAAAAAAAAAAAAA");
        _targetColor = CheckMatchResult(_elements[_elements.Count-2], _elements[_elements.Count-1]) ? Color.white : Color.black;
        Debug.Log(_targetColor);
        _hexagramChangeColor = true;
        _conversionHasBegun = true;
        
        
    }
    
    
    //the following method checks if the two elements are creating or destroying one another and returns a bool
    bool CheckMatchResult(int firstElementValue, int secondElementValue)
    {

        return (FirstElementValue: firstElementValue, SecondElementValue: secondElementValue) switch
        {
            //every possible combination
            (0, 1) => true,
            (1, 0) => true,
            (1, 2) => true,
            (2, 1) => true,
            (2, 3) => true,
            (3, 2) => true,
            (3, 4) => true,
            (4, 3) => true,
            (0, 4) => true,
            (4, 0) => true,
            _ => false
        };
    }
 
}
}