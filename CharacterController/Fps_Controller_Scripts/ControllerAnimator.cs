using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAnimator : MonoBehaviour
{
    Animator m_Animator;
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    
    public void Activar(bool corriendo) {
        if (corriendo != true) {
            m_Animator.SetBool("Corriendo", false);
            return;
        }else {
            m_Animator.SetBool("Corriendo", true);
            return;
        }
    }

    public void Apuntar(bool apuntar)
    {
        if (apuntar != true) {
            m_Animator.SetBool("Apuntar", false);
            return;
        }else {
            m_Animator.SetBool("Apuntar", true);
            return;
        }
    }
    
    public void Disparar()
    {
        m_Animator.SetTrigger("Disparar");
    }
    
    
    
}
