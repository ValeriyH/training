using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JsonConvertTest
{
    public class DataResponse
    {
        public class Status
        {
            public int code;
        }
        public class Meta
        {
            public Status status;
        }

        public JArray data;
        public JArray errors;
        public Meta meta;

        public bool HasError()
        {
            return errors != null || data == null || meta == null || meta.status == null || meta.status.code != 200;
        }

        public string GetError()
        {
            if (errors == null)
            {
                if (data == null)
                {
                    return "No data available";
                }
                else if (meta == null || meta.status == null)
                {
                    return "No meta data status available";
                }
                else
                {
                    if (meta.status.code == 200)
                    {
                        return "Status OK";
                    }
                    else
                    {
                        return $"Status code {meta.status.code}";
                    }
                }
            }
            else
            {
                return errors.Select(t => t["details"].ToString()).Aggregate((i, j) => i + "\n" + j);

            }
            throw new NotImplementedException("Not yet implemented");
        }
    }

    public class ExchangeSummary
    {
        public DateTime time {get;set;}
        public long id { get; set; }
        public long AdvancingCount { get; set; }

        //public long DecliningCount { get; set; }

        //public long UnchangedCount { get; set; }

        //public long TotalCount { get; set; }

        public double AdvancingVolume { get; set; }

        //public double DecliningVolume { get; set; }

        //public double UnchangedVolume { get; set; }

        //public double TotalVolume { get; set; }

        //public long New52WeekHighCount { get; set; }

        //public long New52WeekLowCount { get; set; }

        //public DateTime DateTime { get; set; }
    }

    public class ExchangeSummaryResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public ExchangeSummaryResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
        {
            {"AdvancingCount", "advancing.count"},
            {"LastUpdated", "last_updated"},
            {"Disclaimer", "disclaimer"},
            {"License", "license"},
            {"CountResults", "results"},
            {"Term", "term"},
            {"Count", "count"},
        };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }

        //
        // Summary:
        //     Resolves the contract for a given type.
        //
        // Parameters:
        //   type:
        //     The type to resolve a contract for.
        //
        // Returns:
        //     The contract for a given type.
        public override JsonContract ResolveContract(Type type)
        {
            var ret =  base.ResolveContract(type);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonArrayContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonArrayContract for the given type.
        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            var ret = base.CreateArrayContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates the constructor parameters.
        //
        // Parameters:
        //   constructor:
        //     The constructor to create properties for.
        //
        //   memberProperties:
        //     The type's member properties.
        //
        // Returns:
        //     Properties for the given System.Reflection.ConstructorInfo.
        protected override IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
        {
            var ret = base.CreateConstructorParameters(constructor, memberProperties);
            return ret;
        }
        //
        // Summary:
        //     Determines which contract type is created for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonContract for the given type.
        protected override JsonContract CreateContract(Type objectType)
        {
            var ret = base.CreateContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonDictionaryContract for the given
        //     type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonDictionaryContract for the given type.
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            var ret = base.CreateDictionaryContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonDynamicContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonDynamicContract for the given type.
        protected override JsonDynamicContract CreateDynamicContract(Type objectType)
        {
            var ret = base.CreateDynamicContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonISerializableContract for the given
        //     type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonISerializableContract for the given type.
        protected override JsonISerializableContract CreateISerializableContract(Type objectType)
        {
            var ret = base.CreateISerializableContract(objectType);
            return ret;
        }

        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonLinqContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonLinqContract for the given type.
        protected override JsonLinqContract CreateLinqContract(Type objectType)
        {
            var ret = base.CreateLinqContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates the Newtonsoft.Json.Serialization.IValueProvider used by the serializer
        //     to get and set values from a member.
        //
        // Parameters:
        //   member:
        //     The member.
        //
        // Returns:
        //     The Newtonsoft.Json.Serialization.IValueProvider used by the serializer to get
        //     and set values from a member.
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            var ret = base.CreateMemberValueProvider(member);
            return ret;
        }

        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonObjectContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonObjectContract for the given type.
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var ret = base.CreateObjectContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonPrimitiveContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonPrimitiveContract for the given type.
        protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            var ret = base.CreatePrimitiveContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Creates properties for the given Newtonsoft.Json.Serialization.JsonContract.
        //
        // Parameters:
        //   type:
        //     The type to create properties for.
        //
        //   memberSerialization:
        //     The member serialization mode for the type.
        //
        // Returns:
        //     Properties for the given Newtonsoft.Json.Serialization.JsonContract.
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var ret = base.CreateProperties(type, memberSerialization);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.
        //
        // Parameters:
        //   memberSerialization:
        //     The member's parent Newtonsoft.Json.MemberSerialization.
        //
        //   member:
        //     The member to create a Newtonsoft.Json.Serialization.JsonProperty for.
        //
        // Returns:
        //     A created Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var ret = base.CreateProperty(member, memberSerialization);
            return ret;
        }

        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.ParameterInfo.
        //
        // Parameters:
        //   matchingMemberProperty:
        //     The matching member property.
        //
        //   parameterInfo:
        //     The constructor parameter.
        //
        // Returns:
        //     A created Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.ParameterInfo.
        protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
        {
            var ret = base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
            return ret;
        }
        //
        // Summary:
        //     Creates a Newtonsoft.Json.Serialization.JsonStringContract for the given type.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     A Newtonsoft.Json.Serialization.JsonStringContract for the given type.
        protected override JsonStringContract CreateStringContract(Type objectType)
        {
            var ret = base.CreateStringContract(objectType);
            return ret;
        }
        //
        // Summary:
        //     Gets the serializable members for the type.
        //
        // Parameters:
        //   objectType:
        //     The type to get serializable members for.
        //
        // Returns:
        //     The serializable members for the type.
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var ret = base.GetSerializableMembers(objectType);
            return ret;
        }
        //
        // Summary:
        //     Resolves the default Newtonsoft.Json.JsonConverter for the contract.
        //
        // Parameters:
        //   objectType:
        //     Type of the object.
        //
        // Returns:
        //     The contract's default Newtonsoft.Json.JsonConverter.
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            var ret = base.ResolveContractConverter(objectType);
            return ret;
        }
        //
        // Summary:
        //     Resolves the key of the dictionary. By default Newtonsoft.Json.Serialization.DefaultContractResolver.ResolvePropertyName(System.String)
        //     is used to resolve dictionary keys.
        //
        // Parameters:
        //   dictionaryKey:
        //     Key of the dictionary.
        //
        // Returns:
        //     Resolved key of the dictionary.
        protected override string ResolveDictionaryKey(string dictionaryKey)
        {
            var ret = base.ResolveDictionaryKey(dictionaryKey);
            return ret;
        }
        //
        // Summary:
        //     Resolves the name of the extension data. By default no changes are made to extension
        //     data names.
        //
        // Parameters:
        //   extensionDataName:
        //     Name of the extension data.
        //
        // Returns:
        //     Resolved name of the extension data.
        protected override string ResolveExtensionDataName(string extensionDataName)
        {
            var ret = base.ResolveExtensionDataName(extensionDataName);
            return ret;
        }
        //
        // Summary:
        //     Resolves the name of the property.
        //
        // Parameters:
        //   propertyName:
        //     Name of the property.
        //
        // Returns:
        //     Resolved name of the property.
        //protected virtual string ResolvePropertyName(string propertyName);
    }

    class Program
    {
        #region ExchangeSummary
        static string ex_summary = @"
            {
            ""data"": [
            {
            ""time"" : ""2018-11-06T14:08:00Z"",
            ""id"": 15,
            ""name"": """",
            ""advancing"": {
            ""count"": 2,
            ""volume"": 20
            },
            ""declining"": {
            ""count"": 3,
            ""volume"": 30
            },
            ""unchanged"": {
            ""count"": 4,
            ""volume"": 40
            },
            ""total"": {
            ""count"": 5,
            ""volume"": 50
            },
            ""high52WeekCount"": 6,
            ""low52WeekCount"": 60
            }
            ],
            ""meta"": {
            ""status"": {
            ""code"": 200
            }
            }
            }
        ";

        static string ex_summary2 = @"
            {
            ""data"": [
                {
                ""id"": 15,
                ""name"": """",
                ""time"": ""2018-11-06T14:08:00Z"",
                ""advancing"": {
                    ""count"": 2,
                    ""volume"": 20
                    },
                }
            ],
            ""meta"": {
                ""page"" : {
                    ""next"" : ""next page"",
                    ""total"" : 10
                },
                ""status"": {
                    ""code"": ""200""
                    }
                }
            }
        ";
        #endregion

        #region Error
        static string error_response = @"
            {
            ""errors"": [
                {
                    ""details"": ""Array has less (0) then the minimum allowed (1) elements: data.filter.markets"",
                    ""encryptedDetails"": """",
                    ""type"": 5,
                    ""attribute"": [
                        {
                        ""name"": ""data"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""filter"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""markets"",
                        ""index"": -1
                        }
                    ]
                },
                {
                    ""details"": ""Error !2"",
                    ""encryptedDetails"": """",
                    ""type"": 5,
                    ""attribute"": [
                        {
                        ""name"": ""data"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""filter"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""markets"",
                        ""index"": -1
                        }
                    ]
                },
                {
                    ""details"": ""Error !3"",
                    ""encryptedDetails"": """",
                    ""type"": 5,
                    ""attribute"": [
                        {
                        ""name"": ""data"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""filter"",
                        ""index"": -1
                        },
                        {
                        ""name"": ""markets"",
                        ""index"": -1
                        }
                    ]
                }


            ],
            ""meta"": {
            ""status"": {
            ""code"": 400
            }
            }
            }
        ";
        #endregion


        static void Main(string[] args)
        {
            dynamic build = new JObject();
            build.Test = 19;
            build.Data = new JObject();
            build.Data.Test2 = 15;
            var startDay = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            build.Data.Date = startDay;

            build.Empty = new JObject();

            Console.WriteLine(build.ToString());

            build.Data.Test2 = 1;
            build.Data.Id = 232;

            Console.WriteLine(build.ToString());

            //var settings = new JsonSerializerSettings();
            //settings.DateFormatString = "YYYY-MM-DD";
            //settings.ContractResolver = new ExchangeSummaryResolver();

            //var response = JsonConvert.DeserializeObject<DataResponse<ExchangeSummary>>(ex_summary2, settings);
            //Console.WriteLine("Hello World!");

            var response1 = JsonConvert.DeserializeObject<DataResponse>(ex_summary2);



            //var err_response = JsonConvert.DeserializeObject<DataResponse<ExchangeSummary>>(error_response);
            //Console.WriteLine(err_response.GetError());

            Func<JToken, ExchangeSummary> convert = (
                token => {
                    var advancing = token.Value<JToken>("advancing");
                    return new ExchangeSummary
                    {
                        time = token.Value<DateTime>("time"),
                        id = token.Value<long>("id"),
                        AdvancingCount = advancing.Value<long?>("count") ?? 0,
                        AdvancingVolume = advancing.Value<long?>("volume") ?? 0,
                    };
                }
            );

            JObject o = JObject.Parse(ex_summary2);

            JToken test = o.SelectToken("meta.status");
            string st = test.Value<string>("code");
            int int1 = test.Value<int>("code");

            var data_s = o["data"].Select(convert);
            var data = data_s.ToArray();
            JToken jNext = o.SelectToken("meta.page.next");
            string next = (string)jNext;
            if (jNext != null)
            {
                jNext.Replace("new page");
            }
            int? total = (int?)o.SelectToken("meta.page.total");

            Console.WriteLine(o);


            var resp = JsonConvert.DeserializeObject<DataResponse>(ex_summary);
            var data2 = resp.data.Select(convert).ToArray();
            //int? a = (int?)resp.data["test"];
            var data3 = resp.data.Select(x => x.Value<string>("id"));

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estern = TimeZoneInfo.ConvertTimeFromUtc(data2[0].time, timeZone);
            Console.WriteLine($"Eastern time {TimeZoneInfo.ConvertTimeFromUtc(data2[0].time, timeZone)}");

            string s = timeZone.ToSerializedString();
            var tz = TimeZoneInfo.FromSerializedString(s);

            string serial = "Eastern Standard Time;-300;(UTC-05:00) Eastern Time (US & Canada);Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
            var tz2 = TimeZoneInfo.FromSerializedString(serial);
        }
    }
}
