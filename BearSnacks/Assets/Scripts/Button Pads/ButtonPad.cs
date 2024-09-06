using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPadButton : MonoBehaviour
{
    [HideInInspector]
    public ButtonPad parent;

    [HideInInspector]
    public Vector2Int location;

    private void OnMouseDown()
    {
        parent.PressButton(location);
    }

    private void OnMouseUp()
    {
        parent.ReleaseButton(location);
    }
}

public class ButtonPad : MonoBehaviour
{
    [SerializeField]
    private bool hasTwoButtonPads = false;

    private List<List<GameObject>> buttonList;
    private GameObject buttonsParent;
    private Sprite unpressedSprite;
    public Sprite pressedSprite;

    [HideInInspector]
    public Vector2Int currentLocation;

    public UnityEvent onPress;
    public UnityEvent onRelease;

    void Start()
    {
        buttonsParent = gameObject;
        buttonList = InitButtonList(buttonsParent);

        ApplyComponents(buttonList);
        unpressedSprite = buttonList[0][0].GetComponent<SpriteRenderer>().sprite;
    }

    public List<List<GameObject>> InitButtonList(GameObject parent)
    {
        List<Transform> buttonTransforms = new List<Transform>();

        for(int i = 0; i < parent.transform.childCount; i++) buttonTransforms.Add(parent.transform.GetChild(i));

        List<float> pastXs = new List<float>();
        List<List<Transform>> columns = new List<List<Transform>>();

        foreach(Transform transform in buttonTransforms)
        {
            if(!pastXs.Contains(transform.position.x))
            {
                pastXs.Add(transform.position.x);
                List<Transform> column = new List<Transform>();
                column.Add(transform);
                columns.Add(column);
                continue;
            }

            foreach(List<Transform> column in columns) if(column[0].position.x == transform.position.x) column.Add(transform);
        }

        foreach(List<Transform> column in columns) column.Sort((a, b) => b.position.y.CompareTo(a.position.y));

        columns.Sort((listA, listB) => listA[0].position.x.CompareTo(listB[0].position.x));

        int index = 0;
        List<List<GameObject>> newButtonList = new List<List<GameObject>>();

        foreach(List<Transform> column in columns)
        {
            newButtonList.Add(new List<GameObject>());
            foreach(Transform transform in column) 
            {
                newButtonList[index].Add(transform.gameObject);
            }
            index++;
        }

        return newButtonList;
    }

    private void ApplyComponents(List<List<GameObject>> buttonPad)
    {
        int x = 0;
        foreach(List<GameObject> column in buttonPad)
        {
            int y = 0;
            foreach(GameObject button in column)
            {
                button.AddComponent<BoxCollider2D>();
                ButtonPadButton currentButton = button.AddComponent<ButtonPadButton>();
                currentButton.parent = this;
                currentButton.location = new Vector2Int(x, y);
                y++;
            }
            x++;
        }
    }

    public void PressButton(Vector2Int location)
    {
        currentLocation = location;
        buttonList[location.x][location.y].GetComponent<SpriteRenderer>().sprite = pressedSprite;
        onPress.Invoke();
    }

    public void ReleaseButton(Vector2Int location)
    {
        buttonList[location.x][location.y].GetComponent<SpriteRenderer>().sprite = unpressedSprite;
        onRelease.Invoke();
    }
}
