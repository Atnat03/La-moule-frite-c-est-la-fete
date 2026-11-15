using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class ClientManager : MonoBehaviour
    {
        [SerializeField] private int _maxClientSimultaner = 2;
        [SerializeField] private ClientCommand _clientPrefab;
        [SerializeField] private Transform[] _possiblePoints;
        
        [SerializeField] private Vector2 _timerDeSouleEtIlsSeCassentApres = new Vector2(5, 10);
        
        private Dictionary<ClientCommand, float> _dict = new Dictionary<ClientCommand, float>();

        void Start()
        {
            while (_dict.Count < _maxClientSimultaner)
            {
                NewClientArrived();
            }
        }

        private void NewClientArrived()
        {
            ClientCommand c = Instantiate(_clientPrefab, _possiblePoints[Random.Range(0, _possiblePoints.Length)].position, Quaternion.identity);

            c.OnGetHisCommand += RemoveFromDict;

            float r = Random.Range(_timerDeSouleEtIlsSeCassentApres.x, _timerDeSouleEtIlsSeCassentApres.y);
            c.MaxTimer = r;
            _dict.Add(c, r);
        }

        private void RemoveFromDict(ClientCommand clientCommand)
        {
            _dict.Remove(clientCommand);
            clientCommand.ClientPartis();
            
            if (_dict.Count < _maxClientSimultaner)
            {
                NewClientArrived();
            }
        }
        
        private void Update()
        {
            var dictCopy = new Dictionary<ClientCommand, float>(_dict);

            foreach (var pair in dictCopy)
            {
                float c = _dict[pair.Key] -= Time.deltaTime;
                
                pair.Key.UpdateTimer(c);
                
                if (_dict[pair.Key] <= 0)
                {
                    RemoveFromDict(pair.Key);
                }
            }
        }
    }
}