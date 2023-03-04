using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace MyNamespace.Units
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private Seeker seeker;
        [SerializeField] private float rotateSpeed = 1f;
        [SerializeField] private float speed = 1f;
        
        //TODO: delete. Currently here for debug purpose
        [SerializeField] private Transform debugTarget;
        
        private Transform _transform;

        private Path _path;
        private int _destinationNodeId;
        
        private void Awake()
        {
            _transform = transform;
        }
        
        //TODO: delete and delegate this work to manager
        private void Start()
        {
            CalculatePath();
        }

        private void CalculatePath()
        {
            seeker.StartPath(_transform.position, debugTarget.position, OnPathComplete);
        }

        private void OnPathComplete(Path path)
        {
            if (path.error)
            {
                //TODO: next step throw callback to some sort of crowd manager
                Debug.LogError(path.errorLog, gameObject);
                return;
            }
            
            StartMoveAlongThePath(path);
        }

        private void StartMoveAlongThePath(Path path)
        {
            if (path == null || path.vectorPath.Count == 0)
            {
                Debug.LogError("Path is broken", gameObject);
                return;
            }
            
            _path = path;
            _destinationNodeId = 0;
            MoveToNextNode();
        }

        private void MoveToNextNode()
        {
            _destinationNodeId++;

            if (_destinationNodeId >= _path.vectorPath.Count)
            {
                //TODO: next step throw callback to some sort of crowd manager
                Debug.Log("ReachedEndPoint");
                return;
            }
            
            MoveTowards(_path.vectorPath[_destinationNodeId], MoveToNextNode);
        }
        
        private void MoveTowards(Vector3 position, TweenCallback onReachCallback)
        {
            //TODO: try to optimize
            //It is better to create one Tween and use SetPositionAndRotation to not send update events twice 
            //Also it's more optimal to use LocalPosition due to avoidance of world position calculation
            var sequence = DOTween.Sequence();
            sequence.Insert(0, _transform.DOLookAt(position, rotateSpeed));
            sequence.Insert(0, _transform.DOMove(position, speed).SetEase(Ease.Linear));
            sequence.onComplete += onReachCallback;
            sequence.Play();
        }
    }
}