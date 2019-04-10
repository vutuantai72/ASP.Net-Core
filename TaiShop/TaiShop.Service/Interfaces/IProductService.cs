﻿using System;
using System.Collections.Generic;
using System.Text;
using TaiShop.Service.ViewModels.Product;
using TaiShop.Utilities.Dtos;

namespace TaiShop.Service.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId,string keyword,int page,int pageSize);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void ImportExcel(string filePath, int categoryId);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void Save();

        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);
        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);
    }
}
