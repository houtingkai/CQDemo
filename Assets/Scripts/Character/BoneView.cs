using UnityEngine;
using UnityEditor;
using System.Collections;

public class BoneView : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            Transform t = sr.transform.parent;
            if(t != null && t.GetComponent<SpriteRenderer>() != null)
            {
                Gizmos.DrawLine(sr.transform.position, t.position);
            }
            Vector3 pZero = sr.transform.position;
            Gizmos.DrawSphere(pZero, 0.03f);
        }
    }
}


//string path = AssetDatabase.GetAssetPath(sr.sprite.texture);
//TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
//Vector3 pZero = transform.position;
//Vector2 spriteSize = sr.sprite.bounds.size;
////pZero = new Vector3(pZero.x - spriteSize.x / 2, pZero.y - spriteSize.y / 2, pZero.z);
////
//Debug.Log(path + ": " + sr.transform.position);