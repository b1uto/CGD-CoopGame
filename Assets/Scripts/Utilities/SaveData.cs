//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using Zlib;

//[System.Serializable]
//public static class SaveData
//{
//    //delimiters 
//    // , fields 
//    // : element fields
//    // | array elements 
//    // # end of section
//    //§

   

//    #region Load/Save Functions
//    public static bool ProfileDataFromCSV(string saveData, out OfflineProfileData serverProfileData)
//    {
//        serverProfileData = new OfflineProfileData();
//        serverProfileData.userProfile = ScriptableObject.CreateInstance<UserProfile>();

//        if (!string.IsNullOrEmpty(saveData))
//        {

//            string[] partitions = saveData.Contains('§') ? saveData.Split('§') : saveData.Split('#');

//            if (partitions.Length >= 3)
//            {
//                var userProfileValid = LoadUserProfileFromCSV(serverProfileData, partitions[0]);
//                var userUnlocksValid = LoadUserUnlocksFromCSV(serverProfileData, partitions[1]);
//                var userTeamValid = LoadUserTeamCSV(serverProfileData, partitions[2]);

//                if (userProfileValid && userUnlocksValid && userTeamValid)
//                {

//                    return true;
//                }
//                else
//                {
//#if UNITY_EDITOR
//                    Debug.LogError("Error: Server CSV format is incorrect, Loading New Profile");
//#endif
//                }
//            }
//            else
//            {
//#if UNITY_EDITOR
//                Debug.LogError("Error: Server CSV format is incorrect, Loading New Profile");
//#endif
//            }
//        }

//        return false;
//    }
//    public static string ProfileDataToCSV(OfflineProfileData profileData)
//    {
//        StringBuilder sb = new StringBuilder();

//        CreateUserProfileCSV(profileData.userProfile, sb);
//        CreateUserUnlocksCSV(profileData, sb);
//        CreateUserTeamCSV(profileData, sb);
//        var saveDataCSV = sb.ToString();
//        sb.Clear();

//        return saveDataCSV;
//    }
//    #endregion

//    #region CSV Validity
//    public static bool CheckDataValidity(string data)
//    {
//        var valid = false;

//        string[] partitions = data.Contains('§') ? data.Split('§') : data.Split('#');

//        if (partitions.Length == 3)
//        {
//            valid = CSV_UserProfileValidity(partitions[0]) && CSV_UserUnlocksValidity(partitions[1]) && CSV_UserTeamValidity(partitions[2]);
//        }

//        if (!valid)
//        {
//#if DEBUGGING
//            Debug.LogWarning("Error: SaveData CSV format is invalid");
//#endif
//        }
//        return valid;
//    }
//    private static bool CSV_UserProfileValidity(string data)
//    {
//#if UNITY_EDITOR
//        // Debug.LogWarning("User Profile Valid: " + (data.Split(',').Length >= 24));
//#endif
//        return data.Split(',').Length >= 24;
//    }
//    private static bool CSV_UserUnlocksValidity(string data)
//    {
//#if UNITY_EDITOR
//        // Debug.LogWarning("User Unlocks Valid: " + (data.Split(',').Length >= 10));
//#endif
//        return data.Split(',').Length >= 10;
//    }
//    private static bool CSV_UserTeamValidity(string data)
//    {
//#if UNITY_EDITOR
//        // Debug.LogWarning("User Team Valid: " + (data.Split(',').Length >= 7));
//#endif
//        return data.Split(',').Length >= 7;
//    }
//    #endregion

//    #region Load From CSV
//    private static bool LoadUserProfileFromCSV(OfflineProfileData profileData, string data)
//    {
//        string[] dataParts = data.Split(',');

//        if (dataParts.Length >= 24)
//        {
//            var userProfile = ScriptableObject.CreateInstance<UserProfile>();

//            userProfile.userName = dataParts[0];
//            userProfile.userLevel = System.Convert.ToInt32(dataParts[1]);

//            userProfile.currentXP = System.Convert.ToInt32(dataParts[2]);
//            userProfile.currentCoins = System.Convert.ToInt32(dataParts[3]);
//            userProfile.currentCash = System.Convert.ToInt32(dataParts[4]);
//            userProfile.currentDunks = System.Convert.ToInt32(dataParts[5]);

