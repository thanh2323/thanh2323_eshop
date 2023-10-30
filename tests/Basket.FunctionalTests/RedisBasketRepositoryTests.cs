﻿using eShop.Basket.API.Repositories;
using eShop.Basket.API.Model;
using eShop.Basket.FunctionalTests.Base;
using BasketItem = eShop.Basket.API.Model.BasketItem;

namespace eShop.Basket.FunctionalTests;

public class RedisBasketRepositoryTests
    : BasketScenarioBase
{

    [Fact]
    public async Task UpdateBasket_return_and_add_basket()
    {
        var server = CreateServer();
        var redis = server.Services.GetRequiredService<IConnectionMultiplexer>();

        var redisBasketRepository = BuildBasketRepository(redis);

        var basket = await redisBasketRepository.UpdateBasketAsync(new CustomerBasket
        {
            BuyerId = "buyerId",
            Items = BuildBasketItems()
        });

        Assert.NotNull(basket);
        Assert.Single(basket.Items);
    }

    [Fact]
    public async Task Delete_Basket_return_null()
    {
        var server = CreateServer();
        var redis = server.Services.GetRequiredService<IConnectionMultiplexer>();

        var redisBasketRepository = BuildBasketRepository(redis);

        var basket = await redisBasketRepository.UpdateBasketAsync(new CustomerBasket
        {
            BuyerId = "buyerId",
            Items = BuildBasketItems()
        });

        var deleteResult = await redisBasketRepository.DeleteBasketAsync("buyerId");

        var result = await redisBasketRepository.GetBasketAsync(basket.BuyerId);

        Assert.True(deleteResult);
        Assert.Null(result);
    }

    RedisBasketRepository BuildBasketRepository(IConnectionMultiplexer connMux)
    {
        var loggerFactory = new LoggerFactory();
        return new RedisBasketRepository(loggerFactory.CreateLogger<RedisBasketRepository>(), connMux);
    }

    List<BasketItem> BuildBasketItems()
    {
        return new List<BasketItem>()
        {
            new BasketItem()
            {
                Id = "basketId",
                PictureUrl = "pictureurl",
                ProductId = 1,
                ProductName = "productName",
                Quantity = 1,
                UnitPrice = 1
            }
        };
    }
}
