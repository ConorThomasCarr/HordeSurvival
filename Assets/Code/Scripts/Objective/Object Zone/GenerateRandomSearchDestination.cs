using AI.BaseCharacters;

using UnityEngine;

public class GenerateRandomSearchDestination
{
    private ICharacters _characters;
   
    private int _objectiveIndex;

    private int _searchPositionArrayRow;

    private int _searchPositionArrayColumn;

    private Vector3[,] _currentSearchPositionArray;

    public bool hasGenerateArrayRowAndColum {get; set;}
    public bool hasClearArrayRowAndColumn {get; set;}
    
    public void Initialize(ICharacters character)
    {
        _characters = character;
        EventManager.AddListener<OnUpdateSearchPositionArray>(UpdateSearchPositionArray);
    }
    
    public void GenerateArrayRowAndColumn()
    {
        _searchPositionArrayRow = (Random.Range(1, 100));
        _searchPositionArrayColumn = (Random.Range(1, 100));

        if (_currentSearchPositionArray != null && _currentSearchPositionArray[_searchPositionArrayRow, _searchPositionArrayColumn] == Vector3.zero)
        {
            _searchPositionArrayRow = 0;
            _searchPositionArrayColumn = 0;
        }
        else
        {
            var evtExit = SearchAreaObjective.OnCreatePoint;
            evtExit.ObjectiveIndex = (Random.Range(1, 5)) - 1;
            evtExit.SearchPositionArrayRow = _searchPositionArrayRow;
            evtExit.SearchPositionArrayColumn = _searchPositionArrayColumn;
            EventManager.Broadcast(evtExit);
        }

        hasGenerateArrayRowAndColum = true;
        hasClearArrayRowAndColumn = false;
    }
    
    public void ClearArrayRowAndColumn()
    {
        var evtExit = SearchAreaObjective.OnUpdateSearchPositionArray;
        evtExit.PositionArray[_searchPositionArrayRow, _searchPositionArrayColumn] = Vector3.zero;
        EventManager.Broadcast(evtExit);

        _searchPositionArrayRow = 0;
        _searchPositionArrayColumn = 0;
        
        hasGenerateArrayRowAndColum = false;
        hasClearArrayRowAndColumn = true;
    }


    private void UpdateSearchPositionArray(OnUpdateSearchPositionArray updateSearchPositionArray)
    {
        if (updateSearchPositionArray.PositionArray[_searchPositionArrayRow, _searchPositionArrayColumn] != Vector3.zero)
        {
           _characters.SetDestination(updateSearchPositionArray.PositionArray[_searchPositionArrayRow, _searchPositionArrayColumn]);
        }
    }
}
