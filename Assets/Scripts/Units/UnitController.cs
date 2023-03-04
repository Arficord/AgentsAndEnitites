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
        
        private Transform _transform;

        private Sequence _movingSequence;
        private Path _path;
        private int _destinationNodeId;

        private Crowd _crowd;
        
        private void Awake()
        {
            _transform = transform;
        }

        public void SetCrowd(Crowd crowd)
        {
            _crowd = crowd;
            _crowd.SetNewDestination(this);
        }
        
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        public void SetDestination(Vector3 destination)
        {
            seeker.StartPath(_transform.position, destination, OnPathComplete);
        }

        #region PoolRequests
        public void OnTakeFromPoolRequest()
        {
            gameObject.SetActive(true);
        }
        
        public void OnReturnedToPoolRequest()
        {
            _movingSequence?.Kill();
            gameObject.SetActive(false);
        }

        public void OnDestroyRequest()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Moving

        private void OnPathComplete(Path path)
        {
            if (path.error)
            {
                _crowd.OnPathError(this, path);
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
                _crowd.OnReachedDestination(this);
                return;
            }
            
            MoveTowards(_path.vectorPath[_destinationNodeId], MoveToNextNode);
        }
        
        private void MoveTowards(Vector3 position, TweenCallback onReachCallback)
        {
            //TODO: try to optimize
            //It is better to create one Tween and use SetPositionAndRotation to not send update events twice 
            //Also it's more optimal to use LocalPosition due to avoidance of world position calculation
            _movingSequence = DOTween.Sequence();
            
            //in fact here speed is delay. Should change logic to constant speed
            _movingSequence.Insert(0, _transform.DOLookAt(position, rotateSpeed));
            _movingSequence.Insert(0, _transform.DOMove(position, speed).SetEase(Ease.Linear));
            _movingSequence.onComplete += onReachCallback;
            _movingSequence.Play();
        }
        #endregion
    }
}