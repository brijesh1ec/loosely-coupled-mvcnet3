using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WebPlatformUnitTests
{
    public class JsonHelper
    {
        public static ResultSet Deserialize(string jsonString)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            System.Runtime.Serialization.Json.DataContractJsonSerializer dcjs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ResultSet));
            ResultSet data;
            try
            {
                data = (ResultSet)dcjs.ReadObject(ms);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                data = null;
            }
            return data;

        }
    }


    /// <summary>
    /// Product results JSON HELPER
    /// if you want to adopt it for other return types, refactor it or implement new
    /// </summary>
    [DataContract]
    public class JSONResultSet
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string error { get; set; }
    }

    public class Row
    {
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int Id { get; set; }
         [DataMember]
        public string ProductDescription { get; set; }
         [DataMember]
         public decimal UnitPrice { get; set; }
        public Row()
        {
        }
    }
    [DataContract]
    public class ResultSet
    {
        public ResultSet() { }

        public Row[] Rows { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int Page { get; set; }

        [DataMember]
        public int Records { get; set; }

        [DataMember(Name = "Rows")]
        Row[] data;

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.data != null)
            {
                this.Rows = new Row[data.Length];
                for (int i = 0; i < this.data.Length; i++)
                {
                    this.Rows[i] = new Row();
                    this.Rows[i].Id = this.data[i].Id;
                    this.Rows[i].ProductName = this.data[i].ProductName;
                    this.Rows[i].ProductDescription = this.data[i].ProductDescription;
                    this.Rows[i].UnitPrice = this.data[i].UnitPrice;

                }
            }
        }

    }
}
