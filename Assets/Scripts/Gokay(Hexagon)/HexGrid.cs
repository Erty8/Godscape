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

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public TMP_Text cellLabelPrefab;

	HexCell[] cells;
	HexMesh hexMesh;

	Canvas gridCanvas;


	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
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
	}

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
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = touchedColor;
		hexMesh.Triangulate(cells);
	}


}