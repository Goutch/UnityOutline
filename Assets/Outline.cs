using UnityEngine;

public class Outline : MonoBehaviour
{
    private OutlineRenderer renderer;
    private MeshFilter mesh;
    [SerializeField] private bool active = false;
    public bool Activated { get; set; } = true;
    public MeshFilter Mesh => mesh;

    void Start()
    {
        Activated = active;
        mesh = GetComponent<MeshFilter>();
        renderer = Camera.main.GetComponent<OutlineRenderer>();
    }

    private void Update()
    {
        if (Activated)
        {
            renderer.DrawOutline(this);
        }
    }
}