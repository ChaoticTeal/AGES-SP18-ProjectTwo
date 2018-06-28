using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PressurePlate : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Is the pressure plate temporary?")]
    [SerializeField]
    bool isTempPlate;
    [Tooltip("Target index.")]
    [SerializeField]
    int plateIndex_UseProperty;
    [Tooltip("Target type.\n0 for single target/door opens.\n1 for double target/door opens.")]
    [SerializeField]
    int plateType_UseProperty;
    [Tooltip("Tile used when active.")]
    [SerializeField]
    Tile activeTile;
    [Tooltip("Position of plate in tilemap.")]
    [SerializeField]
    Vector3Int tilePosition;

    // Properties
    bool Activated
    {
        get
        {
            return activated_UseProperty;
        }
        set
        {
            activated_UseProperty = value;
            if (activated_UseProperty)
            {
                tilemap.SetTile(tilePosition, activeTile);
                if (OnActivated != null)
                    // Notify that the target was hit, with its type and index
                    OnActivated.Invoke(TriggerType, TriggerIndex);
            }
            else
            {
                tilemap.SetTile(tilePosition, startingTile);
                if (OnDeactivated != null)
                    OnDeactivated.Invoke(TriggerType, TriggerIndex);
            }

        }
    }
    public int TriggerType
    {
        get
        {
            return plateType_UseProperty;
        }
    }
    public int TriggerIndex
    {
        get
        {
            return plateIndex_UseProperty;
        }
    }

    // Private fields
    /// <summary>
    /// Has the target been hit?
    /// </summary>
    bool activated_UseProperty;
    /// <summary>
    /// Starting tile
    /// </summary>
    Tile startingTile;
    /// <summary>
    /// Tilemap
    /// </summary>
    Tilemap tilemap;

    // Public fields
    /// <summary>
    /// Notify on hit
    /// </summary>
    public static event System.Action<int, int> OnActivated;
    /// <summary>
    /// Notify deactivation
    /// </summary>
    public static event System.Action<int, int> OnDeactivated;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        startingTile = (Tile)tilemap.GetTile(tilePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Activated = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isTempPlate)
            Activated = false;
    }
}
