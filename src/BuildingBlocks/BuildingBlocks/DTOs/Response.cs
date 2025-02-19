﻿using System.Text.Json.Serialization;

namespace BuildingBlocks.DTOs;

public class Response<T> where T : class
{
    public T Data { get; private set; }
    public int StatusCode { get; set; }

    [JsonIgnore]
    public bool IsSuccessful { get; private set; }
    public ErrorDto Error { get; private set; }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Response<T> Fail(ErrorDto errorDto, int statusCode)
    {
        return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
    }

    public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
    {
        return new Response<T> { Error = new ErrorDto(error: errorMessage, isShow: isShow), StatusCode = statusCode, IsSuccessful = false };
    }
}