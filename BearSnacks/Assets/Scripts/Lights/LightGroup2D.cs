using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightList2D : ScriptableObject
{
    [HideInInspector]
    public List<List<LightController2D>> lightList = new List<List<LightController2D>>();
    
    [HideInInspector]
    public int count = 0;
    
    [HideInInspector]
    public int height = 0;

    [HideInInspector]
    public int width = 0; 

    public List<List<LightController2D>> InitLightList(GameObject parent)
    {
        List<Transform> lightTransforms = new List<Transform>();

        for(int i = 0; i < parent.transform.childCount; i++) lightTransforms.Add(parent.transform.GetChild(i));

        List<float> pastXs = new List<float>();
        List<List<Transform>> columns = new List<List<Transform>>();

        foreach(Transform transform in lightTransforms)
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
        List<List<LightController2D>> newLightList = new List<List<LightController2D>>();

        foreach(List<Transform> column in columns)
        {
            newLightList.Add(new List<LightController2D>());
            foreach(Transform transform in column) 
            {
                newLightList[index].Add(transform.GetComponent<LightController2D>());
                count++;
            }
            index++;
        }

        lightList = newLightList;
        return newLightList;
    }

    private Vector2Int Search(LightController2D targetLight)
    {
        int x = 0;
        foreach(List<LightController2D> column in lightList)
        {
            int y = 0;
            foreach(LightController2D light in column)
            { 
                if(light == targetLight) return new Vector2Int(x, y);
                y++;
            }
            x++;
        }
        return new Vector2Int(-1, -1);
    }

    public LightController2D RightNeighbor(int x, int y)
    {
        if(x == lightList.Count - 1) return null;
        return lightList[x + 1][y];       
    }

    public LightController2D LeftNeighbor(int x, int y)
    {
        if(y == lightList[0].Count - 1) return null;
        return lightList[x - 1][y];       
    }

    public LightController2D TopNeighbor(int x, int y)
    {
        if(y == lightList[0].Count - 1) return null;
        return lightList[x][y - 1];       
    }

    public LightController2D BottomNeighbor(int x, int y)
    {
        if(y == lightList[0].Count - 1) return null;
        return lightList[x][y + 1];       
    }

    public LightController2D RightNeighbor(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        if(lightLocation.x == -1) return null;
        return RightNeighbor(lightLocation.x, lightLocation.y); 
    }

    public LightController2D LeftNeighbor(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        if(lightLocation.x == -1) return null;
        return LeftNeighbor(lightLocation.x, lightLocation.y);
    }

    public LightController2D TopNeighbor(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        if(lightLocation.x == -1) return null;
        return TopNeighbor(lightLocation.x, lightLocation.y);  
    }

    public LightController2D BottomNeighbor(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        if(lightLocation.x == -1) return null;
        return BottomNeighbor(lightLocation.x, lightLocation.y);
    }

    public LightController2D NextHorizontal(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        return NextHorizontal(lightLocation.x, lightLocation.y);
    }

    public LightController2D NextHorizontal(int x, int y)
    {
        LightController2D rightNeighbor = RightNeighbor(x, y);
        if(rightNeighbor == null)
        {
            Debug.Log("right neighbor null");
            if(BottomNeighbor(x, y) == null) return null;
            return lightList[0][y + 1];
        }
        return rightNeighbor;
    }

    public LightController2D NextVertical(LightController2D light)
    {
        Vector2Int lightLocation = Search(light);
        return NextVertical(lightLocation.x, lightLocation.y);
    }

    public LightController2D NextVertical(int x, int y)
    {
        LightController2D bottomNeighbor = BottomNeighbor(x, y);
        if(bottomNeighbor == null)
        {
            if(RightNeighbor(x, y) == null) return null;
            return lightList[x + 1][0];
        }
        return bottomNeighbor;
    }
}

public class LightGroup2D : MonoBehaviour
{
    [HideInInspector]
    public LightList2D lightList;

    void Start()
    {
        lightList = new LightList2D();
        lightList.InitLightList(gameObject);
    }

    public void Blink(float duration, int numberOfTimes)
    {
        foreach(List<LightController2D> lights in lightList.lightList)
        {
            foreach(LightController2D light in lights) light.Blink(duration, numberOfTimes);
        }
    }

    public void Off(float duration)
    {
        foreach(List<LightController2D> lights in lightList.lightList)
        {
            foreach(LightController2D light in lights) light.Off(duration);
        }
    }

