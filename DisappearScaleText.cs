using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisappearScaleText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textMesh;
    [SerializeField]
    private float durationPerText;
    [SerializeField]
    private Ease easeType;
    [SerializeField]
    private float intervalPerText=0.2f;
    private bool isComplete;
    private float scale;
    private Mesh textMeshInit;
    private Vector3[] cacheVert;
    private Vector3[] passVert;
    private readonly int sides = 4;
    private bool useRef;
    void Start()
    {
        if (GetComponent<ScaleAppearingText>() != null)
        {
            
            useRef = true;
        }
        else
        {
            textMesh.ForceMeshUpdate();
        }
        textMeshInit = textMesh.mesh;
        cacheVert = textMeshInit.vertices;
        passVert = textMeshInit.vertices;
       
    }
    public void playAnimate()
    {
        StartCoroutine(textAnimate());
    }
    IEnumerator textAnimate()
    {
        if (useRef)
        {
            yield return new WaitUntil(() => GetComponent<ScaleAppearingText>().updateComplete);
            textMesh = GetComponent<ScaleAppearingText>().getText();
            textMeshInit = textMesh.mesh;
            cacheVert = textMeshInit.vertices;
            passVert = textMeshInit.vertices;
        }
        foreach (var characterInfo in textMesh.textInfo.characterInfo)
        {
            scale = 1;
            if (characterInfo.isVisible)
            {
                DOTween.To(() => scale, target =>
                {

                    for (int i = 0; i < sides; i++)
                    {
                        passVert[characterInfo.vertexIndex + i] = Matrix4x4.Scale(Vector3.one * target).MultiplyPoint3x4(cacheVert[characterInfo.vertexIndex + i]);

                    }
                    textMeshInit.vertices = passVert;
                    textMesh.UpdateGeometry(textMeshInit, 0);
                    // textMesh.ForceMeshUpdate() ;
                }, 0, durationPerText).SetEase(easeType).onComplete += () => isComplete = true;
                //yield return new WaitUntil(() => isComplete);
                yield return new WaitForSeconds(intervalPerText);
                isComplete = false;
            }
        }
        yield return null;
    }
}
