﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Tealium.Utility
{
    /// <summary>
    /// Internal utility for caching content to local storage.
    /// </summary>
    /// <remarks>
    /// reference: http://www.sharpgis.net/post/2012/01/12/Reading-and-Writing-text-files-in-Windows-8-Metro.aspx
    /// </remarks>
    internal class StorageHelper
    {

        public static async Task<bool> Save<T>(T data, string fileName)
        {
            try
            {
                using (
                    var file =
                        await
                            ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                                CreationCollisionOption.ReplaceExisting))
                {
                    if (file != null && file.CanWrite)
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof (T));
                        serializer.WriteObject(file, data);
                        await file.FlushAsync();
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                TealiumStatusLog.Error(ex.Message);
            }

            return false;
        }

        public static async Task<T> Load<T>(string fileName)
        {
            try
            {
                using (var file = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    if (file != null && file.CanRead)
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof (T));
                        var contents = serializer.ReadObject(file);

                        return (T) contents;
                    }
                }
            }
            catch (Exception ex)
            {
                TealiumStatusLog.Error(ex.Message);
            }

            return default(T);
        }


        //private static string SerializeObjectGraph(object graph)
        //{
        //    if (graph == null) return null;
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
        //    MemoryStream ms = new MemoryStream();
        //    ser.WriteObject(ms, graph);
        //    var bytes = ms.ToArray();
        //    return UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //}

    }
}
