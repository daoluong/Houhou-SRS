﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kanji.Common.Helpers
{
    public static class ConfigurationHelper
    {
        #region Constants

        /// <summary>
        /// Stores the path to the root application data directory.
        /// </summary>
        public static readonly string DataRootPath = "Data";

        /// <summary>
        /// Stores the path to the user content replicator directory.
        /// Subdirectories and files will be replicated to the user content directory.
        /// </summary>
        public static readonly string DataUserContentDirectoryPath = Path.Combine(DataRootPath, "UserContent");

        /// <summary>
        /// Stores the path to the default database file contained in the data user content directory.
        /// </summary>
        public static readonly string DataUserContentDefaultDatabaseFilePath = Path.Combine(
            DataUserContentDirectoryPath, "SrsDatabase.sqlite");

        /// <summary>
        /// Stores the path to the user content root directory path.
        /// </summary>
        #if DEBUG

        public static readonly string UserContentDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Houhou (Debug)");

        #else

        public static readonly string UserContentDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Houhou");

        #endif

        /// <summary>
        /// Stores the path to the user RadicalSets root directory.
        /// </summary>
        public static readonly string UserContentRadicalDirectoryPath = Path.Combine(
            UserContentDirectoryPath, "Radicals");

        /// <summary>
        /// Stores the path to the user SRSLevelSets root directory.
        /// </summary>
        public static readonly string UserContentSrsLevelDirectoryPath = Path.Combine(
            UserContentDirectoryPath, "SrsLevels");

        /// <summary>
        /// Stores the path to the SRS Database file.
        /// </summary>
        public static readonly string UserContentSrsDatabaseFilePath = Path.Combine(
            UserContentDirectoryPath, "SrsDatabase.sqlite");

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the user content access.
        /// </summary>
        public static void InitializeConfiguration()
        {
            // Make sure user content directories exist.
            CreateDirectoryIfNotExist(UserContentDirectoryPath);
            CreateDirectoryIfNotExist(UserContentRadicalDirectoryPath);
            CreateDirectoryIfNotExist(UserContentSrsLevelDirectoryPath);

            // Make sure the initial files exist.
            ReplicateInitialUserContent();

            // Make sure the database file exists.
            if (!File.Exists(UserContentSrsDatabaseFilePath))
            {
                File.Copy(DataUserContentDefaultDatabaseFilePath,
                    UserContentSrsDatabaseFilePath);
            }
        }

        /// <summary>
        /// Creates a directory accessed by the given path if it
        /// does not already exist.
        /// </summary>
        /// <param name="directoryPath">Path to the directory
        /// to create.</param>
        private static void CreateDirectoryIfNotExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// Replicates the initial user content from the application data files
        /// to the user content directory if needed.
        /// </summary>
        private static void ReplicateInitialUserContent()
        {
            // Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(DataUserContentDirectoryPath, "*",
                SearchOption.AllDirectories))
            {
                string newPath = dirPath.Replace(DataUserContentDirectoryPath, UserContentDirectoryPath);
                CreateDirectoryIfNotExist(newPath);
            }

            // Copy all the files
            foreach (string filePath in Directory.GetFiles(DataUserContentDirectoryPath, "*",
                SearchOption.AllDirectories))
            {
                string newPath = filePath.Replace(DataUserContentDirectoryPath, UserContentDirectoryPath);

                if (!File.Exists(newPath))
                {
                    File.Copy(filePath, newPath);
                }
            }
        }

        #endregion
    }
}