﻿using System;
using System.Threading.Tasks;

public class Cache
{
    public static String CACHE_FILE_NAME = "cache.dat";

    static private async void WriteToCache(String toWrite)
    {
        Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile cacheFile =
            await storageFolder.CreateFileAsync(CACHE_FILE_NAME, Windows.Storage.CreationCollisionOption.OpenIfExists);

        await Windows.Storage.FileIO.WriteTextAsync(cacheFile, toWrite);
    }

    static private async Task<String> ReadFromCache()
    {
        Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile cacheFile =
            await storageFolder.CreateFileAsync(CACHE_FILE_NAME, Windows.Storage.CreationCollisionOption.OpenIfExists);
        return await Windows.Storage.FileIO.ReadTextAsync(cacheFile);
    }

    static public async void SaveToCache(String SSID, String Pass)
    {
        String fromRead = await ReadFromCache();
        String toWrite = "";
        String lookForSSID = "*" + SSID + "*";
        int index1 = fromRead.IndexOf(lookForSSID);
        if (index1 == -1)
        {
            toWrite = fromRead + "*" + SSID + "*" + Pass + "*";
        }
        else
        {
            if ((fromRead.Length - index1) > 0)
            {
                int index2 = fromRead.IndexOf("*", (index1 + lookForSSID.Length));
                if (index2 != -1)
                {
                    toWrite = fromRead.Substring(0, index1) + "*" + SSID + "*" + Pass + "*" + fromRead.Substring(index2, fromRead.Length - index2);
                }
            }
        }
        WriteToCache(toWrite);
    }

    static public async Task<String> GetPassFromCache(String SSID)
    {
        String fromRead = await ReadFromCache();
        String lookForSSID = "*" + SSID + "*";
        int index1 = fromRead.IndexOf(lookForSSID);
        if ((index1 != -1) && ((fromRead.Length - index1 + lookForSSID.Length) > 0))
        {
            int index2 = fromRead.IndexOf("*", index1 + lookForSSID.Length);
            if (index2 != -1)
            {
                return fromRead.Substring((index1 + lookForSSID.Length), index2 - (index1 + lookForSSID.Length));
            }
        }
        return "";
    }
}