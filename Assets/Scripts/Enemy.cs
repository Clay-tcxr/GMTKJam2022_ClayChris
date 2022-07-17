using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int hp = 5;
    public static  Enums.Sides[] currentOrientation = new Enums.Sides[6] {Enums.Sides.Up, Enums.Sides.Front,Enums.Sides.Down, Enums.Sides.Back, Enums.Sides.Left,Enums.Sides.Right};
    [SerializeField]
    public Enums.Sides side;
    [SerializeField]
    private float speed = 1f;
    private Quaternion nextRotation;
    public Transform TargetPosition;
    private bool _isRotating = false;
    public AudioSource thud;
    public 


    void SetSide(Enums.Sides CurrentSide)
    {
        switch (CurrentSide)
        {
            case Enums.Sides.Up:
                gameObject.transform.rotation = Quaternion.identity;
                break;
            case Enums.Sides.Down:
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(180, 0, 0);
                break;
            case Enums.Sides.Left:
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
                break;
            case Enums.Sides.Right:
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
                break;
            case Enums.Sides.Front:
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(90, 0, 0);
                break;
            case Enums.Sides.Back:
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(-90, 0, 0);
                break;
        }
        side = CurrentSide;
    }

    void RotateBack()
    {
        if (!CheckDir(Vector3.back))
        {
            return;
        }
        MakeRotate(Vector3.back);
        Enums.Sides tempSide;
        
        tempSide = currentOrientation[0];
        currentOrientation[0] = currentOrientation[3];
        currentOrientation[3] = currentOrientation[2];
        currentOrientation[2] = currentOrientation[1];
        currentOrientation[1] = tempSide;
        side = currentOrientation[0];

    }
    void RotateForward()
    {
        if (!CheckDir(Vector3.forward))
        {
            return;
        }
        MakeRotate(Vector3.forward);
        Enums.Sides tempSide;

        tempSide = currentOrientation[0];
        currentOrientation[0] = currentOrientation[1];
        currentOrientation[1] = currentOrientation[2];
        currentOrientation[2] = currentOrientation[3];
        currentOrientation[3] = tempSide;
        side = currentOrientation[0];

    }
    void RotateLeft()
    {
        if (!CheckDir(Vector3.left))
        {
            return;
        }
        MakeRotate(Vector3.left);
        Enums.Sides tempSide;

        tempSide = currentOrientation[0];
        currentOrientation[0] = currentOrientation[5];
        currentOrientation[5] = currentOrientation[2];
        currentOrientation[2] = currentOrientation[4];
        currentOrientation[4] = tempSide;
        side = currentOrientation[0];

    }
    void RotateRight()
    {
        if (!CheckDir(Vector3.right))
        {
            return;
        }
        MakeRotate(Vector3.right);
        Enums.Sides tempSide;

        tempSide = currentOrientation[0];
        currentOrientation[0] = currentOrientation[4];
        currentOrientation[4] = currentOrientation[2];
        currentOrientation[2] = currentOrientation[5];
        currentOrientation[5] = tempSide;
        side = currentOrientation[0];

    }
    void Start()
    {

    }

    
    // Update is called once per frame
    void Update()
    {
        if (!_isRotating)
        {
            var direction = TargetPosition.position - transform.position;
            if ( Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if(direction.x > 0)
                    RotateRight();
                else
                    RotateLeft();

            }
            if ( Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            {
                if(direction.z > 0)
                    RotateForward();
                else
                    RotateBack();
            }
        }
    }
    
    bool CheckDir(Vector3 dir)
    {
        return !Physics.Raycast(transform.position, dir, 0.5F);
    }
    void MakeRotate(Vector3 dir)
    {

        var anchor = gameObject.transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);
        if (CheckDir(dir))
        { 
         StartCoroutine(Roll(anchor,axis,CheckSide()));
        }
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis, bool pause = false)
    {
        _isRotating = true;
        if (pause)
        {
            yield return new WaitForSeconds(1f);
        }
           for(int i=0; i < (90 / speed); i++)
        {
            gameObject.transform.RotateAround(anchor, axis, speed);
            yield return new WaitForSeconds(0.01f);
        }
        thud.Play();
        _isRotating = false;
    }

    bool CheckSide()
    {
        if(side == Enums.Sides.Right)
        { 
            return true;
        }
        return false;
    }

    void DoDamage()
    {
        
    }
}