//            userProfile.totalCoins = System.Convert.ToInt32(dataParts[6]);
//            userProfile.totalCash = System.Convert.ToInt32(dataParts[7]);

//            userProfile.gamesPlayed = System.Convert.ToInt32(dataParts[8]);
//            userProfile.gamesWon = System.Convert.ToInt32(dataParts[9]);

//            userProfile.totalPoints = System.Convert.ToInt32(dataParts[10]);
//            userProfile.totalSteals = System.Convert.ToInt32(dataParts[11]);
//            userProfile.totalDunks = System.Convert.ToInt32(dataParts[12]);
//            userProfile.totalShotsTaken = System.Convert.ToInt32(dataParts[13]);
//            userProfile.total1v1 = System.Convert.ToInt32(dataParts[14]);
//            userProfile.total2v2 = System.Convert.ToInt32(dataParts[15]);
//            userProfile.total3v3 = System.Convert.ToInt32(dataParts[16]);


//            userProfile.targetedAds = System.Convert.ToInt32(dataParts[17]);
//            userProfile.newUserLevel = System.Convert.ToInt32(dataParts[18]);
//            userProfile.freeGiftLevel = System.Convert.ToInt32(dataParts[19]);


//            userProfile.lastFreeGiftTime = dataParts[20];
//            userProfile.purchasedBundles = dataParts[21];


//            userProfile.userStage = System.Convert.ToInt32(dataParts[22]);

//            if (System.Int32.TryParse(dataParts[23], out int flagIndex))
//            {
//                userProfile.userFlag = RegionalBiasController.Instance.GetRandomCountry(flagIndex);
//            }
//            else
//            {
//                userProfile.userFlag = dataParts[23];
//            }

//            if (dataParts.Length > 25)
//            {
//                userProfile.freeShotsAvailable = System.Convert.ToInt32(dataParts[24]);
//                userProfile.goldenShotsAvailable = System.Convert.ToInt32(dataParts[25]);
//            }

//            if (dataParts.Length > 27)
//            {
//                userProfile.currentDraftPoints = System.Convert.ToInt32(dataParts[26]);
//                userProfile.totalDraftPoints = System.Convert.ToInt32(dataParts[27]);
//            }

//            if (dataParts.Length > 29)
//            {
//                userProfile.freeDraftPointsLevel = System.Convert.ToInt32(dataParts[28]);
//                userProfile.lastFreeDraftPointsTime = dataParts[20];
//            }

//            profileData.userProfile = userProfile;
//            return true;
//        }
//        else
//        {
//#if UNITY_EDITOR
//            Debug.LogError("Error: Profile Partition Not Correct Length");
//#endif
//            return false;
//        }
//    }
//    private static bool LoadUserUnlocksFromCSV(OfflineProfileData profileData, string data)
//    {
//        string[] elements = data.Split(',');

//        if (elements.Length < 10)
//        {
//            return false;
//        }
//        else
//        {
//            //User Players
//            string[] savedUserPlayers = elements[0].Split('|');
//            profileData.userPlayers = new Dictionary<int, UserPlayer>();

//            foreach (string up in savedUserPlayers)
//            {
//                var parts = up.Split(':');

//                if (parts.Length >= 3)
//                {
//                    int id = System.Convert.ToInt32(parts[0]);
//                    int quantity = System.Convert.ToInt32(parts[1]);
//                    int traininglevel = System.Convert.ToInt32(parts[2]);

//                    var userPlayer = ScriptableObject.CreateInstance<UserPlayer>();

//                    if (userPlayer.Init(id, quantity, traininglevel))
//                        profileData.userPlayers.Add(userPlayer.Player, userPlayer);
//                }
//            }

//            profileData.userChests.Clear();

//            //User Chests
//            //Rookie
//            string[] savedRookieChests = elements[1].Split(':');
//            if (savedRookieChests[0] == "1")
//            {
//                for (int i = 1; i < savedRookieChests.Length; i++)
//                {
//                    if (!string.IsNullOrEmpty(savedRookieChests[i]))
//                    {
//                        profileData.userChests.Add(UserChest.CreateUserChest(1, savedRookieChests[i]));

//                    }
//                }
//            }
//            //Pro
//            string[] savedProChests = elements[2].Split(':');
//            if (savedProChests[0] == "2")
//            {
//                for (int i = 1; i < savedProChests.Length; i++)
//                {
//                    if (!string.IsNullOrEmpty(savedProChests[i]))
//                    {
//                        profileData.userChests.Add(UserChest.CreateUserChest(2, savedProChests[i]));

