using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "Time not set")]
        public DateTime time { get; set; }

        [Required(ErrorMessage ="Payment not set")]
        [Range(minimum: 1, maximum: 1000, ErrorMessage = "Payment should be between 1 and 1000")]
        public double payment { get; set; }
    }

    public class PaymentStorage
    {
        List<Payment> _data = new List<Payment>();

        private void Load()
        {
            string file = ConfigurationManager.AppSettings["StoragePath"];
            if (File.Exists(file))
            {
                _data = JsonConvert.DeserializeObject<List<Payment>>(File.ReadAllText(file));
            }
            else
            {
                _data = new List<Payment>();
            }
        }
        private void Save()
        {
            string file = ConfigurationManager.AppSettings["StoragePath"];
            File.WriteAllText(file, JsonConvert.SerializeObject(_data));
        }
        public PaymentStorage()
        {
            Load();
        }

        public int AddPayment(Payment value)
        {
            _data.Add(value);
            Save();
            return _data.Count;
        }

        public IEnumerable<Payment> GetEnumerator()
        {
            return _data.AsEnumerable();
        }

        public Payment GetPayment(int id)
        {
            return _data[id];
        }

        public void UpdatePayment(int id, Payment value)
        {
            _data[id] = value;
            Save();
        }

        public void RemovePayment(int id)
        {
            _data.RemoveAt(id);
        }
    }
}