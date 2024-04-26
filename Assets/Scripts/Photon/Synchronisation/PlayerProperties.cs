using ExitGames.Client.Photon;

namespace CGD.Networking
{
    public static class PlayerProperties
    {
        /// <summary>
        /// icon used for profile displays
        /// </summary>
        public const string Icon = "Icon";

        /// <summary>
        /// character model used by player
        /// </summary>
        public const string Model = "Model";
    
        public static string[] GetPlayerProperties() => new string[] { Icon, Model };

        public static Hashtable CreatePlayerProperties(int icon, int model)
        {
            return new Hashtable
            {
                { Icon, icon},
                { Model , model}            
            };
        }

    }
}