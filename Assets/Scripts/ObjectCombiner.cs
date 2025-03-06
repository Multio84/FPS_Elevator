using UnityEngine;
using System.Collections.Generic;


public class ObjectCombiner : MonoBehaviour
{

    // Функция объединения всех MeshFilter'ов, найденных в дочерних объектах parent.
    // Если destroyOriginal == true, то оригинальные объекты будут отключены.
    public static void CombineMeshes(GameObject parent, bool destroyOriginal = true)
    {
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();

        // Сгруппируем объекты по материалу. Ключ – материал, значение – список CombineInstance.
        Dictionary<Material, List<CombineInstance>> combineByMaterial = new Dictionary<Material, List<CombineInstance>>();

        foreach (MeshFilter mf in meshFilters)
        {
            // skip parent's MeshFilter
            if (mf.transform == parent.transform)
                continue;

            MeshRenderer mr = mf.GetComponent<MeshRenderer>();
            if (mr == null)
                continue;

            Material mat = mr.sharedMaterial;
            if (!combineByMaterial.ContainsKey(mat))
            {
                combineByMaterial[mat] = new List<CombineInstance>();
            }

            // Создаем CombineInstance для данного меша.
            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            // Матрица преобразования переводит координаты из локальных координат объекта в систему координат parent.
            ci.transform = parent.transform.worldToLocalMatrix * mf.transform.localToWorldMatrix;
            combineByMaterial[mat].Add(ci);
        }

        // Для каждого материала создадим новый объект с объединенным мешем.
        foreach (KeyValuePair<Material, List<CombineInstance>> pair in combineByMaterial)
        {
            Material mat = pair.Key;
            List<CombineInstance> combineInstances = pair.Value;

            Mesh combinedMesh = new Mesh();
            // Параметр mergeSubMeshes = true – объединяем в один сабмеш, так как все экземпляры используют один и тот же материал.
            combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

            // Создаем новый дочерний объект для объединенного меша.
            GameObject combinedObject = new GameObject("CombinedMesh_" + mat.name);
            combinedObject.transform.SetParent(parent.transform);
            combinedObject.transform.localPosition = Vector3.zero;
            combinedObject.transform.localRotation = Quaternion.identity;
            combinedObject.transform.localScale = Vector3.one;

            // Назначаем компонент MeshFilter и его меш.
            MeshFilter mfCombined = combinedObject.AddComponent<MeshFilter>();
            mfCombined.mesh = combinedMesh;
            // Создаем MeshRenderer и назначаем материал.
            MeshRenderer mrCombined = combinedObject.AddComponent<MeshRenderer>();
            mrCombined.material = mat;
        }

        // Отключаем (или, по желанию, уничтожаем) оригинальные объекты, чтобы избежать двойной отрисовки.
        if (destroyOriginal)
        {
            foreach (MeshFilter mf in meshFilters)
            {
                // Чтобы случайно не отключить parent, проверяем, что объект не == родительскому.
                if (mf.gameObject != parent)
                {
                    mf.gameObject.SetActive(false);
                }
            }
        }
    }


}
