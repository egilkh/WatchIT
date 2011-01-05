using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

	public class Serialization {

		public string UTF8ByteArrayToString (byte[] characters) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			string constructedString = encoding.GetString(characters);
			return(constructedString);
		}

		public byte[] StringToUTF8ByteArray (string xmlString) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			byte[] byteArray = encoding.GetBytes(xmlString);
			return(byteArray);
		}

		public string SerializeObject (Object o) {
			try {
				string xmlString = null;
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(o.GetType());
				System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(ms, System.Text.Encoding.UTF8);
				xs.Serialize(xw, o);
				ms = (System.IO.MemoryStream)xw.BaseStream;
				xmlString = this.UTF8ByteArrayToString(ms.ToArray());
				return (xmlString);
			} catch (Exception e) {
				System.Console.WriteLine("SerializeObject: Exception when serializing object. Message: " + e.Message);
				return(null);
			}
		}

		public T DeserializeObject<T> (string xmlString) where T : new () {
			try {
				System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
				System.IO.MemoryStream ms = new System.IO.MemoryStream(this.StringToUTF8ByteArray(xmlString));
				return ((T)xs.Deserialize(ms));
			} catch (Exception e) {
				System.Console.WriteLine("SerializeObject: Exception when deserializing object. Message: " + e.Message);
				return (new T());
			}
		} 

	}
}
