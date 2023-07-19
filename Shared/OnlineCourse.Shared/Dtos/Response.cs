using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCourse.Shared.Dtos;

public class Response<T>
{
    public T Data { get; set; }

    [JsonIgnore]
    public int StatusCode { get; set; }

    [JsonIgnore]
    public bool IsSuccessful { get; set; }

    public List<string> Errors { get; set; }
    public string Message { get; set; }

    // Static Factory Method
    public static Response<T> Success(int statusCode, T data = default, string message = "")
    {
        return new Response<T>
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccessful = true,
            Message = message
        };
    }
    public static Response<T> Fail(List<string> errors, int statusCode)

    {
        return new Response<T>
        {
            Errors = errors,
            StatusCode = statusCode,
            IsSuccessful = false
        };
    }

    public static Response<T> Fail(string error, int statusCode)
    {
      return  Fail(new List<string>() { error }, statusCode);
    }
}

