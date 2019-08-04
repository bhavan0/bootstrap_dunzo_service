using Domain.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository;
using Repository.Entity;

namespace Domain
{
    public interface IImageService
    {
        Task<ParsedData> UploadImage(JsonDto imageMeta);

        bool SaveData(ParsedData data);

        MetaData GetAllMetaData();

        IList<GridDataViewModel> GetAllData();
    }

    public class ImageService : IImageService
    {
        private readonly IInventoryRepository _repo;

        public ImageService(IInventoryRepository repo)
        {
            _repo = repo;
        }

        // Call the R Api to get data
        public async Task<ParsedData> UploadImage(JsonDto imageMeta)
        {
            // Send file to R and get string.
            const string URL = "http://127.0.0.1:7082/ImageToText";
            string data = null;

            string image = JsonConvert.SerializeObject(imageMeta);

            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "d7dd6116-67d5-bafe-91fc-29f73a61f0da");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("undefined", image, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            data = response.Content.ToString();

            ParsedData parsedData = ParseImageData(data);

            return parsedData;
        }

        // Save Data in Db
        public bool SaveData(ParsedData data)
        {
            IList<Product> products = new List<Product>();
            foreach (var item in data.Products)
            {
                var product = new Product
                {
                    ProductName = item.Key,
                    ProductPrice = item.Value
                };
                products.Add(product);
            }
            var parseData = new InventoryData
            {
                Products = products,
                ShopName = "Rohilya Foods Pvt Ltd",
                TotalPrice = data.TotalPrice
            };
            return _repo.AddProduct(parseData);            
        }

        // Return Details about no of bills parsed
        public MetaData GetAllMetaData()
        {
            MetaData metaData = _repo.GetMetaData();

            return metaData;
        }

        public IList<GridDataViewModel> GetAllData()
        {
            var data = _repo.GetAll();
            var list = new List<GridDataViewModel>();
            foreach (var item in data)
            {
                var itm = new GridDataViewModel()
                {
                    Price = item.ProductPrice,
                    ProductName = item.ProductName,
                    ShopName = item.InventoryData.ShopName
                };
                list.Add(itm);
            }
            return list;
        }
        
        private ParsedData ParseImageData(string mainData)
        {
            string[] data = mainData.Split("\\n");

            Dictionary<string, double> productDetails = new Dictionary<string, double>();
            List<string> headerMetaData =
                new List<string>(new string[] { "tten", "ttem", "iten", "item", "qty", "price", "amount", "total", "name" });
            double totalPrice = 0;
            bool foundHeader = false;
            bool foundTotalLine = false;

            foreach (var line in data)
            {
                var words = line.Split();
                var wrds = words.Select(x => x.ToLower()).ToList();
                if (!foundHeader)
                {
                    var commonMetaCount = wrds.Intersect(headerMetaData).Count();
                    if (commonMetaCount > 0)
                        foundHeader = true;
                    continue;
                }
                if (foundHeader)
                {
                    var count = 0;
                    double price = 0;
                    string productName = String.Empty;
                    foreach (var word in words)
                    {

                        if (Double.TryParse(word, out double prc))
                        {
                            count++;
                            price = prc;
                        }
                        else
                            productName = productName + " " + word;
                    }
                    if (count >= 2)
                        productDetails.Add(productName, price);
                }

                if (!foundTotalLine)
                    foreach (var word in words)
                    {
                        if (word.ToLower().Contains("total"))
                        {
                            foundTotalLine = true;
                            continue;
                        }
                        if (foundTotalLine)
                        {
                            if (Double.TryParse(word, out double totprc))
                            {
                                totalPrice = totprc;
                                break;
                            }
                        }
                    }
            }

            return new ParsedData()
            {
                Products = productDetails,
                TotalPrice = totalPrice
            };
        }
    }
}
