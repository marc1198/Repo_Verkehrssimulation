using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]           // Generate Rigidbody to objects without Rigidbody
public class CarController : MonoBehaviour
{
    Rigidbody Body;

    public PlacementManager PlacementManagerTest;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        drivethisway.Add(0);
        drivethisway.Add(0);
        drivethisway.Add(0);
    }

    float InterpolationStraight, InterpolationCircular; bool StraightPossible, CurvePossible, driven = false;
    Vector3 Startposition, FromVector, ToVector = Vector3.forward;

    Vector3Int Infront, TwoInfront, Left;

    List<int> drivethisway = new List<int>();

    

    private void DriveLeftOrRightOrStraight(int Direction)// Direction: streight 0 left -1 right 1 
    {
        if (InterpolationStraight < 0.01f & InterpolationCircular < 0.01f)//If currently not driving at all
        {
            Startposition = transform.position;

            if (transform.rotation.eulerAngles.y > 315.0f | transform.rotation.eulerAngles.y < 45.0f) { FromVector = Vector3.left * Direction; ToVector = Vector3.forward; }
            if (transform.rotation.eulerAngles.y > 45.0f & transform.rotation.eulerAngles.y < 135.0f) { FromVector = Vector3.forward * Direction; ToVector = Vector3.right; }
            if (transform.rotation.eulerAngles.y > 135.0f & transform.rotation.eulerAngles.y < 225.0f) { FromVector = Vector3.right * Direction; ToVector = Vector3.back; }
            if (transform.rotation.eulerAngles.y > 225.0f & transform.rotation.eulerAngles.y < 315.0f) { FromVector = Vector3.back * Direction; ToVector = Vector3.left; }

            Infront = Vector3Int.FloorToInt(transform.position + new Vector3(0.5f, 0.0f, 0.5f) + (-0.5f * (ToVector.x + ToVector.z) + 0.5f) * ToVector);
            TwoInfront = Infront + Vector3Int.FloorToInt(ToVector); Left = Infront - Vector3Int.FloorToInt(FromVector);
            StraightPossible = PlacementManagerTest.CheckIfPositionIsOfType(Infront, CellType.Road) & PlacementManagerTest.CheckIfPositionIsOfType(TwoInfront, CellType.Road);
            CurvePossible = PlacementManagerTest.CheckIfPositionIsOfType(Infront, CellType.Road) & PlacementManagerTest.CheckIfPositionIsOfType(Left, CellType.Road);

        }


        if (Direction < 0 | Direction > 0)//If not driving straight and should drive circle
        {

            if ((InterpolationStraight < 0.01f) & CurvePossible)
            {
                Body.MoveRotation(Quaternion.Slerp(Quaternion.LookRotation(ToVector), Quaternion.LookRotation(-1 * FromVector), InterpolationCircular));
                Body.MovePosition(Startposition - (0.5f - Direction * 0.1f) * FromVector + Vector3.Slerp((0.5f - Direction * 0.1f) * FromVector, (0.5f - Direction * 0.1f) * ToVector, InterpolationCircular));
                if (InterpolationCircular < 1.012732f) { InterpolationCircular = InterpolationCircular + 0.01f * 1.2732f; }
                if (InterpolationCircular > 1.012732f) { InterpolationCircular = 0.0f; }
                driven = true;
            }
        }


        else if (Direction < 1 & Direction > -1) //Straight code
        {
            if ((InterpolationCircular < 0.01f) & StraightPossible)//If not driving a circle and should drive straight
            {

                Body.MovePosition(Vector3.Lerp(Startposition, Startposition + ToVector, InterpolationStraight));
                if (InterpolationStraight < 1.01f) { InterpolationStraight = InterpolationStraight + 0.01f; }
                if (InterpolationStraight > 1.01f) { InterpolationStraight = 0.0f; }
                driven = true;
            }

        }
    }

    int RandomDirection;
    private void FixedUpdate()
    {
        /* Ursprüngliche Programmierung mit Fahrt in zufällige Richtung
        if (InterpolationStraight < 0.01f & InterpolationCircular < 0.01f) { RandomDirection = UnityEngine.Random.Range(-1, 2); }
        DriveLeftOrRightOrStraight(RandomDirection);//UnityEngine.Random.Range(-1, 2)
        */

        
        DriveLeftOrRightOrStraight(0);
        if (driven == true)
        {

            driven = false;
        }
            

        //List<int> richtungsabfolge = new List<int>();
        /*if (InterpolationStraight < 0.01f & InterpolationCircular < 0.01f) 
        {
            
            
            richtungsabfolge.Add(0);
            richtungsabfolge.Add(0);
            richtungsabfolge.Add(0);
            richtungsabfolge.Add(0);
            richtungsabfolge.Add(0);
            richtungsabfolge.Add(0);
            
        }
        
        foreach (int wert in richtungsabfolge)
        {
            DriveLeftOrRightOrStraight(wert);//UnityEngine.Random.Range(-1, 2)
        }
            */
    }
}
