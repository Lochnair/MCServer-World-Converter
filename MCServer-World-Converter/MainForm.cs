/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2015 Nils Andreas Svee
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Substrate;
using Substrate.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCServer_World_Converter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void sourceBrowseButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sourceTextBox.Text = dialog.SelectedPath;
            }
        }

        private void outputBrowseButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                outputTextBox.Text = dialog.SelectedPath;
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(sourceTextBox.Text) || String.IsNullOrEmpty(outputTextBox.Text))
            {
                MessageBox.Show("Both source and output folders must be set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.Equals(sourceTextBox.Text, outputTextBox.Text))
            {
                MessageBox.Show("Source and output can't be the same folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Run(sourceTextBox.Text, outputTextBox.Text, false);
        }

        private static JArray convertInventory(ItemCollection itemCollection, int length)
        {
            JArray array = new JArray();

            for (int i = 0; i < length; i++)
            {
                Item item = itemCollection[i];
                JObject jItem = new JObject();

                if (item != null)
                {
                    jItem = convertItemToObject(item);
                }
                else
                {
                    jItem.Add("ID", -1);
                }

                array.Add(jItem);
            }

            return array;
        }

        private static JObject convertItemToObject(Item item)
        {
            JObject jItem = new JObject();
            jItem.Add("Count", item.Count);
            jItem.Add("Health", item.Damage);
            jItem.Add("ID", item.ID);
            jItem.Add("RepairCost", 0);
            return jItem;
        }

        private static JArray convertPlayerInventory(ItemCollection itemCollection)
        {
            JArray array = new JArray();

            // The JSON originally included the 4 crafting slots and the result, so we have to put empty items there, too:
            JObject emptyItem = new JObject();
            emptyItem.Add("ID", -1);
            for (int i = 0; i < 5; i++ )
            {
                array.Add(emptyItem);
            }

            // Count downwards to get armor saved in the right order
            for (int i = 103; i > 99; i--)
            {
                if (itemCollection[i] == null)
                {
                    array.Add(emptyItem);
                }
                else
                {
                    array.Add(convertItemToObject(itemCollection[i]));
                }
            }

            // Save main inventory
            for (int i = 9; i < 36; i++)
            {
                if (itemCollection[i] == null)
                {
                    array.Add(emptyItem);
                }
                else
                {
                    array.Add(convertItemToObject(itemCollection[i]));
                }
            }

            // Save hotbar
            for (int i = 0; i < 9; i++ )
            {
                if (itemCollection[i] == null)
                {
                    array.Add(emptyItem);
                }
                else
                {
                    array.Add(convertItemToObject(itemCollection[i]));
                }
            }

            return array;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    if(subdir.Name == "playerdata" || subdir.Name == "players")
                    {
                        continue;
                    }

                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static IDictionary<string, string> ReadDictionaryFile(string fileName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string line in File.ReadAllLines(fileName))
            {
                if ((!string.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        public static void Run(string sourcePath, string outputPath, bool console)
        {
            string playerPath = Path.Combine(outputPath, "players");

            if (!Directory.Exists(playerPath))
            {
                Directory.CreateDirectory(playerPath);
            }

            NbtWorld world = NbtWorld.Open(sourcePath);

            if (Directory.Exists(Path.Combine(outputPath, world.Level.LevelName)))
            {
                if (console)
                {
                    Console.WriteLine("World folder already exists, stopping.");
                }
                else
                {
                    MessageBox.Show("World folder exists already, stopping.");
                }
                
                return;
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(outputPath, world.Level.LevelName));
            }

            DirectoryCopy(sourcePath, Path.Combine(outputPath, world.Level.LevelName), true);


            INIFile iniFile = new INIFile(Path.Combine(outputPath, world.Level.LevelName, "world.ini"));
            iniFile.SetValue("General", "Gamemode", (int)world.Level.GameType);
            iniFile.SetValue("General", "TimeInTicks", world.Level.Time);
            iniFile.SetValue("SpawnPosition", "X", world.Level.Spawn.X);
            iniFile.SetValue("SpawnPosition", "Y", world.Level.Spawn.Y);
            iniFile.SetValue("SpawnPosition", "Z", world.Level.Spawn.Z);
            iniFile.SetValue("Seed", "Seed", world.Level.RandomSeed);

            if (File.Exists(Path.Combine(Directory.GetParent(sourcePath).FullName, "server.properties")))
            {
                IDictionary<string, string> serverProperties = ReadDictionaryFile(Path.Combine(Directory.GetParent(sourcePath).FullName, "server.properties"));
                iniFile.SetValue("Mechanics", "CommandBlocksEnabled", serverProperties["enable-command-block"] == "true" ? 1 : 0);
                iniFile.SetValue("Mechanics", "PVPEnabled", serverProperties["pvp"] == "true" ? 1 : 0);
                iniFile.SetValue("SpawnPosition", "MaxViewDistance", serverProperties["view-distance"]);
                iniFile.SetValue("SpawnProtect", "ProtectRadius", serverProperties["spawn-protection"]);
                iniFile.SetValue("Difficulty", "WorldDifficulty", serverProperties["difficulty"]);
            }

            iniFile.Flush();

            PlayerManager playerManager = (PlayerManager)world.GetPlayerManager();

            foreach (Player player in playerManager)
            {
                JObject rootObject = new JObject();
                rootObject.Add("SpawnX", player.Spawn.X == 0 ? world.Level.Spawn.X : player.Spawn.X);
                rootObject.Add("SpawnY", player.Spawn.Y == 0 ? world.Level.Spawn.Y : player.Spawn.Y);
                rootObject.Add("SpawnZ", player.Spawn.Z == 0 ? world.Level.Spawn.Z : player.Spawn.Z);
                rootObject.Add("air", player.Air);
                rootObject.Add("enderchestinventory", convertInventory(player.EnderItems, 27));
                rootObject.Add("food", player.HungerLevel);
                rootObject.Add("foodExhaustion", player.HungerExhaustionLevel);
                rootObject.Add("foodSaturation", player.HungerSaturationLevel);
                rootObject.Add("foodTickTimer", player.HungerTimer);
                rootObject.Add("gamemode", (int)player.GameType);
                rootObject.Add("health", player.Health);
                rootObject.Add("inventory", convertPlayerInventory(player.Items));
                rootObject.Add("isflying", player.Air);
                rootObject.Add("position", new JArray(player.Position.X, player.Position.Y, player.Position.Z));
                rootObject.Add("rotation", new JArray(player.Rotation.Yaw, player.Rotation.Pitch, 0.0));
                rootObject.Add("world", player.World);
                rootObject.Add("xpCurrent", player.XPLevel);
                rootObject.Add("xpTotal", player.XPTotal);

                string uuidPrefix = player.Name.Substring(0, 2);
                string outputFile = Path.Combine(playerPath, uuidPrefix, player.Name.Substring(2) + ".json");

                if (!Directory.Exists(Path.Combine(playerPath, uuidPrefix)))
                {
                    Directory.CreateDirectory(Path.Combine(playerPath, uuidPrefix));
                }

                StreamWriter writer = new StreamWriter(outputFile);
                writer.Write(rootObject.ToString());
                writer.Flush();
                writer.Close();
            }

            if (console)
            {
                Console.WriteLine("Done!");
            }
            else
            {
                MessageBox.Show("Done!");
            }
        }
    }
}
