using UnityEngine;

public class RaysManager : MonoBehaviour
{

    [SerializeField] ElectricRay[] electricRay;

    public void Init(Vector3 pos, int id = 0)
    {
        electricRay[id].Init(GetMouseWorldPosition(), pos);
    }
    public void SetOff(int id = 0)
    {
        electricRay[id].SetOff();
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Distancia del rayo desde la cámara
        return Camera.main.ScreenToWorldPoint(mousePos);
    }


}
