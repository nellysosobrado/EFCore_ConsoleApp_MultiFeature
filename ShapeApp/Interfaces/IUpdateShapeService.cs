﻿namespace ShapeApp.Services;

public interface IUpdateShapeService
{
    void UpdateShape(int id);
    int GetShapeIdForUpdate();
    bool ShouldChangeShapeType();
}