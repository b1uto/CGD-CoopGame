using ExitGames.Client.Photon;
using Photon.Pun;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CGD
{
    public static class RoomProperties
    {
        /// <summary>
        /// Used for creating room.
        /// </summary>
        public static int MaxPlayersPerRoom = 4;

        /// <summary>
        /// Key for room stored in PlayerPrefs
        /// </summary>
        public const string RoomKey = "Room_Key";

        /// <summary>
        /// boolean, has game started.
        /// </summary>
        public const string GameStarted = "GameStarted";

        /// <summary>
        /// update game started
        /// </summary>
        /// <param name="hashtable"></param>
        /// <param name="gameStarted"></param>
        public static void SetGameStarted(bool gameStarted)
        {
            if(PhotonNetwork.CurrentRoom != null && PhotonNetwork.IsMasterClient) 
            {
                var hashtable = CreateCustomRoomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
                hashtable[GameStarted] = gameStarted;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
        }

        public static string[] GetLobbyProperties() => new string[] { GameStarted };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameStarted"></param>
        /// <returns></returns>
        public static Hashtable CreateCustomRoomProperties(bool gameStarted = false)
        {
            return new Hashtable
            {
                { GameStarted, false }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameStarted"></param>
        /// <returns></returns>
        public static Hashtable CreateCustomRoomProperties(Hashtable oldCustomRoomProperties)
        {
            var newCustomProperties = new Hashtable();

            foreach (var prop in oldCustomRoomProperties.Keys)
            {
                newCustomProperties.Add(prop, oldCustomRoomProperties[prop]);
            }
            return newCustomProperties;
        }

    }
}