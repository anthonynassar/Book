using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace PeopleApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserId", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserId", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static DateTime AccessTokenExpiration
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessTokenExpiration", DateTime.UtcNow);
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessTokenExpiration", value);
            }
        }

        public static string IdentityProvider
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessTokenExpiration", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessTokenExpiration", value);
            }
        }

        public static string PhotoAlbumPath
        {
            get
            {
                return AppSettings.GetValueOrDefault("PhotoAlbumPath", "/storage/emulated/0/Pictures/PeopleApp");
            }
            set
            {
                AppSettings.AddOrUpdateValue("PhotoAlbumPath", value);
            }
        }

        public static string CurrentSharingSpace
        {
            get
            {
                return AppSettings.GetValueOrDefault("CurrentSharingSpace", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("CurrentSharingSpace", value);
            }
        }

        public static int SharingSpaceCount
        {
            get
            {
                return AppSettings.GetValueOrDefault("SharingSpaceCount", -1);
            }
            set
            {
                AppSettings.AddOrUpdateValue("SharingSpaceCount", value);
            }
        }

        public static void ResetAll()
        {
            CurrentSharingSpace = "";
            PhotoAlbumPath = "";
            IdentityProvider = "";
            AccessToken = "";
            AccessTokenExpiration = DateTime.Now.AddDays(-60);
            UserId = "";
            Username = "";
            Password = "";
            SharingSpaceCount = -1;
        }
    }
}
