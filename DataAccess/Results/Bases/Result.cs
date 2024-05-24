﻿namespace DataAccess.Results.Bases
{
    public abstract class Result
    {
        public bool IsSuccessful { get; }
        public string Message { get; }

        protected Result(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}
