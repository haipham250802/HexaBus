using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AutoAligh : MonoBehaviour
{
    public float spacing = 1.0f; 
    [Button]
    public void AlignChildren()
    {
        int childCount = transform.childCount;

        if (childCount == 0)
            return;
        float totalWidth = (childCount - 1) * spacing;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float xPos = -totalWidth / 2 + i * spacing;
            child.localPosition = new Vector3(xPos, child.localPosition.y, child.localPosition.z);
        }
    }
}
