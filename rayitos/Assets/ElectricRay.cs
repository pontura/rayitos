using UnityEngine;

public class ElectricRay : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public int segments = 10; // Número de puntos en la línea
    public float noiseIntensity = 0.2f;

    private LineRenderer lineRenderer;
    bool isOn;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments;
    }
    public void SetOff()
    {
        gameObject.SetActive(false);
        isOn = false;
    }
    public void Init(Vector3 startPoint, Vector3 endPoint)
    {
        isOn = true;
        Draw(0);
        this.startPoint = startPoint;
        this.endPoint = endPoint; 
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isOn) return;
        if(startPoint == null || endPoint == null) return;

        for (int i = 0; i < segments; i++)
        {
            Draw(i);
        }
    }
    private void Draw(int i)
    {
        float t = (float)i / (segments - 1);
        Vector3 pos = Vector3.Lerp(startPoint, endPoint, t);

        // Agregar ruido aleatorio para un efecto eléctrico
        pos.x += Random.Range(-noiseIntensity, noiseIntensity);
        pos.y += Random.Range(-noiseIntensity, noiseIntensity);

        lineRenderer.SetPosition(i, pos);
    }
}