//                    }
//                }
//            }
//            //Elite
//            string[] savedEliteChests = elements[3].Split(':');
//            if (savedEliteChests[0] == "3")
//            {
//                for (int i = 1; i < savedEliteChests.Length; i++)
//                {
//                    if (!string.IsNullOrEmpty(savedEliteChests[i]))
//                    {
//                        profileData.userChests.Add(UserChest.CreateUserChest(3, savedEliteChests[i]));

//                    }
//                }
//            }


//            //Unlocked Items
//            profileData.unlockedJerseys.Clear();
//            profileData.unlockedPants.Clear();
//            profileData.unlockedBoots.Clear();
//            profileData.unlockedEmblems.Clear();
//            profileData.unlockedKits.Clear();
//            profileData.unlockedAccessories.Clear();
//            profileData.unlockedBasketballs.Clear();

//            string[] savedJerseys = elements[4].Split(':');
//            foreach (string savedJersey in savedJerseys)
//            {
//                if (!string.IsNullOrEmpty(savedJersey))
//                {
//                    profileData.unlockedJerseys.Add(System.Convert.ToInt32(savedJersey));
//                }
//            }

//            string[] savedPants = elements[5].Split(':');
//            foreach (string savedPant in savedPants)
//            {
//                if (!string.IsNullOrEmpty(savedPant))
//                {
//                    profileData.unlockedPants.Add(System.Convert.ToInt32(savedPant));
//                }
//            }

//            string[] savedBoots = elements[6].Split(':');
//            foreach (string savedBoot in savedBoots)
//            {
//                if (!string.IsNullOrEmpty(savedBoot))
//                {
//                    profileData.unlockedBoots.Add(System.Convert.ToInt32(savedBoot));
//                }
//            }

//            string[] savedEmblems = elements[7].Split(':');
//            foreach (string savedEmblem in savedEmblems)
//            {
//                if (!string.IsNullOrEmpty(savedEmblem))
//                {
//                    profileData.unlockedEmblems.Add(System.Convert.ToInt32(savedEmblem));
//                }
//            }

//            string[] savedKits = elements[8].Split(':');
//            foreach (string savedKit in savedKits)
//            {
//                if (!string.IsNullOrEmpty(savedKit))
//                {
//                    profileData.unlockedKits.Add(System.Convert.ToInt32(savedKit));
//                }
//            }


//            //Reward Chests
//            string[] rewardChests = elements[9].Split(':');
//            foreach (string rewardChest in rewardChests)
//            {
//                if (!string.IsNullOrEmpty(rewardChest))
//                {
//                    profileData.rewardChests.Add(System.Convert.ToInt32(rewardChest));
//                }
//            }


//            profileData.userTournamentsWon = new Dictionary<int, int>();

//            if (elements.Length > 10)
//            {
//                string[] userTournamentResults = elements[10].Split('|');

//                foreach (string userTournamentResult in userTournamentResults)
//                {
//                    var parts = userTournamentResult.Split(':');

//                    if (parts.Length >= 2)
//                    {
//                        int id = System.Convert.ToInt32(parts[0]);
//                        int quantity = System.Convert.ToInt32(parts[1]);

//                        profileData.userTournamentsWon.Add(id, quantity);
//                    }
//                }
//            }

//            if (elements.Length > 11)
//            {
//                string[] tierWinnings = elements[11].Split(':');

//                for (int i = 0; i < 8; i++)
//                {
//                    profileData.TierWinnings[i] = System.Convert.ToInt32(tierWinnings[i]);
//                }
//            }

//            if (elements.Length > 13)
//            {
//                string[] savedAccessories = elements[12].Split(':');
//                foreach (string savedAccessory in savedAccessories)
//                {
//                    if (!string.IsNullOrEmpty(savedAccessory))
//                    {
//                        profileData.unlockedAccessories.Add(System.Convert.ToInt32(savedAccessory));
//                    }
//                }

//                string[] savedBasketballs = elements[13].Split(':');
//                foreach (string savedBasketball in savedBasketballs)
//                {
//                    if (!string.IsNullOrEmpty(savedBasketball))
//                    {
//                        profileData.unlockedBasketballs.Add(System.Convert.ToInt32(savedBasketball));
//                    }
//                }
//            }
//            return true;
//        }
//    }
//    private static bool LoadUserTeamCSV(OfflineProfileData profileData, string data)
//    {
//        var userTeam = UserTeam.CreateUserTeam();

