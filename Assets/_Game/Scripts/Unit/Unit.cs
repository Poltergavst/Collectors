using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(ObstacleAvoider), typeof(TargetReacher))]
[RequireComponent(typeof(Carrier))]
public class Unit : MonoBehaviour
{
    private UnitMover _unitMover;
    private ObstacleAvoider _obstacleAvoider;
    private Coroutine _tasksCoroutine;
    private Queue<IUnitTask> _taskQueue;

    public event Action TasksCompleted;

    public Carrier Carrier { get; private set; }
    public TargetReacher Reacher { get; private set; }

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        _obstacleAvoider = GetComponent<ObstacleAvoider>();
        _taskQueue = new Queue<IUnitTask>();

        Carrier = GetComponent<Carrier>();
        Reacher = GetComponent<TargetReacher>();

        Reacher.Initialize(_unitMover, _obstacleAvoider);
    }

    public void AddTaskToQueue(IUnitTask task)
    {
        _taskQueue.Enqueue(task);
    }

    public void PerformTasks()
    {
        if (_tasksCoroutine != null)
            return;

        _tasksCoroutine = StartCoroutine(PerformTasksCoroutine());
    }    

    private IEnumerator PerformTasksCoroutine()
    {
        while (_taskQueue.Count > 0)
            yield return _taskQueue.Dequeue().Execute(this);

        _tasksCoroutine = null;
        TasksCompleted?.Invoke();
    }    
}