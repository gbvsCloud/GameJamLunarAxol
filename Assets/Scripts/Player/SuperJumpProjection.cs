using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class SuperJumpProjection : MonoBehaviour
{

    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    [SerializeField] 
    private Transform grid;

    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    public int maxPhysicsIterations;

    private void Start()
    {
        grid = FindObjectOfType<Grid>().transform;
        lineRenderer = GetComponent<LineRenderer>();
        CreatePhysicsScene();     
    }

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));

        physicsScene = simulationScene.GetPhysicsScene2D();

        var ghostObj = Instantiate(grid.gameObject, grid.transform.position, grid.transform.rotation);
        SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
    }

    public void SimulateTrajectory(SuperJumpEcho echo, Vector2 pos, Vector2 velocity)
    {
        var ghostObj = Instantiate(echo, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);

        ghostObj.Init(velocity);

        lineRenderer.positionCount = maxPhysicsIterations;

        for(int i = 0; i < maxPhysicsIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }

}