//        string[] parts = data.Split(',');

//        if (parts.Length >= 7)
//        {
//            userTeam.playerOne.Player = System.Convert.ToInt32(parts[0]);
//            userTeam.playerTwo.Player = System.Convert.ToInt32(parts[1]);
//            userTeam.playerThree.Player = System.Convert.ToInt32(parts[2]);
//            userTeam.TeamJersey = System.Convert.ToInt32(parts[3]);
//            userTeam.TeamPants = System.Convert.ToInt32(parts[4]);
//            userTeam.TeamBoots = System.Convert.ToInt32(parts[5]);
//            userTeam.TeamEmblem = System.Convert.ToInt32(parts[6]);

//            userTeam.playerOne.Jersey = userTeam.playerTwo.Jersey = userTeam.playerThree.Jersey = userTeam.TeamJersey;
//            userTeam.playerOne.Pants = userTeam.playerTwo.Pants = userTeam.playerThree.Pants = userTeam.TeamPants;
//            userTeam.playerOne.Boots = userTeam.playerTwo.Boots = userTeam.playerThree.Boots = userTeam.TeamBoots;
//            userTeam.playerOne.Emblem = userTeam.playerTwo.Emblem = userTeam.playerThree.Emblem = userTeam.TeamEmblem;


//            if (parts.Length == 14)
//            {
//                userTeam.playerOne.AccessorySet = System.Convert.ToInt32(parts[7]);
//                userTeam.playerOne.Boots = System.Convert.ToInt32(parts[8]);
//                userTeam.playerTwo.AccessorySet = System.Convert.ToInt32(parts[9]);
//                userTeam.playerTwo.Boots = System.Convert.ToInt32(parts[10]);
//                userTeam.playerThree.AccessorySet = System.Convert.ToInt32(parts[11]);
//                userTeam.playerThree.Boots = System.Convert.ToInt32(parts[12]);
//                userTeam.TeamBasketball = System.Convert.ToInt32(parts[13]);
//            }


//            userTeam.AssignAllItems();
//            profileData.userTeam = userTeam;

//            return true;
//        }
//        else
//        {
//            return false;
//        }

//    }

//    #endregion

//    #region Convert To CSV
//    private static void CreateUserProfileCSV(UserProfile userProfile, StringBuilder builder)
//    {
//        //delimiters 
//        // , fields 
//        // : element fields
//        // | array elements 
//        // # end of section
//        //§
//        var usernameServerSafe = userProfile.userName;

//        if (!string.IsNullOrEmpty(usernameServerSafe))
//        {
//            var charsToRemove = new string[] { ",", ";", ":", "|", "#", "§" };
//            foreach (var c in charsToRemove)
//            {
//                usernameServerSafe = usernameServerSafe.Replace(c, string.Empty);
//            }
//        }

//        builder.Append(usernameServerSafe);
//        builder.Append(",");

//        builder.Append(userProfile.userLevel);
//        builder.Append(",");

//        builder.Append(userProfile.currentXP);
//        builder.Append(",");

//        builder.Append(userProfile.currentCoins);
//        builder.Append(",");

//        builder.Append(userProfile.currentCash);
//        builder.Append(",");

//        builder.Append(userProfile.currentDunks);
//        builder.Append(",");

//        builder.Append(userProfile.totalCoins);
//        builder.Append(",");

//        builder.Append(userProfile.totalCash);
//        builder.Append(",");

//        builder.Append(userProfile.gamesPlayed);
//        builder.Append(",");

//        builder.Append(userProfile.gamesWon);
//        builder.Append(",");

//        builder.Append(userProfile.totalPoints);
//        builder.Append(",");

//        builder.Append(userProfile.totalSteals);
//        builder.Append(",");

//        builder.Append(userProfile.totalDunks);
//        builder.Append(",");

//        builder.Append(userProfile.totalShotsTaken);
//        builder.Append(",");

//        builder.Append(userProfile.total1v1);
//        builder.Append(",");

//        builder.Append(userProfile.total2v2);
//        builder.Append(",");

//        builder.Append(userProfile.total3v3);
//        builder.Append(",");

