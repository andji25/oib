using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using petShop.Model;

namespace petShop.Repository
{
    public class JsonReceiptRepository : IReceiptRepository
    {
        private const string FilePath = "receipts.json";
        public void Add(Receipt receipt)
        {
            var receipts = GetAll();
            receipts.Add(receipt);
            Save(receipts);
        }
        public List<Receipt> GetAll()
        {
            if (!File.Exists(FilePath))
                return new List<Receipt>();

            var json = File.ReadAllText(FilePath);
            var receipts = JsonSerializer.Deserialize<List<Receipt>>(json);

            return receipts ?? new List<Receipt>();
        }

        private void Save(List<Receipt> receipts)
        {
            var json = JsonSerializer.Serialize(receipts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
