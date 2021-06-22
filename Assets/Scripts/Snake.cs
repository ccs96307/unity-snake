using UnityEngine;
using System.Collections.Generic;


public abstract class SnakeBase : MonoBehaviour
{
    // Username
    public string _username;

    // Speed
    protected bool _speedUp;
    protected float _basicMoveSpeed;
    protected float _moveSpeed;

    // Food
    public GameObject _smallFoodObject;
    public GameObject _bigFoodObject;
    protected int _dropFoodFrame;

    // Move
    protected Vector3 _direction;
    protected Vector3 _newDirection;
    protected Vector3 _oldDirection = new Vector3(-0.01f, 0.0f, 0.0f);

    // Position
    public List<Vector3> _positionList;

    // Rotation
    protected float _playerRotation = 0f;

    // Score
    public int _score;

    // Sprite
    public Dictionary<int, string> _index2snakeSprite = new Dictionary<int, string>();
    public Dictionary<string, string> _spriteHeadTable = new Dictionary<string, string>();
    public Dictionary<string, string> _spriteBodyTable = new Dictionary<string, string>();

    //public Sprite[] _spriteHeadArray;
    //public Sprite[] _spriteBodyArray;

    public Sprite _headSprite;
    public Sprite _bodySprite;
    protected int _spriteIndex;

    // Body Info
    public GameObject _bodyGameObject;
    public List<GameObject> _bodyList;
    public List<int> _bodyPositionIndex;
    protected int _bodyNum;
    protected float _bodyGrowSize;
    protected int _bodyGrowStep;

    protected int _basicDistanceWithEachOther = 7;
    protected int _distanceWithEachOther;
    protected int _distanceWithEachOtherStep;
    protected bool _positionRecordEnable;

    protected Dictionary<int, string> _InitializeIndex2Sprite()
    {
        _index2snakeSprite.Add(0, "hide");
        _index2snakeSprite.Add(1, "blue");
        _index2snakeSprite.Add(2, "cyan");
        _index2snakeSprite.Add(3, "green");
        _index2snakeSprite.Add(4, "grey");
        _index2snakeSprite.Add(5, "iron");
        _index2snakeSprite.Add(6, "orange");
        _index2snakeSprite.Add(7, "pink");
        _index2snakeSprite.Add(8, "purple");
        _index2snakeSprite.Add(9, "red");
        _index2snakeSprite.Add(10, "white");
        _index2snakeSprite.Add(11, "yellow");

        return _index2snakeSprite;
    }

    protected string _colorName2HeadSprite(string colorName)
    {
        _spriteHeadTable.Add("hide", "Heads/hide_snake_head");
        _spriteHeadTable.Add("blue", "Heads/blue_snake_head");
        _spriteHeadTable.Add("cyan", "Heads/cyan_snake_head");
        _spriteHeadTable.Add("green", "Heads/green_snake_head");
        _spriteHeadTable.Add("grey", "Heads/grey_snake_head");
        _spriteHeadTable.Add("iron", "Heads/iron_snake_head");
        _spriteHeadTable.Add("orange", "Heads/orange_snake_head");
        _spriteHeadTable.Add("pink", "Heads/pink_snake_head");
        _spriteHeadTable.Add("purple", "Heads/purple_snake_head");
        _spriteHeadTable.Add("red", "Heads/red_snake_head");
        _spriteHeadTable.Add("white", "Heads/white_snake_head");
        _spriteHeadTable.Add("yellow", "Heads/yellow_snake_head");

        return _spriteHeadTable[colorName];
    }

    protected string _colorName2BodySprite(string colorName)
    {
        _spriteBodyTable.Add("hide", "Bodies/hide_snake_body");
        _spriteBodyTable.Add("blue", "Bodies/blue_snake_body");
        _spriteBodyTable.Add("cyan", "Bodies/cyan_snake_body");
        _spriteBodyTable.Add("green", "Bodies/green_snake_body");
        _spriteBodyTable.Add("grey", "Bodies/grey_snake_body");
        _spriteBodyTable.Add("iron", "Bodies/iron_snake_body");
        _spriteBodyTable.Add("orange", "Bodies/orange_snake_body");
        _spriteBodyTable.Add("pink", "Bodies/pink_snake_body");
        _spriteBodyTable.Add("purple", "Bodies/purple_snake_body");
        _spriteBodyTable.Add("red", "Bodies/red_snake_body");
        _spriteBodyTable.Add("white", "Bodies/white_snake_body");
        _spriteBodyTable.Add("yellow", "Bodies/yellow_snake_body");

        return _spriteBodyTable[colorName];
    }

    protected Vector3 _Normalization(Vector3 direction)
    {
        double normal = direction.x * direction.x + direction.y * direction.y;
        normal = System.Math.Sqrt(normal);

        direction.x *= (1 / (float)normal);
        direction.y *= (1 / (float)normal);
        direction.z = 0f;

        return direction;
    }

    protected int _ScoreChangeBodyNum(int score)
    {
        int moreBody = score / 10 + 4;
        return moreBody;
    }
}
