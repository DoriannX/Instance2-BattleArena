using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class RoomManager
    {
        private static List<string> _allRoomCodes = new();

        public static void AddRoomCode(string code)
        {
            _allRoomCodes.Add(code);
        }

        public static string GetRandomCode()
        {
            int randomIndex = Random.Range(0, _allRoomCodes.Count);
            string randomCode = _allRoomCodes[randomIndex];
            return randomCode;
        }
    }
}