//        builder.Append(userProfile.targetedAds);
//        builder.Append(",");

//        builder.Append(userProfile.newUserLevel);
//        builder.Append(",");

//        builder.Append(userProfile.freeGiftLevel);
//        builder.Append(",");

//        builder.Append(userProfile.lastFreeGiftTime);
//        builder.Append(",");

//        builder.Append(userProfile.purchasedBundles);
//        builder.Append(",");

//        builder.Append(userProfile.userStage);
//        builder.Append(",");

//        builder.Append(RegionalBiasController.Instance.GetCountryIndex(userProfile.userFlag));
//        builder.Append(",");

//        builder.Append(userProfile.freeShotsAvailable);
//        builder.Append(",");

//        builder.Append(userProfile.goldenShotsAvailable);
//        builder.Append(",");

//        builder.Append(userProfile.currentDraftPoints);
//        builder.Append(",");

//        builder.Append(userProfile.totalDraftPoints);
//        builder.Append(",");

//        builder.Append(userProfile.freeDraftPointsLevel);
//        builder.Append(",");

//        builder.Append(userProfile.lastFreeDraftPointsTime);
//        builder.Append("§");
//    }
//    private static void CreateUserUnlocksCSV(OfflineProfileData profileData, StringBuilder builder)
//    {
//        //User Players
//        foreach (KeyValuePair<int, UserPlayer> kvp in profileData.userPlayers)
//        {
//            builder.Append($"{kvp.Key}:{kvp.Value.Quantity}:{kvp.Value.TrainingLevel}|");
//        }
//        builder.Append(",");

//        //User Chests
//        //Rookie
//        builder.Append("1:");
//        if (profileData.userChests.Any(x => x.PackId == 1))
//        {
//            var rookieChests = profileData.userChests.Where(x => x.PackId == 1).ToArray();
//            foreach (UserChest chest in rookieChests)
//            {
//                builder.Append($"{chest.UnlocksAt}:");
//            }
//        }
//        builder.Append(",");
//        //Pro
//        builder.Append("2:");
//        if (profileData.userChests.Any(x => x.PackId == 2))
//        {
//            var rookieChests = profileData.userChests.Where(x => x.PackId == 2).ToArray();
//            foreach (UserChest chest in rookieChests)
//            {
//                builder.Append($"{chest.UnlocksAt}:");
//            }
//        }
//        builder.Append(",");
//        //Elite
//        builder.Append("3:");
//        if (profileData.userChests.Any(x => x.PackId == 3))
//        {
//            var rookieChests = profileData.userChests.Where(x => x.PackId == 3).ToArray();
//            foreach (UserChest chest in rookieChests)
//            {
//                builder.Append($"{chest.UnlocksAt}:");
//            }
//        }
//        builder.Append(",");

//        //Jerseys
//        for (int i = 0; i < profileData.unlockedJerseys.Count; i++)
//        {
//            builder.Append(profileData.unlockedJerseys[i].ToString());
//            if (i < profileData.unlockedJerseys.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");
//        //Pants
//        for (int i = 0; i < profileData.unlockedPants.Count; i++)
//        {
//            builder.Append(profileData.unlockedPants[i].ToString());
//            if (i < profileData.unlockedPants.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");
//        //Boots
//        for (int i = 0; i < profileData.unlockedBoots.Count; i++)
//        {
//            builder.Append(profileData.unlockedBoots[i].ToString());
//            if (i < profileData.unlockedBoots.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");
//        //Emblems
//        for (int i = 0; i < profileData.unlockedEmblems.Count; i++)
//        {
//            builder.Append(profileData.unlockedEmblems[i].ToString());
//            if (i < profileData.unlockedEmblems.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");
//        //Kits
//        for (int i = 0; i < profileData.unlockedKits.Count; i++)
//        {
//            builder.Append(profileData.unlockedKits[i].ToString());
//            if (i < profileData.unlockedKits.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");
//        //Reward Chests
//        for (int i = 0; i < profileData.rewardChests.Count; i++)
//        {
//            builder.Append(profileData.rewardChests[i].ToString());
//            if (i < profileData.rewardChests.Count - 1) builder.Append(":");
//        }
//        builder.Append(",");

