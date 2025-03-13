using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }   
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }

    protected static PlayerController _PLAYER { get { return PlayerController.instance; } }

    public GameState _CurrentGameState => GameManager.instance.GameState;


    /// <summary>
    /// Gets the closet object
    /// </summary>
    /// <param name="_origin">the object</param>
    /// <param name="_objects"></param>
    /// <returns></returns>
    public Transform GetClosestObject(Transform _origin, List<GameObject> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = go.transform;
            }
        }
        return closest;
    }
    public Transform GetClosestObject(Transform _origin, List<Enemy> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (Enemy go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = go.transform;
            }
        }
        return closest;
    }
}
