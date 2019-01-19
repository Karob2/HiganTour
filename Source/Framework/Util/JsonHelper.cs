using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lichen.Util
{
    // TODO: DataContractJsonSerializer does not report unknown elements during deserialization (is there a setting for this?) and does not allow custom default values during deserialization. Consider switching to another JSON serialization library.
    public static class JsonHelper<T> where T : class
    {
        public static T Load(string path)
        {
            
            string json;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                json = streamReader.ReadToEnd();
            }
            T item = JsonConvert.DeserializeObject<T>(json);
            return item;
            
            /*
            byte[] json = null;
            FileStream fs = new FileStream(path, FileMode.Open);
            // TODO: Replace this unreliable byte size limit with a "file size too large" error.
            json = new byte[Math.Min(fs.Length, 32768)];
            fs.Read(json, 0, (int)fs.Length);
            //MemoryStream ms = new MemoryStream(json);
            MemoryStream ms = new MemoryStream(json.Length);
            // TODO: Improve this cheap way of removing single-line comments (that doesn't even consider alternative uses of //, such as characters inside of strings), or consider banning comments.
            RemoveComments(json, ms);
            ms.Seek(0, SeekOrigin.Begin);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            object obj = serializer.ReadObject(ms);
            ms.Close();
            T item = (T)obj;
            return item;
            */
        }

        public static void Save(string path, T item)
        {
            
            string json;
            json = JsonConvert.SerializeObject(item);
            using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
            {
                streamWriter.Write(json);
            }
            
            /*
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, item);
            byte[] json = ms.ToArray();
            ms.Close();
            //return Encoding.UTF8.GetString(json, 0, json.Length);
            FileStream fs = new FileStream(path, FileMode.Create);
            // TODO: Change the way the size limit works.
            fs.Write(json, 0, Math.Min(json.Length, 32768));
            */
        }

        static void RemoveComments(byte[] source, MemoryStream target)
        {
            int count = 0;
            bool commenting = false;
            while (count < source.Length)
            {
                if (commenting == true)
                {
                    if (source[count] == '\r' || source[count] == '\n')
                    {
                        commenting = false; // NOTE: We don't even need to seek past the newlines since the json parser will ignore them.
                    }
                    else
                    {
                        count++;
                    }
                }
                if (commenting == false)
                {
                    if (count + 1 < source.Length && source[count] == '/' && source[count + 1] == '/')
                    {
                        commenting = true;
                        count += 2;
                    }
                    else
                    {
                        target.WriteByte(source[count++]);
                    }
                }
            }
        }
    }
}
