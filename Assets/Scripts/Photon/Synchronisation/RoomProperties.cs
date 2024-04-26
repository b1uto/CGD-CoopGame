using ExitGames.Client.Photon;
using Photon.Pun;

namespace CGD.Networking
{
    public static class RoomProperties
    {
        /// <summary>
        /// Room Code Characters
        /// </summary>
        private static readonly char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        /// <summary>
        /// 
        /// </summary>
        private static readonly System.Random random = new System.Random();


        /// <summary>
        /// Used for creating room.
        /// </summary>
        public static int MaxPlayersPerRoom = 4;

        /// <summary>
        /// Room Name used for display purposes
        /// </summary>
        public const string RoomName = "RoomName";

        /// <summary>
        /// Game Mode
        /// </summary>
        public const string TeamGame = "TeamGame";

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
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.IsMasterClient)
            {
                var hashtable = CreateCustomRoomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
                hashtable[GameStarted] = gameStarted;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
        }

        public static string[] GetLobbyProperties() => new string[]
        {
            GameStarted,
            RoomName
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameStarted"></param>
        /// <returns></returns>
        public static Hashtable CreateCustomRoomProperties(bool gameStarted = false, string roomName = "", bool teams = false)
        {
            return new Hashtable
            {
                { GameStarted, false },
                { RoomName , roomName},
                { TeamGame , teams}
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


        public static string GenerateCode(int length = 6)
        {
            var code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = characters[random.Next(characters.Length)];
            }
            return new string(code);
        }


    }
}