    public void On(float duration)
    {
        foreach(List<LightController2D> lights in lightList.lightList)
        {
            foreach(LightController2D light in lights) light.On(duration);
        }
    }

    public void ChangeColor(float duration, List<Color> colors, int numberOfTimes, bool resetColor)
    {
        foreach(List<LightController2D> lights in lightList.lightList)
        {
            foreach(LightController2D light in lights) light.ChangeColor(duration, colors, numberOfTimes, resetColor);
        }
        
    }

    public void HorizontalBlinkCascade(float duration, int numberOfTimes)
    {
        StartCoroutine(HorizontalBlinkCascadeCoroutine(duration, numberOfTimes));
    }

    public void VerticalBlinkCascade(float duration, int numberOfTimes)
    {
        StartCoroutine(VerticalBlinkCascadeCoroutine(duration, numberOfTimes));
    }

    public void VerticalChangeColorCascade(float duration, List<Color> colors, int numberOfTimes, int lightGap, float targetIntensity)
    {
        StartCoroutine(VerticalChangeColorCascadeCoroutine(duration, colors, numberOfTimes, lightGap, targetIntensity));
    }

    public void VerticalChangeColorCascade(float duration, List<Color> colors, int numberOfTimes, int lightGap)
    {
        StartCoroutine(VerticalChangeColorCascadeCoroutine(duration, colors, numberOfTimes, lightGap, 1f));
    }

    public void HorizontalChangeColorCascade(float duration, List<Color> colors, int numberOfTimes, int lightGap, float targetIntensity)
    {
        StartCoroutine(HorizontalChangeColorCascadeCoroutine(duration, colors, numberOfTimes, lightGap, targetIntensity));
    }

    public void HorizontalChangeColorCascade(float duration, List<Color> colors, int numberOfTimes, int lightGap)
    {
        StartCoroutine(HorizontalChangeColorCascadeCoroutine(duration, colors, numberOfTimes, lightGap, 1f));
    }


    public List<Color> testColors;
    public void BlinkTest()
    {
        VerticalChangeColorCascade(10f, testColors, 3, 2);
    }

    public IEnumerator HorizontalBlinkCascadeCoroutine(float duration, int numberOfTimes)
    {
        float durationPerRound = duration/numberOfTimes;
        float pause = durationPerRound/lightList.count;

        for(int i = 0; i < numberOfTimes; i++)
        {
            lightList.lightList[0][0].Blink(pause, 1);

            yield return new WaitForSeconds(pause);

            if(lightList.NextHorizontal(0,0) != null) yield return StartCoroutine(HorizontalBlinkCascadeCoroutineRecursive(lightList.NextHorizontal(0,0), pause));
        }
    }

    public IEnumerator HorizontalBlinkCascadeCoroutineRecursive(LightController2D targetLight, float pause)
    {
        targetLight.Blink(pause, 1);
        yield return new WaitForSeconds(pause);
        if(lightList.NextHorizontal(targetLight) != null) yield return StartCoroutine(HorizontalBlinkCascadeCoroutineRecursive(lightList.NextHorizontal(targetLight), pause));
    }

    public IEnumerator VerticalBlinkCascadeCoroutine(float duration, int numberOfTimes)
    {
        float durationPerRound = duration/numberOfTimes;
        float pause = durationPerRound/lightList.count;

        for(int i = 0; i < numberOfTimes; i++)
        {
            lightList.lightList[0][0].Blink(pause, 1);

            yield return new WaitForSeconds(pause);

            if(lightList.NextVertical(0,0) != null) yield return StartCoroutine(VerticalBlinkCascadeCoroutineRecursive(lightList.NextVertical(0,0), pause));
        }
    }

    public IEnumerator VerticalBlinkCascadeCoroutineRecursive(LightController2D targetLight, float pause)
    {
        targetLight.Blink(pause, 1);
        yield return new WaitForSeconds(pause);
        if(lightList.NextVertical(targetLight) != null) yield return StartCoroutine(VerticalBlinkCascadeCoroutineRecursive(lightList.NextVertical(targetLight), pause));
    }

