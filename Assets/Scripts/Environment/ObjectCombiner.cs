using UnityEngine;
using System.Collections.Generic;


public class ObjectCombiner : MonoBehaviour
{
    public static string tagToCombine = "Combine";


    /// <summary>
    /// Combines the tagged as 'tagToCombine' child objects of the sourceRoot into one, 
    /// if they have MeshFilter, MeshRenderer and the same material.
    /// Also copies their BoxColliders to new empty childs, if original object had non-zero rotation.
    /// All tagged objects are deleted after combination.
    /// </summary>
    /// <param name="sourceRoot">Parent object, whose childs will be combined.</param>
    /// <param name="prefix">Name prefix of the new combined object</param>
    /// <param name="destroyOriginals">Destroys the original objects, if true, else - disables them.</param>
    public static void CombineObjectsByTag(GameObject sourceRoot, string prefix = "", bool destroyOriginals = true)
    {
        List<GameObject> objectsToDestroy = new List<GameObject>();
        var combineInstancesByMat = new Dictionary<Material, List<CombineInstance>>(); // objects to combine
        Transform[] children = sourceRoot.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (!child.CompareTag(tagToCombine) || child == sourceRoot.transform)
                continue;
            objectsToDestroy.Add(child.gameObject); // add all objects that have the correct tag

            MeshFilter mf = child.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null)
                continue;
            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr == null)
                continue;
            Material mat = mr.sharedMaterial;
            if (mat == null)
                continue;

            // convert mesh matrices from original coordinate system into sourceRoot's:
            CombineInstance ci = new CombineInstance
            {
                mesh = mf.sharedMesh,
                transform = sourceRoot.transform.worldToLocalMatrix * child.localToWorldMatrix
            };

            // create a list for each material:
            if (!combineInstancesByMat.TryGetValue(mat, out List<CombineInstance> list))
            {
                list = new List<CombineInstance>();
                combineInstancesByMat.Add(mat, list);
            }
            list.Add(ci);
        }

        // create new mesh, combine and copy settings to it:
        Dictionary<Material, GameObject> combinedMeshObjects = new Dictionary<Material, GameObject>();
        foreach (KeyValuePair<Material, List<CombineInstance>> pair in combineInstancesByMat)
        {
            Material mat = pair.Key;
            List<CombineInstance> instances = pair.Value;

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(instances.ToArray(), true, true);

            // new object to contain combined mesh and all it's settings
            GameObject combinedChild = new GameObject(prefix + mat.name + "s");
            combinedChild.transform.SetParent(sourceRoot.transform, false);

            combinedChild.isStatic = true;
            combinedChild.tag = tagToCombine;

            // add components for combined mesh
            MeshFilter mfCombined = combinedChild.AddComponent<MeshFilter>();
            mfCombined.mesh = combinedMesh;
            MeshRenderer mrCombined = combinedChild.AddComponent<MeshRenderer>();
            mrCombined.material = mat;

            combinedMeshObjects[mat] = combinedChild;
        }

        // copy colliders and create new empty objects to contain colliders, if the have non-zero rotation (to preserve it):
        BoxCollider[] boxColliders = sourceRoot.GetComponentsInChildren<BoxCollider>(true);
        foreach (BoxCollider originalBox in boxColliders)
        {
            // find object in collection by it's sharedMaterial:
            MeshRenderer originalMR = originalBox.GetComponent<MeshRenderer>();
            if (originalMR == null)
                continue;
            Material targetMat = originalMR.sharedMaterial;
            if (targetMat == null || !combinedMeshObjects.ContainsKey(targetMat))
                continue;
            GameObject targetParent = combinedMeshObjects[targetMat];

            Vector3 colliderWorldCenter = originalBox.transform.TransformPoint(originalBox.center);
            Vector3 colliderWorldSize = Vector3.Scale(originalBox.size, originalBox.transform.lossyScale);

            if (originalBox.transform.rotation == Quaternion.identity)
            {
                Vector3 localCenter = targetParent.transform.InverseTransformPoint(colliderWorldCenter);
                BoxCollider newBox = targetParent.AddComponent<BoxCollider>();
                newBox.center = localCenter;

                // get local size, taking into account it's scale
                Vector3 parentScale = targetParent.transform.lossyScale;
                Vector3 localSize = new Vector3(
                    colliderWorldSize.x / parentScale.x,
                    colliderWorldSize.y / parentScale.y,
                    colliderWorldSize.z / parentScale.z);
                newBox.size = localSize;

                newBox.isTrigger = originalBox.isTrigger;
                newBox.material = originalBox.material;
            }
            else
            {
                // create a new object for rotated objects, to preserve their collider's rotation
                GameObject colliderCopy = new GameObject(originalBox.gameObject.name + "Collider");
                colliderCopy.transform.SetParent(targetParent.transform, true);

                colliderCopy.transform.position = colliderWorldCenter;
                colliderCopy.transform.rotation = originalBox.transform.rotation;
                colliderCopy.transform.localScale = Vector3.one;

                // set collider's world size and copy parameters:
                BoxCollider newBox = colliderCopy.AddComponent<BoxCollider>();
                newBox.center = Vector3.zero;
                newBox.size = colliderWorldSize;
                newBox.isTrigger = originalBox.isTrigger;
                newBox.material = originalBox.material;
            }
        }

        foreach (var go in objectsToDestroy)
        {
            if (destroyOriginals)
                DestroyImmediate(go);
            else
                go.SetActive(false);
        }
    }
}

