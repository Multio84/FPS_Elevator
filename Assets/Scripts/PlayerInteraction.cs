using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Интерфейс для интерактивных объектов
public interface IInteractable
{
    void Interact();
}


public class PlayerInteraction : MonoBehaviour
{

    // Расстояние, на котором можно взаимодействовать с объектами
    public float interactDistance = 3f;


    void Update()
    {
        // Если нажата клавиша E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Получаем луч из центра экрана
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            // Если луч что-то попадает в пределах interactDistance
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Пытаемся получить компонент IInteractable на объекте
                //IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    // Вызов метода взаимодействия
                    interactable.Interact();
                }
            }
        }
    }
}
