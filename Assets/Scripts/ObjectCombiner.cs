using UnityEngine;
using System.Collections.Generic;


public class ObjectCombiner : MonoBehaviour
{

    // ������� ����������� ���� MeshFilter'��, ��������� � �������� �������� parent.
    // ���� destroyOriginal == true, �� ������������ ������� ����� ���������.
    public static void CombineMeshes(GameObject parent, bool destroyOriginal = true)
    {
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();

        // ����������� ������� �� ���������. ���� � ��������, �������� � ������ CombineInstance.
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

            // ������� CombineInstance ��� ������� ����.
            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            // ������� �������������� ��������� ���������� �� ��������� ��������� ������� � ������� ��������� parent.
            ci.transform = parent.transform.worldToLocalMatrix * mf.transform.localToWorldMatrix;
            combineByMaterial[mat].Add(ci);
        }

        // ��� ������� ��������� �������� ����� ������ � ������������ �����.
        foreach (KeyValuePair<Material, List<CombineInstance>> pair in combineByMaterial)
        {
            Material mat = pair.Key;
            List<CombineInstance> combineInstances = pair.Value;

            Mesh combinedMesh = new Mesh();
            // �������� mergeSubMeshes = true � ���������� � ���� ������, ��� ��� ��� ���������� ���������� ���� � ��� �� ��������.
            combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

            // ������� ����� �������� ������ ��� ������������� ����.
            GameObject combinedObject = new GameObject("CombinedMesh_" + mat.name);
            combinedObject.transform.SetParent(parent.transform);
            combinedObject.transform.localPosition = Vector3.zero;
            combinedObject.transform.localRotation = Quaternion.identity;
            combinedObject.transform.localScale = Vector3.one;

            // ��������� ��������� MeshFilter � ��� ���.
            MeshFilter mfCombined = combinedObject.AddComponent<MeshFilter>();
            mfCombined.mesh = combinedMesh;
            // ������� MeshRenderer � ��������� ��������.
            MeshRenderer mrCombined = combinedObject.AddComponent<MeshRenderer>();
            mrCombined.material = mat;
        }

        // ��������� (���, �� �������, ����������) ������������ �������, ����� �������� ������� ���������.
        if (destroyOriginal)
        {
            foreach (MeshFilter mf in meshFilters)
            {
                // ����� �������� �� ��������� parent, ���������, ��� ������ �� == �������������.
                if (mf.gameObject != parent)
                {
                    mf.gameObject.SetActive(false);
                }
            }
        }
    }


}