//        //User Tournament
//        if (profileData.userTournamentsWon != null)
//        {
//            foreach (KeyValuePair<int, int> kvp in profileData.userTournamentsWon)
//            {
//                builder.Append($"{kvp.Key}:{kvp.Value}|");
//            }
//        }
//        builder.Append(",");

//        builder.Append($"{profileData.TierWinnings[0]}:{profileData.TierWinnings[1]}:{profileData.TierWinnings[2]}:" +
//            $"{profileData.TierWinnings[3]}:{profileData.TierWinnings[4]}:{profileData.TierWinnings[5]}:" +
//            $"{profileData.TierWinnings[6]}:{profileData.TierWinnings[7]}:{profileData.TierWinnings[8]}:{profileData.TierWinnings[9]}");



//        builder.Append(",");
//        //TODO ADD UNLOCKED ACCESSORY SETS + BASKETBALLS.
//        //AccessorySets
//        for (int i = 0; i < profileData.unlockedAccessories.Count; i++)
//        {
//            builder.Append(profileData.unlockedAccessories[i].ToString());
//            if (i < profileData.unlockedAccessories.Count - 1) builder.Append(":");
//        } //Basketballs

//        builder.Append(",");
//        for (int i = 0; i < profileData.unlockedBasketballs.Count; i++)
//        {
//            builder.Append(profileData.unlockedBasketballs[i].ToString());
//            if (i < profileData.unlockedBasketballs.Count - 1) builder.Append(":");
//        }


//        builder.Append("§");
//    }
//    private static void CreateUserTeamCSV(OfflineProfileData profileData, StringBuilder builder)
//    {
//        if (profileData.userTeam != null)
//        {
//            builder.Append($"{profileData.userTeam.playerOne.Player},");
//            builder.Append($"{profileData.userTeam.playerTwo.Player},");
//            builder.Append($"{profileData.userTeam.playerThree.Player},");

//            builder.Append($"{profileData.userTeam.TeamJersey},");
//            builder.Append($"{profileData.userTeam.TeamPants},");
//            builder.Append($"{profileData.userTeam.TeamBoots},");
//            builder.Append($"{profileData.userTeam.TeamEmblem},"); //TODO ADD COMMA!

//            //TODO Add Accessory Set and Unique Boots
//            builder.Append($"{profileData.userTeam.playerOne.AccessorySet},");
//            builder.Append($"{profileData.userTeam.playerOne.Boots},");
//            builder.Append($"{profileData.userTeam.playerTwo.AccessorySet},");
//            builder.Append($"{profileData.userTeam.playerTwo.Boots},");
//            builder.Append($"{profileData.userTeam.playerThree.AccessorySet},");
//            builder.Append($"{profileData.userTeam.playerThree.Boots},");
//            builder.Append($"{profileData.userTeam.TeamBasketball}");
//        }
//    }
//    #endregion

//    #region COMPRESSION
//    private static void CompressDataString(string saveDataCSV)
//    {
//        var zlib = new Zlib.ZlibCodec(Zlib.CompressionMode.Compress);

//        byte[] UncompressedBytes = Encoding.Default.GetBytes(saveDataCSV);


//        Debug.LogWarning("Uncompressed Length: " + UncompressedBytes.Length);


//        int bufferSize = 1024;
//        byte[] buffer = new byte[bufferSize];
//        ZlibCodec compressor = new ZlibCodec();


//        var ms = new System.IO.MemoryStream();

//        //System.Console.WriteLine("\n============================================");
//        //System.Console.WriteLine("Size of Buffer to Deflate: {0} bytes.", UncompressedBytes.Length);
//        int rc = compressor.InitializeDeflate(Zlib.CompressionLevel.BestSpeed);

//        compressor.InputBuffer = UncompressedBytes;
//        compressor.NextIn = 0;
//        compressor.AvailableBytesIn = UncompressedBytes.Length;

//        compressor.OutputBuffer = buffer;

//        //  pass 1: deflate 
//        do
//        {
//            compressor.NextOut = 0;
//            compressor.AvailableBytesOut = buffer.Length;
//            rc = compressor.Deflate(FlushType.None);

//            if (rc != ZlibConstants.Z_OK && rc != ZlibConstants.Z_STREAM_END)
//            {
//                throw new System.Exception("deflating: " + compressor.Message);
//            }

//            ms.Write(compressor.OutputBuffer, 0, buffer.Length - compressor.AvailableBytesOut);
//        }
//        while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);

