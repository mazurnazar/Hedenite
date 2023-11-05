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
        Debug.Log(talk.Length);
    }
    public void PlayAnimation(string parameter)
    {
       // Debug.Log("play anim " + parameter);
        if(parameter=="Speak")
        {
            int r = Random.Range(0, talk.Length);
            Debug.Log(gameObject.name);
            Debug.Log(r);
            Debug.Log(talk.Length);
            Debug.Log(talk[r]);
            animator.Play(talk[r]);
           // animator.SetBool(talk[r],true);
            currentParameter = talk[r];

           // StartCoroutine(StopAnim(talk[r]));
        }
        else
        {
            Debug.Log(parameter);
            animator.Play(parameter);
           // animator.SetBool(parameter, true);
            currentParameter = parameter;
           // StartCoroutine(StopAnim(parameter));
            //animator.SetBool(parameter, false);
        }

    }
    public IEnumerator StopAnim(string parameter)
    {
        yield return new WaitForSeconds(.5f);
        animator.Play("idle");
       // animator.SetBool(parameter, false);
    }
    public void StopAnimation(string parameter)
    {
        animator.Play("idle");
        //animator.SetBool(currentParameter, false);
    }
    private void OnDisable()
    {
        Say.AnimationPlay -= PlayAnimation;
        Say.AnimationStop -= StopAnimation;
    }
    private void OnDestroy()
    {
        Say.AnimationPlay -= PlayAnimation;
        Say.AnimationStop -= StopAnimation;
    }
}
