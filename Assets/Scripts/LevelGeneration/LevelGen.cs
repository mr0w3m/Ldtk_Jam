using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;


public enum TileGenType
{
    MainPath,
    Start,
    End,
    Extra
}

public class LevelGenChunk
{
    public TileGenType _genTileType = TileGenType.Extra;
}

[System.Serializable]
public class LevelGenData
{
    public Vector2Int coordinate;
    public bool fromBelow;
    public bool hasCeiling;

    public LevelGenData(Vector2Int coordinate, bool fromBelow, bool hasCeiling)
    {
        this.coordinate = coordinate;
        this.fromBelow = fromBelow;
        this.hasCeiling = hasCeiling;
    }
}

public class LevelGen : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chunksStartRoom;
    [SerializeField] private GameObject _chunkEndRoom_LR;
    [SerializeField] private List<GameObject> _chunks_LR;
    [SerializeField] private List<GameObject> _chunks_LRD;
    [SerializeField] private List<GameObject> _chunks_LRU;
    [SerializeField] private List<GameObject> _chunks_LRUD;
    [SerializeField] private GameObject _chunk_Exterior;


    [SerializeField] private Vector2 _chunkSize;

    [SerializeField] private int _levelSizeX = 4, _levelSizeY = 4;

    [SerializeField] private List<List<TileType>> generatedGrid = new List<List<TileType>>();
    [SerializeField] private List<LevelGenData> _mainPathLevelGenData = new List<LevelGenData>();
    [SerializeField] private Vector2Int _startingRoomCoordinate = Vector2Int.zero;
    [SerializeField] private Vector2Int _endRoomCoordinate = Vector2Int.zero;

    private Vector2 _playerPosition;
    
    public enum TileType 
    {
        LeftRight,
        LeftRightUp,
        LeftRightDown,
        LeftRightUpDown,
        StartingTile,
        EndingTile
    }

    private void Start()
    {
        GenerateLevel();

        PlacePlayer();
    }

    private void PlacePlayer()
    {
        if (Actor.i != null)
        {
            Actor.i.movement.SetPosition(_playerPosition);
        }
    }

    [ContextMenu("GenerateData")]
    private void GenerateLevel()
    {
        Vector2Int addLeft = new Vector2Int(-1, 0);
        Vector2Int addRight = new Vector2Int(1, 0);
        Vector2Int addUp = new Vector2Int(0, 1);
        Vector2Int addDown = new Vector2Int(0, -1);

        //determine start room
        _startingRoomCoordinate = new Vector2Int(Random.Range(0, _levelSizeX), 0);

        //then find a path between them


        //calculate amount of travel left or right for each row
        List<int> rowTravel = new List<int>();


        //sidenote: i could add functionality here to determine how much i care for duplicate rolls, which rn will never happen thanks to the while

        //Debug.Log("Build Travel data");
        int pastTravel = -1;
        for (int i = 0; i <= _levelSizeY - 1; i++)
        {
            int travel = Random.Range(0, 3);
            
            while (travel == pastTravel)
            {
                travel = Random.Range(0, 3);
            }
            
            if (i == 0 || i == _levelSizeY - 1)
            {
                //always at least a block of travel for first room and last room
                travel = Random.Range(1,3);
            }
            rowTravel.Add(travel);
            //Debug.Log(rowTravel[i]);
            pastTravel = travel;
        }

        Vector2Int lastRoomCoordinates = _startingRoomCoordinate;
        bool ignoreTheRest = false;
        bool firstLayer = true;
        
        //this is a loop through every row, bottom to top
        for (int y = 0; y < _levelSizeY; y++)
        {
            //bottom row
            //Debug.Log("Row: " + y + " | Row travel: " + rowTravel[y]);

            if (firstLayer)
            {
                firstLayer = false;
            }
            else
            {
                //if not the first row we go up one
                //Debug.Log("Adding Upward");
                lastRoomCoordinates += addUp;
            }
            ignoreTheRest = false;
            bool right = Random.value > 0.5;
            if (lastRoomCoordinates.x == _levelSizeX - 1)
            {

                //Debug.Log("setting left");
                right = false;
            }
            else if (lastRoomCoordinates.x == 0)
            {
                //Debug.Log("setting right");
                right = true;
            }
            //Debug.Log(right == true ? "right" : "left");

            //this one comes up from below
            _mainPathLevelGenData.Add(new LevelGenData(new Vector2Int(lastRoomCoordinates.x, lastRoomCoordinates.y), (y != 0), (rowTravel[y] > 0 ? true : false)));
            //Debug.Log("Added Coordinates: " + lastRoomCoordinates);

            for (int o = 0; o < rowTravel[y]; o++)
            {
                //for each point of travel across we need to place the next room, if this is a 0 value then we don't do anything here

                Vector2Int tempTestThis = (lastRoomCoordinates + (right ? addRight : addLeft));
                //Debug.Log("IsthisRoomGoingToBeInBounds? " + tempTestThis);
                //is this room going to be in bounds?

                //is the x negative? || is the x larger than the level size?
                if (tempTestThis.x < 0 || tempTestThis.x > _levelSizeX - 1)
                {
                    //it's out of bounds if we go here, so we don't want to place this if that's the case.
                    ignoreTheRest = true;
                    //Debug.Log("IgnoreTheRest");
                }

                bool lastRoomInSequence = false;

                //if we're not ignoring it then we'll add the offset, and add the item to the list
                if (!ignoreTheRest)
                {
                    lastRoomCoordinates += right ? addRight : addLeft;
                    //Debug.Log("PlacingThisRoom: " + lastRoomCoordinates);
                    Vector2Int tempTestNext = (lastRoomCoordinates + (right ? addRight : addLeft));
                    //Debug.Log("NextCoordinateWillBe: " + tempTestNext);
                    //check if it's the last in the sequence

                    bool nextWillBeIgnored = false;

                    if (tempTestNext.x < 0 || tempTestNext.x > _levelSizeX - 1)
                    {
                        //it's out of bounds if we go here, so we don't want to place this if that's the case.
                        nextWillBeIgnored = true;
                        //Debug.Log("NextWillBeIgnored");
                    }

                    if (o == rowTravel[y] - 1 || nextWillBeIgnored)
                    {
                        lastRoomInSequence = true;
                        //Debug.Log("LastRoomInSequenceTriggered");
                    }

                    _mainPathLevelGenData.Add(new LevelGenData(new Vector2Int(lastRoomCoordinates.x, lastRoomCoordinates.y), false, !lastRoomInSequence));
                    //Debug.Log("lastRoomInSequence: " + lastRoomInSequence);
                    //Debug.Log("Added Coordinates: " + lastRoomCoordinates);

                }


                //if it's the last row, and the last room in the sequence
                if (y == _levelSizeY - 1 && lastRoomInSequence)
                {
                    //Debug.Log("generating final room");
                    _endRoomCoordinate = lastRoomCoordinates;
                }
            }
        }


        //Debug.Log("StartingCoordinates: " + _startingRoomCoordinate);


        foreach (LevelGenData data in _mainPathLevelGenData)
        {
            //Debug.Log(data.coordinate);
            SpawnRoom(data);
        }

        for (int x = 0; x < _levelSizeX; x++)
        {
            for (int y = 0; y < _levelSizeY; y++)
            {
                bool onMainPath = false;
                foreach (LevelGenData data in _mainPathLevelGenData)
                {
                    if (data.coordinate.x == x && data.coordinate.y == y)
                    {
                        onMainPath = true;
                    }
                }

                if (!onMainPath)
                {
                    SpawnRoom(new LevelGenData(new Vector2Int(x,y), false, true));
                }
            }
        }

        for (int x = -1; x < _levelSizeX + 1; x++)
        {
            for (int y = -1; y < _levelSizeY + 1; y++)
            {
                if (x == -1 || x == _levelSizeX || y == -1 || y == _levelSizeY)
                {
                    //Debug.Log("SpawnExterior: " + x + ", " + y);
                    SpawnRoom(_chunk_Exterior, x, y);
                }
            }
        }
    }

    private void SpawnRoom(LevelGenData data)
    {
        GameObject go = Instantiate(ReturnRoom(data));
        go.transform.position = new Vector3(data.coordinate.x * _chunkSize.x, data.coordinate.y * _chunkSize.y, 0);

        LevelChunkObject levelChunkRef = go.GetComponent<LevelChunkObject>();

        //set the player position if it's the first chunk
        if (data.coordinate.x == _startingRoomCoordinate.x && data.coordinate.y == _startingRoomCoordinate.y)
        {
            if (levelChunkRef != null)
            {
                Debug.Log("Setting Player Position to " + levelChunkRef.PlayerStartPosition.position.ToString());
                _playerPosition = levelChunkRef.PlayerStartPosition.position;
            }
            else
            {
                Debug.Log("Setting Player Position to Default Center Room");
                _playerPosition = new Vector2((data.coordinate.x * _chunkSize.x) + (_chunkSize.x / 2), (data.coordinate.y * _chunkSize.y) + (_chunkSize.y / 2));
            }
        }

    }

    //outerwall override
    private void SpawnRoom(GameObject room, int x, int y)
    {
        GameObject go = Instantiate(room);
        go.transform.position = new Vector3(x * _chunkSize.x, y * _chunkSize.y, 0);
    }

    private GameObject ReturnRoom(LevelGenData data)
    {
        if (data.coordinate.x == _startingRoomCoordinate.x && data.coordinate.y == _startingRoomCoordinate.y)
        {
            GameObject gameObjectRef = _chunksStartRoom[Random.Range(0, _chunksStartRoom.Count)];
            return gameObjectRef;
        }
        else if (data.coordinate.x == _endRoomCoordinate.x && data.coordinate.y == _endRoomCoordinate.y)
        {
            return _chunkEndRoom_LR;
        }
        else if (data.fromBelow && !data.hasCeiling)
        {
            return _chunks_LRUD[Random.Range(0, _chunks_LRUD.Count)];
        }
        else if (data.fromBelow && data.hasCeiling)
        {
            return _chunks_LRD[Random.Range(0, _chunks_LRD.Count)];
        }
        else if (!data.hasCeiling)
        {
            return _chunks_LRU[Random.Range(0, _chunks_LRU.Count)];
        }
        else
        {
            return _chunks_LR[Random.Range(0, _chunks_LR.Count)];
        }
    }
}