//        //   pass 2: finish and flush
//        do
//        {
//            compressor.NextOut = 0;
//            compressor.AvailableBytesOut = buffer.Length;
//            rc = compressor.Deflate(FlushType.Finish);

//            if (rc != ZlibConstants.Z_STREAM_END && rc != ZlibConstants.Z_OK)
//                throw new System.Exception("deflating: " + compressor.Message);

//            if (buffer.Length - compressor.AvailableBytesOut > 0)
//                ms.Write(buffer, 0, buffer.Length - compressor.AvailableBytesOut);
//        }
//        while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);

//        compressor.EndDeflate();

//        ms.Seek(0, System.IO.SeekOrigin.Begin);
//        var CompressedBytes = new byte[compressor.TotalBytesOut];
//        ms.Read(CompressedBytes, 0, CompressedBytes.Length);


//        var savePath = Application.persistentDataPath + "/ServerSaveData_Binary.txt";

//        System.IO.File.WriteAllBytes(savePath, CompressedBytes);


//        Debug.LogWarning("Compressed Length: " + CompressedBytes.Length);
//    }

//    private static string DeCompressDataToString(byte[] CompressedBytes)
//    {
//        int bufferSize = 1024;
//        byte[] buffer = new byte[bufferSize];

//        ZlibCodec decompressor = new ZlibCodec();

//        //Console.WriteLine("\n============================================");
//        //Console.WriteLine("Size of Buffer to Inflate: {0} bytes.", CompressedBytes.Length);
//        var ms = new System.IO.MemoryStream(0);

//        int rc = decompressor.InitializeInflate();

//        decompressor.InputBuffer = CompressedBytes;
//        decompressor.NextIn = 0;
//        decompressor.AvailableBytesIn = CompressedBytes.Length;

//        decompressor.OutputBuffer = buffer;

//        //   pass 1: inflate 
//        do
//        {
//            decompressor.NextOut = 0;
//            decompressor.AvailableBytesOut = buffer.Length;
//            rc = decompressor.Inflate(FlushType.None);

//            if (rc != ZlibConstants.Z_OK && rc != ZlibConstants.Z_STREAM_END)
//            {
//                throw new System.Exception("inflating: " + decompressor.Message);
//            }

//            ms.Write(decompressor.OutputBuffer, 0, buffer.Length - decompressor.AvailableBytesOut);
//        }
//        while (decompressor.AvailableBytesIn > 0 || decompressor.AvailableBytesOut == 0);

//        //    pass 2: finish and flush
//        do
//        {
//            decompressor.NextOut = 0;
//            decompressor.AvailableBytesOut = buffer.Length;
//            rc = decompressor.Inflate(FlushType.Finish);

//            if (rc != ZlibConstants.Z_STREAM_END && rc != ZlibConstants.Z_OK)
//            {
//                throw new System.Exception("inflating: " + decompressor.Message);
//            }

//            if (buffer.Length - decompressor.AvailableBytesOut > 0)
//                ms.Write(buffer, 0, buffer.Length - decompressor.AvailableBytesOut);
//        }
//        while (decompressor.AvailableBytesIn > 0 || decompressor.AvailableBytesOut == 0);

//        decompressor.EndInflate();


//        var decompressedBytes = decompressor.OutputBuffer;

//        return System.Text.ASCIIEncoding.ASCII.GetString(decompressedBytes, 0, decompressedBytes.Length);
//    }

//    public static void GetDataString()
//    {
//        var savePath = Application.persistentDataPath + "/ServerSaveData_Binary.txt";

//        if (System.IO.File.Exists(savePath))
//        {
//            var data = System.IO.File.ReadAllBytes(savePath);

//            var dataString = DeCompressDataToString(data);


//            savePath = Application.persistentDataPath + "/ServerSaveData.csv";

//            if (System.IO.File.Exists(savePath))
//            {
//                var savedData = System.IO.File.ReadAllText(savePath);

//#if UNITY_EDITOR
//                Debug.LogWarning($"SERVER DATA DECOMPRESSED: {savedData == dataString}");
//                Debug.LogWarning(savedData);
//                Debug.LogWarning(dataString);
//#endif
//            }
//        }
//        else
//        {
//#if UNITY_EDITOR
//            Debug.LogWarning($"File Not Found: {savePath}");
//#endif
//        }
//    }
//    #endregion
//}
