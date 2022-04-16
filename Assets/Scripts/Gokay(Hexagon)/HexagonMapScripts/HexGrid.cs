using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexGrid : MonoBehaviour
{

	public Color defaultColor = Color.white;
	public Color touchedColor = Color.magenta;

	void Start()
	{
		hexMesh.Triangulate(cells);
	}

	public const int chunkSizeX = 5, chunkSizeZ = 5;

	int cellCountX, cellCountZ;
	public int chunkCountX = 4, chunkCountZ = 3;

	public HexCell cellPrefab;
	public TMP_Text cellLabelPrefab;
	public HexGridChunk chunkPrefab;

	HexGridChunk[] chunks;
	HexCell[] cells;
	HexMesh hexMesh;

	Canvas gridCanvas;

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cellCountX = chunkCountX * Hex.chunkSizeX;
		cellCountZ = chunkCountZ * Hex.chunkSizeZ;

		CreateChunks();
		CreateCells();
	}

	void CreateChunks()
	{
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++)
		{
			for (int x = 0; x < chunkCountX; x++)
			{
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells()
	{
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++)
		{
			for (int x = 0; x < cellCountX; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (Hex.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (Hex.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		TMP_Text label = Instantiate<TMP_Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();

		//AddCellToChunk(x, z, cell);
	}
	
	
	
	
	/* The new updates for larger maps, about assiging celss to chunks.
	void AddCellToChunk(int x, int z, HexCell cell)
	{
		int chunkX = x / Hex.chunkSizeX;
		int chunkZ = z / Hex.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * Hex.chunkSizeX;
		int localZ = z - chunkZ * Hex.chunkSizeZ;
		chunk.AddCell(localX + localZ * Hex.chunkSizeX, cell);
	}
	*/
	
	
	
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			TouchCell(hit.point);
		}
	}

	public void TouchCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * cellCountZ + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = touchedColor;
		hexMesh.Triangulate(cells);
	}


}