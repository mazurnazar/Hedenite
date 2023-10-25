using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] string[] talk;
    Animator animator;
    [SerializeField] string currentParameter;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Say.AnimationPlay += PlayAnimation;
        Say.AnimationStop += StopAnimation;
    }
    public void PlayAnimation(string parameter)
    {
        Debug.Log("play anim " + parameter);
        if(parameter=="Speak")
        {
            int r = Random.Range(0, talk.Length);
            animator.SetBool(talk[r],true);
            currentParameter = talk[r];
        }
        else
        {
            animator.SetBool(parameter, true);
            currentParameter = parameter;
            StartCoroutine(StopAnim(parameter));
            //animator.SetBool(parameter, false);
        }

    }
    public IEnumerator StopAnim(string parameter)
    {
        yield return new WaitForSeconds(.1f);
        animator.SetBool(parameter, false);
    }
    public void StopAnimation(string parameter)
    {
        animator.SetBool(currentParameter, false);
    }
    private void OnDestroy()
    {
        Say.AnimationPlay -= PlayAnimation;
        Say.AnimationStop -= StopAnimation;
    }
}
