using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathRequest
{
	public Vector2 PathStart;
	public Vector2 PathEnd;
	public Action<Vector2[], bool> Callback;

	public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
	{
		PathStart = start;
		PathEnd = end;
		Callback = callback;
	}
}

public class PathRequestManager : StaticInstance<PathRequestManager>
{
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	private PathFinder pathFinder;
	private bool isProcessingPath;

	protected override void Awake()
	{
		base.Awake();

		pathFinder = GetComponent<PathFinder>();
	}

	public static void RequestPath(Vector2 startPos, Vector2 endPos, Action<Vector2[], bool> callback)
	{
		Instance.pathRequestQueue.Enqueue(new PathRequest(startPos, endPos, callback));
		Instance.TryProcessNextPath();
	}

	private void TryProcessNextPath()
	{
		if(!isProcessingPath && pathRequestQueue.Count > 0)
		{
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathFinder.StartFindPath(currentPathRequest.PathStart, currentPathRequest.PathEnd);
		}
	}

	public void FinishProcessingPath(Vector2[] path, bool succes)
	{
		currentPathRequest.Callback(path, succes);
		isProcessingPath = false;
		TryProcessNextPath();
	}
}