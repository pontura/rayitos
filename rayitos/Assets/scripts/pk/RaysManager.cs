using UnityEngine;

public class RaysManager : MonoBehaviour
{

    [SerializeField] ElectricRay[] electricRay;

    public void Init(Vector3 pos, int id = 0)
    {
        electricRay[id].Init(InitPos(), pos);
    }
    public void SetOff(int id = 0)
    {
        electricRay[id].SetOff();
    }
    Vector3 InitPos()
    {
        return new Vector3(0, 0, -6);
    }


}