    public IEnumerator VerticalChangeColorCascadeCoroutine(float duration, List<Color> colors, int numberOfTimes, int lightGap, float targetIntensity)
    {
        float durationPerLightPerColor = duration/((colors.Count*numberOfTimes)+(lightGap*numberOfTimes-1));
        float durationPerRound = durationPerLightPerColor*(colors.Count+lightGap);
        if(numberOfTimes == 1) durationPerRound = duration;

        List<LightController2D> previousLights = new List<LightController2D>(); // init a list for the lights that have already been processed
        LightController2D currentLight = lightList.lightList[0][0]; // init the current light and set it to the first light
        previousLights.Add(currentLight);
        currentLight.ChangeIntensity(durationPerLightPerColor, targetIntensity);

        int gapCounter = 0;
        bool isNextLightsStarted = false;

        List<Color> currentColors = new List<Color>(colors);

        for(int j = 0; j < lightList.count + colors.Count; j++) // loop through all the lights
        {
            if(gapCounter == lightGap && isNextLightsStarted == false && numberOfTimes > 1)
            {
                isNextLightsStarted = true;
                StartCoroutine(VerticalChangeColorCascadeCoroutine(duration-durationPerRound, colors, numberOfTimes-1, lightGap, targetIntensity));
            }

            if(previousLights.Count >= currentColors.Count) // if there are more previous lights than colors available...
            {
                if(isNextLightsStarted == false) gapCounter++;

                LightController2D lightToBeRemoved = previousLights[previousLights.Count-1];
                lightToBeRemoved.ResetColor(durationPerLightPerColor); // reset the color of the last light to the original color
                lightToBeRemoved.ResetIntensity(durationPerLightPerColor);
                
                previousLights.RemoveAt(previousLights.Count-1); // remove the last light from the previous lights
            }

            int index = 0;
            foreach(LightController2D light in previousLights) // this loop updates the previous lights
            {
                light.ChangeColor(durationPerLightPerColor, currentColors[index]); // change the color to the next color on the list
                index++;
            }

            yield return new WaitForSeconds(durationPerLightPerColor); // wait while the colors change

            if(currentColors.Count == 1) break;

            LightController2D nextLight = lightList.NextVertical(currentLight); // set the next light to the next horizontal light
            if(nextLight == null) currentColors.RemoveAt(0);
            else 
            {
                currentLight = nextLight;
                previousLights.Insert(0, currentLight);
                currentLight.ChangeIntensity(durationPerLightPerColor, targetIntensity);
            }
        }
    }

    public IEnumerator HorizontalChangeColorCascadeCoroutine(float duration, List<Color> colors, int numberOfTimes, int lightGap, float targetIntensity)
    {
        float durationPerLightPerColor = duration/((colors.Count*numberOfTimes)+(lightGap*numberOfTimes-1));
        float durationPerRound = durationPerLightPerColor*(colors.Count+lightGap);
        if(numberOfTimes == 1) durationPerRound = duration;

        List<LightController2D> previousLights = new List<LightController2D>(); // init a list for the lights that have already been processed
        LightController2D currentLight = lightList.lightList[0][0]; // init the current light and set it to the first light
        previousLights.Add(currentLight);
        currentLight.ChangeIntensity(durationPerLightPerColor, targetIntensity);

        int gapCounter = 0;
        bool isNextLightsStarted = false;

        List<Color> currentColors = new List<Color>(colors);

        for(int j = 0; j < lightList.count + colors.Count; j++) // loop through all the lights
        {
            if(gapCounter == lightGap && isNextLightsStarted == false && numberOfTimes > 1)
            {
                isNextLightsStarted = true;
                StartCoroutine(HorizontalChangeColorCascadeCoroutine(duration-durationPerRound, colors, numberOfTimes-1, lightGap, targetIntensity));
            }

            if(previousLights.Count >= currentColors.Count) // if there are more previous lights than colors available...
            {
                if(isNextLightsStarted == false) gapCounter++;

                LightController2D lightToBeRemoved = previousLights[previousLights.Count-1];
                lightToBeRemoved.ResetColor(durationPerLightPerColor); // reset the color of the last light to the original color
                lightToBeRemoved.ResetIntensity(durationPerLightPerColor);
                
                previousLights.RemoveAt(previousLights.Count-1); // remove the last light from the previous lights
            }

            int index = 0;
            foreach(LightController2D light in previousLights) // this loop updates the previous lights
            {
                light.ChangeColor(durationPerLightPerColor, currentColors[index]); // change the color to the next color on the list
                index++;
            }

            yield return new WaitForSeconds(durationPerLightPerColor); // wait while the colors change

            if(currentColors.Count == 1) break;

            LightController2D nextLight = lightList.NextHorizontal(currentLight); // set the next light to the next horizontal light
            if(nextLight == null) currentColors.RemoveAt(0);
            else 
            {
                currentLight = nextLight;
                previousLights.Insert(0, currentLight);
                currentLight.ChangeIntensity(durationPerLightPerColor, targetIntensity);
            }
        }
    }
}
