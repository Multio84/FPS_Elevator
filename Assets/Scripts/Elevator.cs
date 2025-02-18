using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    LevelGenerator generator;

    [SerializeField] Transform buttonsStartpoint;
    [SerializeField] GameObject elevatorButtonPrefab;
    float verticalButtonSpacing = 0.185f;
    float horizontalButtonSpacing = 0.24f;


    void Awake()
    {
        generator = LevelGenerator.Instance;
        SpawnButtons();
    }

    public void SpawnButtons()
    {
        int totalFloors = generator.totalFloors;

        for (int floor = 1; floor <= totalFloors; floor++)
        {
            int column = (floor - 1) / 10;
            int row = (floor - 1) % 10;
            Vector3 buttonPosition = new Vector3(column * horizontalButtonSpacing, row * verticalButtonSpacing, 0f);

            GameObject newButton = Instantiate(elevatorButtonPrefab, buttonsStartpoint);
            newButton.transform.localPosition = buttonPosition;

            ElevatorButton buttonComponent = newButton.GetComponent<ElevatorButton>();
            //Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.elevator = this;
            buttonComponent.floorNumber = floor;
            buttonComponent.buttonText.text = floor.ToString();
        }
    }

    public void MoveTo(int floorNumber)
    {
        Floor floor = LevelGenerator.Instance.building[floorNumber - 1];
        Transform target = floor.elevatorStartpoint;

        StartCoroutine(AnimateElevator(target));
    }



    public float acceleration = 4f;

    public IEnumerator AnimateElevator(Transform target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, target.position.y, startPos.z);
        float distance = Mathf.Abs(targetPos.y - startPos.y);

        if (distance < 0.001f)
            yield break;

        float duration = 2f * Mathf.Sqrt(distance / acceleration);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Ќормированное врем€ t измен€етс€ от 0 до 1
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = (1f - Mathf.Cos(t * Mathf.PI)) / 2f;

            transform.position = Vector3.Lerp(startPos, targetPos, easedT);
            yield return null;
        }

        // ¬ конце гарантируем, что позици€ точно равна целевой.
        transform.position = targetPos;
    }
}

