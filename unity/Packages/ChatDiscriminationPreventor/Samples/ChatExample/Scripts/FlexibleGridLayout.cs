using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
        FixedBoth,
    }

    public FitType fitType;

    [HideInInspector]
    public bool enableRowsEditing;
    [DrawIf("enableRowsEditing", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public int rows;

    [HideInInspector]
    public bool enableColumnsEditing = false;
    [DrawIf("enableColumnsEditing", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public int columns;

    [HideInInspector]
    public bool disableSizeEditing = false;
    [DrawIf("disableSizeEditing", false, ComparisonType.Equals, DisablingType.ReadOnly)]
    public Vector2 cellSize;

    [DrawIf("autoSpacing", false, ComparisonType.Equals, DisablingType.ReadOnly)]
    public Vector2 spacing;

    [DrawIf("autoSpacing", true, ComparisonType.Equals)]
    public Vector2 minSpacing;


    [DrawIf("disableSizeEditing", false, ComparisonType.Equals, DisablingType.ReadOnly)]
    public bool fitX;
    [DrawIf("disableSizeEditing", false, ComparisonType.Equals, DisablingType.ReadOnly)]
    public bool fitY;

    [HideInInspector]
    public bool enableSquareCellSizeOption;
    [DrawIf("enableSquareCellSizeOption", true, ComparisonType.Equals)]
    public bool squareCellSize;

    [HideInInspector]
    public bool autoSpacing;

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();
        enableRowsEditing = fitType == FitType.FixedBoth || fitType == FitType.FixedRows;
        enableColumnsEditing = fitType == FitType.FixedBoth || fitType == FitType.FixedColumns;


        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
            disableSizeEditing = true;
        }
        else
        {
            disableSizeEditing = fitType == FitType.FixedBoth;
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        if (fitType != FitType.FixedBoth && fitType != FitType.FixedColumns)
        {
            squareCellSize = false;
            enableSquareCellSizeOption = false;
        }
        else
        {
            enableSquareCellSizeOption = true;
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;


        if (fitType == FitType.FixedColumns && squareCellSize)
        {
            cellSize.y = cellSize.x;
        }

        if (fitType == FitType.FixedBoth && squareCellSize)
        {
            autoSpacing = true;
            float innerWidth = parentWidth - padding.left - padding.right;
            float innerHeight = parentHeight - padding.top - padding.bottom;
            float normalizedCellSize = Math.Min(innerWidth / columns, innerHeight / rows);

            cellSize.x = normalizedCellSize - minSpacing.x;
            cellSize.y = normalizedCellSize - minSpacing.y;

            spacing.x = (innerWidth - cellSize.x * columns) / (columns - 1);
            spacing.y = (innerHeight - cellSize.y * rows) / (rows - 1);
        }
        else
        {
            autoSpacing = false;
        }

        int columnCount;
        int rowCount;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}