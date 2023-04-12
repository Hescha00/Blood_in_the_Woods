using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float spaceBetweenTiles = 0;

    [Header("Tile Settings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public Material material;
    public bool FlatTopped;


    private void OnEnable()
    {
        LayoutGrid();
    }

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            LayoutGrid();
        }
    }

    private void LayoutGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                tile.transform.position = GetPositionfromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = FlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.height = height;
                hexRenderer.material = material;
                hexRenderer.DrawMesh();

                tile.transform.SetParent(transform, true);

            } 

        }
    }

    public Vector3 GetPositionfromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldoffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if(!FlatTopped)
        {
            shouldoffset = (row % 2) == 0;
            width = 1.732f * size;   // Wurzel 3
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldoffset) ? width / 2 : 0;
            xPosition = (column * (horizontalDistance + spaceBetweenTiles)) + offset;
            yPosition = row * (verticalDistance+ spaceBetweenTiles);
        }
        else
        {
            shouldoffset = (column % 2) == 0;
            width = 2f * size;   
            height = 1.732f * size;// Wurzel 3

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height; 

            offset = (shouldoffset) ? height / 2 : 0;
            xPosition = column * (horizontalDistance + spaceBetweenTiles);
            yPosition = (row * (verticalDistance + spaceBetweenTiles)) - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}
