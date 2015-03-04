using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortingLayer : MonoBehaviour 
{
    public int sortingLayerID;

	void Start () 
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingLayerID = sortingLayerID;
        }
	}
	
}
